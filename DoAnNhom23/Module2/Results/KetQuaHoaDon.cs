using DoAnNhom23.Module2.Enums;

namespace DoAnNhom23.Module2.Results;

// HD-08: lập xong không sửa, không phụ thuộc giá hiện hành
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
    // Khách còn phải trả bao nhiêu (0 nếu không còn nghĩa vụ)
    public long SoTienPhaiTra => Math.Max(0, SoTienConLai);

    // Chỉ có nút thanh toán/QR khi CHO_THANH_TOAN và còn dương (CO-05)
    public bool CanThanhToan => TrangThai == TrangThaiHoaDon.CHO_THANH_TOAN && SoTienConLai > 0;
}