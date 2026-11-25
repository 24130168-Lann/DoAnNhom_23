namespace DoAnNhom23.Module2;

// có 2 trạng thái: CHO_THANH_TOAN, DA_THANH_TOAN
// hoàn cọc dư xử lý ngay khi lập hóa đơn
public enum TrangThaiHoaDon
{
    CHO_THANH_TOAN,
    DA_THANH_TOAN
}

// Lệnh hoàn cọc dư mô phỏng, ghi lên DAT_PHONG trước khi INSERT HOA_DON (HD-07)
public record LenhHoanCoc(
    string MaHoanCoc,
    long SoTien,
    DateTime ThoiDiem,
    string NoiDung,
    string TkHoanTen,
    string TkHoanSo,
    string TkHoanNganHang);

// Ảnh chụp hóa đơn (HD-08: lập xong không sửa, không phụ thuộc giá hiện hành)
public record KetQuaHoaDon(
    string MaHoaDon,
    string MaDatPhong,
    DateTime ThoiDiemLap,
    long TongTienPhong,
    long TongTienDichVu,
    long TongCong,
    long CocGiuLai,          // tổng coc_giu_lai của các dòng NO_SHOW
    long TienCocConLai,      // tien_coc - coc_giu_lai (cọc khả dụng)
    long TienCocCanTru,      // cọc thực tế cấn trừ = MIN(tong_cong, tien_coc_con_lai)
    long SoTienConLai,       // có dấu: dương = khách trả, 0 = tất toán, âm = hoàn cọc dư
    long SoTienHoan,         // MAX(0, -so_tien_con_lai)
    TrangThaiHoaDon TrangThai,
    DateTime? ThoiDiemThanhToan,
    LenhHoanCoc? LenhHoan)
{
    // Khách còn phải trả bao nhiêu (0 nếu tiền cọc = tiền phải trả => không có dư)
    public long SoTienPhaiTra => Math.Max(0, SoTienConLai);

    // Chỉ có nút thanh toán/QR khi CHO_THANH_TOAN và còn dương (CO-05)
    public bool CanThanhToan => TrangThai == TrangThaiHoaDon.CHO_THANH_TOAN && SoTienConLai > 0;
}

// chỉ đọc, đủ để quyết định và tính hóa đơn
public record DatPhongTinhHoaDon(
    string MaDatPhong,
    string TenNguoiDat,
    TrangThaiDatPhong TrangThai,
    bool VangLai,
    long TienCoc,
    bool DaCoHoaDon,
    IReadOnlyList<DongPhongTinhHoaDon> CacDong,
    string? TkHoanTen = null,
    string? TkHoanSo = null,
    string? TkHoanNganHang = null);

public enum LyDoChuaLapHoaDon
{
    KhongCo,                   // đủ điều kiện
    DaCoHoaDon,                // đã có hóa đơn: trả hóa đơn hiện có, không lập thêm
    DatPhongKhongDangO,
    KhongCoDongNaoCheckin,     // thuộc nhánh DA_HUY của Module 1, không lập hóa đơn
    CoDongChuaKetThuc          // còn dòng DANG_O hoặc CHUA_NHAN chưa được Module 1 xử lý no-show
}

public record DieuKienLapHoaDon(
    bool DuDieuKien,
    LyDoChuaLapHoaDon LyDo,
    IReadOnlyList<int> DongDangO,
    IReadOnlyList<int> DongChuaNhan);

// CO-04: logic thuần, không ghi DB. Application Service gọi sau CO-03 và sau sự kiện DP-09 (HD-10),
// rồi ghi lệnh hoàn (nếu có) -> INSERT HOA_DON -> đặt phòng DA_TRA trong cùng một transaction (HD-11)
public static class LapHoaDon
{
    private static readonly IReadOnlyList<int> Rong = Array.Empty<int>();

