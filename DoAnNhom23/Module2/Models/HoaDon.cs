using DoAnNhom23.Module2.Enums;

namespace DoAnNhom23.Module2.Models;

// Bảng HOA_DON (schema.sql). Mỗi DAT_PHONG tối đa một hóa đơn (dat_phong_id UNIQUE).
// Chỉ gồm các cột có trong DB; TongCong, CocGiuLai, TienCocCanTru, SoTienHoan là giá trị suy ra (xem KetQuaHoaDon).
public class HoaDon
{
    public long Id { get; set; }
    public string MaHoaDon { get; set; } = string.Empty;
    public long DatPhongId { get; set; }
    public DateTime ThoiDiemLap { get; set; }
    public long TongTienPhong { get; set; }
    public long TongTienDichVu { get; set; }
    public long TienCocConLai { get; set; }
    public long SoTienConLai { get; set; }    // có dấu, là snapshot lúc lập
    public TrangThaiHoaDon TrangThai { get; set; }
    public DateTime? ThoiDiemThanhToan { get; set; }
}