using System.Globalization;
using System.Text;

namespace DoAnNhom23.Module2;

public enum TrangThaiHoaDon
{
    CHO_THANH_TOAN, // so_tien_con_lai > 0 và chưa bấm thanh toán
    DA_THANH_TOAN   // so_tien_con_lai <= 0 ngay khi lập, hoặc đã thanh toán 
}

// Trạng thái một dòng phòng
public enum TrangThaiDongPhong
{
    CHUA_NHAN, // chưa check-in
    DANG_O,    // đã check-in, chưa check-out
    DA_TRA,    // đã check-out, đã chốt tiền
    NO_SHOW    // chưa check-in, huy_luc do Module 1 ghi (DP-09)
}

// dữ liệu của một dòng phòng phục vụ cho hoá đơn => chỉ đọc , không ghi
public record DongPhongHoaDon(
    DateTime? CheckinThucTe,
    DateTime? CheckoutThucTe,
    DateTime? HuyLuc,
    long? TienPhong,
    long? TienDichVu,
    long CocGiuLai = 0)
{
    public TrangThaiDongPhong TrangThai
    {
        get
        {
            if (CheckinThucTe.HasValue && HuyLuc.HasValue)
                throw new InvalidOperationException("Dữ liệu không nhất quán: dòng phòng vừa check-in vừa bị hủy");
            if (CheckoutThucTe.HasValue && !CheckinThucTe.HasValue)
                throw new InvalidOperationException("Dữ liệu không nhất quán: có check-out nhưng chưa check-in");

            if (CheckinThucTe.HasValue)
                return CheckoutThucTe.HasValue ? TrangThaiDongPhong.DA_TRA : TrangThaiDongPhong.DANG_O;
            return HuyLuc.HasValue ? TrangThaiDongPhong.NO_SHOW : TrangThaiDongPhong.CHUA_NHAN;
        }
    }
}

public enum KetQuaDieuKienHoaDon
{
    DuDieuKien,
    DaCoHoaDon,          // trả hóa đơn hiện có
    DatPhongKhongDangO,
    ChuaCoDongCheckin,   // đặt phòng chưa từng check-in, không lập hóa đơn( trường hợp khách huỷ )
    ConDongChuaKetThuc   // còn dòng DANG_O hoặc CHUA_NHAN 
}

// hóa đơn tại thời điểm lập 
public record KetQuaHoaDon(
    long TongTienPhong,
    long TongTienDichVu,
    long TienCocBanDau,   // DAT_PHONG.tien_coc
    long CocGiuLai,       // tổng coc_giu_lai của các dòng NO_SHOW
    long TienCocConLai,   // HOA_DON.tien_coc_con_lai = tien_coc - coc_giu_lai
    long SoTienConLai,    // HOA_DON.so_tien_con_lai, có dấu: dương phải thu, 0 tất toán, âm hoàn dư
    TrangThaiHoaDon TrangThai)
{
    public long TongCong => checked(TongTienPhong + TongTienDichVu);

    // số cọc thực tế cấn trừ = MIN(tong_cong, tien_coc_con_lai) 
    public long TienCocCanTru => Math.Min(TongCong, TienCocConLai);

    public long SoTienPhaiTra => Math.Max(0, SoTienConLai);

    // số hoàn dư = MAX(0, -so_tien_con_lai)
    public long SoTienHoan => Math.Max(0, -SoTienConLai);

    // true: phải ghi lệnh hoàn duy nhất vào DAT_PHONG trước INSERT HOA_DON
    public bool CanTaoLenhHoan => SoTienHoan > 0;
}

public class LapHoaDon
{
    // Điều kiện lập hóa đơn. Chỉ đọc, không có quyền ghi
    public static KetQuaDieuKienHoaDon KiemTraDieuKien(
        TrangThaiDatPhong trangThaiDatPhong,
        bool daCoHoaDon,
        IEnumerable<DongPhongHoaDon> cacDong)
    {
        ArgumentNullException.ThrowIfNull(cacDong);
        var dongs = cacDong.ToList();

        if (daCoHoaDon) return KetQuaDieuKienHoaDon.DaCoHoaDon;
        if (trangThaiDatPhong != TrangThaiDatPhong.DANG_O) return KetQuaDieuKienHoaDon.DatPhongKhongDangO;

        var trangThais = dongs.Select(d => d.TrangThai).ToList();
        if (!trangThais.Any(t => t is TrangThaiDongPhong.DA_TRA or TrangThaiDongPhong.DANG_O))
            return KetQuaDieuKienHoaDon.ChuaCoDongCheckin;
        if (trangThais.Any(t => t is TrangThaiDongPhong.DANG_O or TrangThaiDongPhong.CHUA_NHAN))
            return KetQuaDieuKienHoaDon.ConDongChuaKetThuc;

        return KetQuaDieuKienHoaDon.DuDieuKien;
    }