    public static DieuKienLapHoaDon KiemTraDieuKien(DatPhongTinhHoaDon dat)
    {
        ArgumentNullException.ThrowIfNull(dat);
        ArgumentNullException.ThrowIfNull(dat.CacDong);

        if (dat.DaCoHoaDon)
            return new DieuKienLapHoaDon(false, LyDoChuaLapHoaDon.DaCoHoaDon, Rong, Rong);
        if (dat.TrangThai != TrangThaiDatPhong.DANG_O)
            return new DieuKienLapHoaDon(false, LyDoChuaLapHoaDon.DatPhongKhongDangO, Rong, Rong);

        var dangO = new List<int>();
        var chuaNhan = new List<int>();
        int soDaTra = 0;
        foreach (var dong in dat.CacDong)
        {
            switch (dong.XacDinhTrangThai())
            {
                case TrangThaiDongPhong.DANG_O: dangO.Add(dong.Id); break;
                case TrangThaiDongPhong.CHUA_NHAN: chuaNhan.Add(dong.Id); break;
                case TrangThaiDongPhong.DA_TRA: soDaTra++; break;
                // NO_SHOW: đã được Module 1 xử lý hợp lệ, chỉ đọc
            }
        }

        // HD-02: không tự bỏ qua, không tự hủy dòng chưa check-in
        if (dangO.Count > 0 || chuaNhan.Count > 0)
            return new DieuKienLapHoaDon(false, LyDoChuaLapHoaDon.CoDongChuaKetThuc, dangO, chuaNhan);
        if (soDaTra == 0)
            return new DieuKienLapHoaDon(false, LyDoChuaLapHoaDon.KhongCoDongNaoCheckin, Rong, Rong);

        return new DieuKienLapHoaDon(true, LyDoChuaLapHoaDon.KhongCo, Rong, Rong);
    }
        //điều kiện cho lập hoá đơn 
    public static string MoTaLyDo(DieuKienLapHoaDon dk) => dk.LyDo switch
    {
        LyDoChuaLapHoaDon.KhongCo => "Đủ điều kiện lập hóa đơn.",
        LyDoChuaLapHoaDon.DaCoHoaDon => "Đặt phòng đã có hóa đơn.",
        LyDoChuaLapHoaDon.DatPhongKhongDangO => "Đặt phòng không ở trạng thái đang ở.",
        LyDoChuaLapHoaDon.KhongCoDongNaoCheckin => "Đặt phòng chưa có dòng phòng nào check-in nên không lập hóa đơn.",
        _ => $"Chưa đủ điều kiện lập hóa đơn: còn {dk.DongDangO.Count} phòng đang ở và " +
             $"{dk.DongChuaNhan.Count} phòng chưa check-in chưa được xác nhận no-show."
    };

