namespace DoAnNhom23.Module2;

public record CauHinhGioTra(TimeSpan GioTraChuan, int? SoGioTreToiDa = null, int? SoGioThueTheoGioToiDa = null)
{
    public static CauHinhGioTra MacDinh { get; } = new(new TimeSpan(12,0,0));

    public void KiemTra()
    {
        
    }
}
