namespace DoAnNhom23.Module2;
// comment
public record KetQuaTienPhong(
    long TienPhong,
    int SoDem,  //comment
    int SoGio,  //comment
    int SoGioTre,   //comment
    long TienDem,   //comment
    long TienGio, //comment
    string CachTinh
    );

public class TinhTienPhong
{
    //cm
    public static readonly TimeSpan GioTraChuanMacDinh = new TimeSpan(12, 0, 0);
    //cm
    public static KetQuaTienPhong Tinh(
        DateTime gioNhanThucTe,
        DateTime gioTraThucTe,
        long giaMoiDem,
        long giaMoiGio,
        TimeSpan? gioTraChuan = null)
    {
        if (gioTraThucTe < gioNhanThucTe)
            throw new ArgumentException(("Giờ trả thực tế không được trước giờ nhận thực tế"), nameof(gioTraThucTe));
        if (giaMoiDem < 0)
            throw new ArgumentOutOfRangeException(nameof(gioTraThucTe), "Giá mỗi đêm không được âm");
        var chuan = gioTraChuan ?? GioTraChuanMacDinh;
        if (chuan < TimeSpan.Zero || chuan >= TimeSpan.FromDays(1))
            throw new ArgumentOutOfRangeException(nameof(gioTraChuan),
                "Giờ trả chuẩn phải nằm trong một ngày(00:00 đến 23:59)");
        int soDem = (gioTraThucTe.Date - gioNhanThucTe.Date).Days;
        //cm
        if (soDem == 0)
        {
            int soGio = (int)Math.Max(1, LamTronLenGio(gioTraThucTe - gioNhanThucTe));
            long tienGio = Math.Min(Nhan(soGio, giaMoiGio), giaMoiDem);
            return new KetQuaTienPhong(tienGio, 0, soGio, 0, 0, tienGio,
                $"{soGio}giờ x {giaMoiGio:N0}, trần một đêm{giaMoiDem:N0}");

        }

        //TP3
        long tienDem = Nhan(soDem, giaMoiDem);
        //TP04
        DateTime moc = gioTraThucTe.Date + chuan;
        int soGioTre = 0;
        long phuThu = 0;
        if (gioTraThucTe > moc)
        {
            soGioTre = (int)LamTronLenGio(gioTraThucTe - moc);
            phuThu = Math.Min(Nhan(soGioTre, giaMoiGio), giaMoiDem);
        }

        string cach = soGioTre > 0
            ? $"{soDem} đêm x {giaMoiDem:N0}+ {soGioTre}giờ trễ x {giaMoiGio:NO}(trần một đêm{giaMoiDem:N0})"
            : $"{soDem} đêm x{giaMoiDem:N0} ";
        return new KetQuaTienPhong(tienDem + phuThu, soDem, 0, soGioTre, tienDem, phuThu, cach);
    }
    //cm
    private static long LamTronLenGio(TimeSpan khoang) 
        => (khoang.Ticks + TimeSpan.TicksPerHour - 1) / TimeSpan.TicksPerHour;

    private static long Nhan(long a, long b) => checked(a * b);



}