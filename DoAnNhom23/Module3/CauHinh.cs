namespace khachSan;

// Bảng CAU_HINH: giờ nhận/trả chuẩn, ch dùng để tính mức tham chiếu
public sealed class CauHinh
{
    public TimeSpan? GioNhanChuan{get; init;}
    public TimeSpan? GioTraChuan{get; init;}
}