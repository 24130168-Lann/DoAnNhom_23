namespace DoAnNhom23.Module2.Models;

// Bảng CHI_TIET_DICH_VU (schema.sql). Tên/đơn vị/đơn giá là bản chụp lúc gọi (DV-02).
public class ChiTietDichVu
{
    public long Id { get; set; }
    public long ChiTietDatPhongId { get; set; }
    public long DichVuId { get; set; }
    public string TenDichVuLucGoi { get; set; } = string.Empty;
    public string? DonViLucGoi { get; set; }
    public int SoLuong { get; set; }          // 1..99
    public long DonGia { get; set; }
    public DateTime ThoiDiem { get; set; }
    public long? TaiKhoanId { get; set; }     // dòng mới bắt buộc có (Nhân viên/Chủ)

    // xóa mềm (DV-05)
    public bool DaXoa { get; set; }
    public string? GhiChuXoa { get; set; }    // 1..200 ký tự khi DaXoa
    public long? TaiKhoanXoaId { get; set; }
    public DateTime? ThoiDiemXoa { get; set; }
}