namespace DoAnNhom23.Module2.Models;

// Bảng DICH_VU (schema.sql). Danh mục do Chủ quản lý; DangBan=false chỉ ẩn khỏi lần gọi mới.
public class DichVu
{
    public long Id { get; set; }
    public string Ten { get; set; } = string.Empty;
    public long DonGia { get; set; }
    public string? DonVi { get; set; }
    public bool DangBan { get; set; }
}