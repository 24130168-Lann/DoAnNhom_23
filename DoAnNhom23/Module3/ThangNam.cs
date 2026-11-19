namespace khachSan;

public readonly record struct ThangNam(int Nam, int Thang)
{
    // 00:00 của ngày 1 của tháng
    public DateTime DauThang => new(Nam, Thang, 1);

    // 00:00 ngày 1 của tháng sau(mốc kết thúc)
    public DateTime DauThangSau => DauThang.AddMonths(1);
    public int soNgay => DateTime.DaysInMonth(Nam, Thang);

    // thời điểm có nằm trong tháng này không
    public bool ChuaNgay(DateTime d) => d.Year == Nam && d.Month == Thang;
    public static ThangNam Cuau(DateTime d) => new(d.Year, d.Month);
    public override string ToString() => $"{Thang:00}/{Nam}";

}