namespace DoAnNhom23.Module2.Enums;

// Giá trị khớp TAI_KHOAN.vai_tro trong schema.sql ('CHU', 'NHAN_VIEN') để dùng Enum.Parse như Module 1.
// KHACH không có tài khoản trong DB (khách xác thực theo AC-01).
public enum VaiTro
{
    KHACH,
    NHAN_VIEN,
    CHU
}