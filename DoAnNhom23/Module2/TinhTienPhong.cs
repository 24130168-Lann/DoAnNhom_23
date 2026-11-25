namespace DoAnNhom23.Module2;

// Tính tiền một dòng phòng khi check-out (CO-03). Kết quả là số nguyên đồng (GC-9), không dùng số thực.
public class TinhTienPhong
{
    // giờ trả chuẩn mặc định 12:00 (CAU_HINH.gio_tra_chuan)
    public static readonly TimeSpan GioTraChuanMacDinh = CauHinhGioTra.MacDinh.GioTraChuan;

    public static KetQuaTienPhong Tinh(
        DateTime gioNhanThucTe,
        DateTime gioTraThucTe,
        long giaMoiDem,
        long giaMoiGio,
        CauHinhGioTra cauHinh)
    {
        ArgumentNullException.ThrowIfNull(cauHinh);
        return Tinh(gioNhanThucTe, gioTraThucTe, giaMoiDem, giaMoiGio, cauHinh.GioTraChuan);
    }

    public static KetQuaTienPhong Tinh(
        DateTime gioNhanThucTe,
        DateTime gioTraThucTe,
        long giaMoiDem,
        long giaMoiGio,
        TimeSpan? gioTraChuan = null)
    {
        //TP-01: check-out phải lớn hơn check-in
        if (gioTraThucTe <= gioNhanThucTe)
            throw new ArgumentException("Giờ trả thực tế phải lớn hơn giờ nhận thực tế", nameof(gioTraThucTe));
        if (giaMoiDem < 0)
            throw new ArgumentOutOfRangeException(nameof(giaMoiDem), "Giá mỗi đêm không được âm");
        if (giaMoiGio < 0)
            throw new ArgumentOutOfRangeException(nameof(giaMoiGio), "Giá mỗi giờ không được âm");

        var chuan = gioTraChuan ?? GioTraChuanMacDinh;
        new CauHinhGioTra(chuan).KiemTra();

        int soDem = (gioTraThucTe.Date - gioNhanThucTe.Date).Days;

        //TP-02: cùng ngày lịch, tính theo giờ, trần một đêm, không cộng TP-04
        if (soDem == 0)
        {
            int soGio = (int)Math.Max(1, LamTronLenGio(gioTraThucTe - gioNhanThucTe));
            long tienGio = Math.Min(Nhan(soGio, giaMoiGio), giaMoiDem);
            return new KetQuaTienPhong(tienGio, 0, soGio, 0, 0, tienGio,
                $"{soGio} giờ x {giaMoiGio:N0}, trần một đêm {giaMoiDem:N0}");
        }

        //TP-03: qua ngày, tính số đêm theo ngày lịch (qua nửa đêm tính ít nhất một đêm)
        long tienDem = Nhan(soDem, giaMoiDem);

        //TP-04: chỉ nhánh qua đêm, trễ sau giờ trả chuẩn của ngày check-out, trần một đêm
        DateTime moc = gioTraThucTe.Date + chuan;
        int soGioTre = 0;
        long phuThu = 0;
        if (gioTraThucTe > moc)
        {
            soGioTre = (int)LamTronLenGio(gioTraThucTe - moc);
            phuThu = Math.Min(Nhan(soGioTre, giaMoiGio), giaMoiDem);
        }

        string cach = soGioTre > 0
            ? $"{soDem} đêm x {giaMoiDem:N0} + {soGioTre} giờ trễ x {giaMoiGio:N0} (trần một đêm {giaMoiDem:N0})"
            : $"{soDem} đêm x {giaMoiDem:N0}";

        //TP-07: tien_phong = tiền cơ bản + phụ thu
        return new KetQuaTienPhong(Cong(tienDem, phuThu), soDem, 0, soGioTre, tienDem, phuThu, cach);
    }

    // làm tròn lên theo giờ
    private static long LamTronLenGio(TimeSpan khoang)
        => (khoang.Ticks + TimeSpan.TicksPerHour - 1) / TimeSpan.TicksPerHour;

    private static long Nhan(long a, long b) => checked(a * b);

    private static long Cong(long a, long b) => checked(a + b);
}