namespace ConsoleApp1;

public record DatPhongHuy(
    DateTime? ThoiDiemCoc,
    long TienCoc,
    long TienCocHoan,
    string LyDoHuy,
    string ThoiDiemHuy)
{
    public bool DaThuCoc => ThoiDiemCoc.HasValue;
}