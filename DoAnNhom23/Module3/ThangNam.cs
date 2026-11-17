namespace khachSan;

public readonly record struct ThangNam(int Nam, int Thang)
{
    // 00:00 của ngày 1 của tháng
    public DateTime NgayDau => new(Nam, Thang, 1, 0, 0, 0);

    // 00:00 ngày 1 của tháng sau(mốc kết thúc)
    public DateTime ThangSau => NgayDau.AddMonths(1);
    public int soNgay => DateTime.DaysInMonth(Nam, Thang);

    // thời điểm có nằm trong tháng này không
    public bool Chua(DateTime thoiDiem) =>thoiDiem >= NgayDau && thoiDiem <= ThangSau;
    public static ThangNam Tu(DateTime thoiDiem) => new(thoiDiem.Year, thoiDiem.Month);
    public override string ToString() => $"{Thang:00}/{Nam}";

}