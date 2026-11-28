namespace DoAnNhom23.Data;

public class DongHo
{
    public static DateTime Bay()
        {
            var t = DateTime.Now;
            return new DateTime(t.Year, t.Month, t.Day, t.Hour, t.Minute, t.Second);
        }
}