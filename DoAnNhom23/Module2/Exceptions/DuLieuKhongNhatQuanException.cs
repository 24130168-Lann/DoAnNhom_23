namespace DoAnNhom23.Module2.Exceptions;

// Dữ liệu trong DB vi phạm bất biến phải dừng, không được tự sửa
public class DuLieuKhongNhatQuanException : Exception
{
    public DuLieuKhongNhatQuanException(string message) : base(message) { }
}