    // Tính các giá trị ghi vào HOA_DON . Chỉ gọi khi KiemTraDieuKien trả DuDieuKien
    // tienCoc là DAT_PHONG.tien_coc 
    public static KetQuaHoaDon Tinh(long tienCoc, IEnumerable<DongPhongHoaDon> cacDong)
    {
        ArgumentNullException.ThrowIfNull(cacDong);
        if (tienCoc < 0)
            throw new ArgumentOutOfRangeException(nameof(tienCoc), "Tiền cọc không được âm");

        long tongPhong = 0, tongDichVu = 0, cocGiuLai = 0;
        foreach (var dong in cacDong)
        {
            switch (dong.TrangThai)
            {
                case TrangThaiDongPhong.DA_TRA:
                    if (dong.TienPhong is not { } tienPhong || dong.TienDichVu is not { } tienDichVu)
                        throw new InvalidOperationException(
                            "Dữ liệu không nhất quán: dòng phòng đã trả nhưng chưa chốt tien_phong/tien_dich_vu");
                    if (tienPhong < 0 || tienDichVu < 0)
                        throw new InvalidOperationException("Dữ liệu không nhất quán: tiền phòng/dịch vụ âm");
                    tongPhong = checked(tongPhong + tienPhong);
                    tongDichVu = checked(tongDichVu + tienDichVu);
                    break;

                case TrangThaiDongPhong.NO_SHOW:
                    if (dong.CocGiuLai < 0)
                        throw new InvalidOperationException("Dữ liệu không nhất quán: coc_giu_lai âm");
                    cocGiuLai = checked(cocGiuLai + dong.CocGiuLai);
                    break;

                default:
                    throw new InvalidOperationException(
                        "Còn dòng phòng chưa kết thúc hợp lệ (DANG_O hoặc CHUA_NHAN); chưa đủ điều kiện lập hóa đơn");
            }
        }

        // cọc giữ vượt cọc ban đầu thì dừng lập hóa đơn
        long tienCocConLai = tienCoc - cocGiuLai;
        if (tienCocConLai < 0)
            throw new InvalidOperationException("Dữ liệu không nhất quán: cọc giữ lại vượt tiền cọc ban đầu");

        long tongCong = checked(tongPhong + tongDichVu);
        long soTienConLai = tongCong - tienCocConLai;

        // chỉ dương mới CHO_THANH_TOAN; 0 hoặc âm là DA_THANH_TOAN ngay khi lập
        var trangThai = soTienConLai > 0 ? TrangThaiHoaDon.CHO_THANH_TOAN : TrangThaiHoaDon.DA_THANH_TOAN;

        return new KetQuaHoaDon(tongPhong, tongDichVu, tienCoc, cocGiuLai, tienCocConLai, soTienConLai, trangThai);
    }

    // chỉ thanh toán khi CHO_THANH_TOAN và số còn lại dương; chỉ thanh toán một lần
    public static bool CoTheThanhToan(TrangThaiHoaDon trangThai, long soTienConLai)
        => trangThai == TrangThaiHoaDon.CHO_THANH_TOAN && soTienConLai > 0;

    // Mỗi đặt phòng chỉ có  1 hoá đơn
    public static string TaoMaHoaDon(string maDatPhong) => "HD-" + KiemTraMa(maDatPhong);

    public static string TaoMaHoanCoc(string maDatPhong) => "HC-" + KiemTraMa(maDatPhong);

    // phần hoàn trả : HOAN TRA COC CHO QUY KHACH <TEN> <MA DAT PHONG>
    public static string TaoNoiDungHoanCoc(string tenKhach, string maDatPhong)
    {
        string ten = BoDauVaVietHoa(tenKhach);
        string ma = BoDauVaVietHoa(KiemTraMa(maDatPhong));
        if (ten.Length == 0)
            throw new ArgumentException("Tên khách không được rỗng", nameof(tenKhach));
        return $"HOAN TRA COC CHO QUY KHACH {ten} {ma}";
    }

    private static string KiemTraMa(string maDatPhong)
    {
        if (string.IsNullOrWhiteSpace(maDatPhong))
            throw new ArgumentException("Mã đặt phòng không được rỗng", nameof(maDatPhong));
        return maDatPhong.Trim();
    }

    private static string BoDauVaVietHoa(string chuoi)
    {
        var sb = new StringBuilder();
        foreach (char c in chuoi.Normalize(NormalizationForm.FormD))
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.NonSpacingMark) continue;
            sb.Append(c switch { 'đ' => 'd', 'Đ' => 'D', _ => c });
        }
        
        var cacTu = sb.ToString().ToUpperInvariant().Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
        return string.Join(' ', cacTu);
    }
}