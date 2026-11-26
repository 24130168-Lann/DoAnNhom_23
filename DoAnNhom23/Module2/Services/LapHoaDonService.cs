using DoAnNhom23.Module2.Enums;
using DoAnNhom23.Module2.Exceptions;
using DoAnNhom23.Module2.Models;
using DoAnNhom23.Module2.Results;

namespace DoAnNhom23.Module2.Services;

public static class LapHoaDonService
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

        // HD-04: >0 chờ thanh toán; <=0 đã tất toán ngay khi lập 
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

    // HD-07: đúng một lệnh hoàn cho mỗi đặt phòng; mã xác định theo mã đặt phòng nên retry không sinh mã mới
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