    // Lập hóa đơn. Ném InvalidOperationException nếu chưa đủ điều kiện,
    // DuLieuKhongNhatQuanException nếu dữ liệu vi phạm bất biến (khi đó không tạo hóa đơn)
    public static KetQuaHoaDon Lap(DatPhongTinhHoaDon dat, DateTime thoiDiemLap)
    {
        var dieuKien = KiemTraDieuKien(dat);
        if (!dieuKien.DuDieuKien)
            throw new InvalidOperationException(MoTaLyDo(dieuKien));

        if (dat.TienCoc < 0)
            throw new DuLieuKhongNhatQuanException("Tiền cọc không được âm.");
        if (dat.VangLai && dat.TienCoc != 0)
            throw new DuLieuKhongNhatQuanException("Khách vãng lai không có cọc (HD-09).");

        long tongPhong = 0, tongDichVu = 0, cocGiu = 0;
        foreach (var dong in dat.CacDong)
        {
            switch (dong.XacDinhTrangThai())
            {
                case TrangThaiDongPhong.DA_TRA:
                    if (dong.TienPhong is not long tienPhong || dong.TienDichVu is not long tienDichVu)
                        throw new DuLieuKhongNhatQuanException($"Dòng phòng {dong.Id} đã trả nhưng chưa chốt tien_phong/tien_dich_vu.");
                    if (tienPhong < 0 || tienDichVu < 0)
                        throw new DuLieuKhongNhatQuanException($"Dòng phòng {dong.Id}: tiền phòng/dịch vụ không được âm.");
                    tongPhong = checked(tongPhong + tienPhong);
                    tongDichVu = checked(tongDichVu + tienDichVu);
                    break;
                case TrangThaiDongPhong.NO_SHOW:
                    cocGiu = checked(cocGiu + dong.CocGiuLai);
                    break;
            }
        }

        // cọc giữ vượt cọc ban đầu thì dữ liệu không nhất quán, dừng
        if (cocGiu > dat.TienCoc)
            throw new DuLieuKhongNhatQuanException(
                $"Tổng coc_giu_lai ({DinhDang.Tien(cocGiu)}) vượt tiền cọc ban đầu ({DinhDang.Tien(dat.TienCoc)}).");

        long tienCocConLai = dat.TienCoc - cocGiu;
        long tongCong = checked(tongPhong + tongDichVu);
        long soTienConLai = tongCong - tienCocConLai;
        long tienCocCanTru = Math.Min(tongCong, tienCocConLai);
        long soTienHoan = Math.Max(0, -soTienConLai);

        // HD-04: >0 chờ thanh toán; <=0 đã tất toán ngay khi lập (thoi_diem_thanh_toan để NULL theo CHECK của HOA_DON)
        var trangThai = soTienConLai > 0 ? TrangThaiHoaDon.CHO_THANH_TOAN : TrangThaiHoaDon.DA_THANH_TOAN;
        var lenhHoan = soTienHoan > 0 ? TaoLenhHoan(dat, soTienHoan, thoiDiemLap) : null;

        return new KetQuaHoaDon(
            MaHoaDon: "HD-" + dat.MaDatPhong,
            MaDatPhong: dat.MaDatPhong,
            ThoiDiemLap: thoiDiemLap,
            TongTienPhong: tongPhong,
            TongTienDichVu: tongDichVu,
            TongCong: tongCong,
            CocGiuLai: cocGiu,
            TienCocConLai: tienCocConLai,
            TienCocCanTru: tienCocCanTru,
            SoTienConLai: soTienConLai,
            SoTienHoan: soTienHoan,
            TrangThai: trangThai,
            ThoiDiemThanhToan: null,
            LenhHoan: lenhHoan);
    }

    // HD-07: đúng một lệnh hoàn cho mỗi đặt phòng; mã xác định theo mã đặt phòng không retry
    private static LenhHoanCoc TaoLenhHoan(DatPhongTinhHoaDon dat, long soTienHoan, DateTime thoiDiem)
    {
        if (string.IsNullOrWhiteSpace(dat.TkHoanTen) || string.IsNullOrWhiteSpace(dat.TkHoanSo)
            || string.IsNullOrWhiteSpace(dat.TkHoanNganHang))
            throw new DuLieuKhongNhatQuanException("Thiếu thông tin tài khoản nhận hoàn cọc; không thể tạo lệnh hoàn.");
        if (string.IsNullOrWhiteSpace(dat.TenNguoiDat))
            throw new DuLieuKhongNhatQuanException("Thiếu tên người đặt để tạo nội dung hoàn cọc.");

        string noiDung = $"HOAN TRA COC CHO QUY KHACH {DinhDang.BoDauVietHoa(dat.TenNguoiDat)} {DinhDang.BoDauVietHoa(dat.MaDatPhong)}";
        return new LenhHoanCoc("REF-" + dat.MaDatPhong, soTienHoan, thoiDiem, noiDung,
            dat.TkHoanTen.Trim(), dat.TkHoanSo.Trim(), dat.TkHoanNganHang.Trim());
    }
}
