namespace DoAnNhom23.Module2;

// Kết quả tính tiền phòng (TP-01 đến TP-07)
public enum HinhThucThue
{
    TheoGio, // check-in va check-out cung ngay lich
    TheoDem  // check-out khac ngay lich
}
public record KetQuaTienPhong(
    long TienPhong,
    HinhThucThue Hinhthuc,
    int SoDem,      // số đêm tính tiền (0 nếu cùng ngày)
    int SoGio,      // số giờ tính tiền (chỉ dùng cho nhánh cùng ngày)
    int SoGioTre,   // số giờ trễ sau giờ trả chuẩn (chỉ dùng cho nhánh qua đêm)
    long TienDem,   // tiền đêm (nhánh qua đêm)
    long TienGio,   // tiền theo giờ (nhánh cùng ngày) hoặc phụ thu trễ (nhánh qua đêm)
    string CachTinh
    );

public class TinhTienPhong
{
    // Giờ trả chuẩn mặc định 12:00 (CAU_HINH.gio_tra_chuan)
    public static readonly TimeSpan GioTraChuanMacDinh = new TimeSpan(12, 0, 0);

    public static KetQuaTienPhong Tinh(
        DateTime gioNhanThucTe,
        DateTime gioTraThucTe,
        long giaMoiDem,
        long giaMoiGio,
        TimeSpan? gioTraChuan = null)
    {
        // TP-01: checkout phải lớn hơn checkin
        if (gioTraThucTe <= gioNhanThucTe)
            throw new ArgumentException("Giờ trả thực tế phải sau giờ nhận thực tế", nameof(gioTraThucTe));
        if (giaMoiDem < 0)
            throw new ArgumentOutOfRangeException(nameof(giaMoiDem), "Giá mỗi đêm không được âm");
        if (giaMoiGio < 0)
            throw new ArgumentOutOfRangeException(nameof(giaMoiGio), "Giá mỗi giờ không được âm");
        var chuan = gioTraChuan ?? GioTraChuanMacDinh;
        if (chuan < TimeSpan.Zero || chuan >= TimeSpan.FromDays(1))
            throw new ArgumentOutOfRangeException(nameof(gioTraChuan),
                "Giờ trả chuẩn phải nằm trong một ngày (00:00 đến 23:59)");

        int soDem = (gioTraThucTe.Date - gioNhanThucTe.Date).Days;

        // TP-02: cùng ngày lịch -> tính theo giờ, trần một đêm
        if (soDem == 0)
        {
            int soGio = (int)Math.Max(1, LamTronLenGio(gioTraThucTe - gioNhanThucTe));
            long tienGio = Math.Min(Nhan(soGio, giaMoiGio), giaMoiDem);
            return new KetQuaTienPhong(TienPhong: tienGio, Hinhthuc: HinhThucThue.TheoGio, SoDem: 0, SoGio: soGio,
                SoGioTre: 0, TienDem: 0, TienGio: tienGio,
                CachTinh: $"{soGio} giờ x {DinhDang.Tien(giaMoiGio)}, trần một đêm {DinhDang.Tien(giaMoiDem)}");
        }

        // TP-03: qua ngày -> số đêm = chênh lệch ngày lịch (qua nửa đêm tính ít nhất một đêm)
        long tienDem = Nhan(soDem, giaMoiDem);

        // TP-04: phụ thu trễ giờ trả chuẩn (chỉ nhánh qua đêm), trần một đêm
        DateTime moc = gioTraThucTe.Date + chuan;
        int soGioTre = 0;
        long phuThu = 0;
        if (gioTraThucTe > moc)
        {
            soGioTre = (int)LamTronLenGio(gioTraThucTe - moc);
            phuThu = Math.Min(Nhan(soGioTre, giaMoiGio), giaMoiDem);
        }

        string cach = soGioTre > 0
            ? $"{soDem} đêm x {DinhDang.Tien(giaMoiDem)} + {soGioTre} giờ trễ x {DinhDang.Tien(giaMoiGio)} (trần một đêm {DinhDang.Tien(giaMoiDem)})"
            : $"{soDem} đêm x {DinhDang.Tien(giaMoiDem)}";
        // TP-07: tiền phòng = tiền cơ bản + phụ thu
        return new KetQuaTienPhong(checked(tienDem + phuThu),Hinhthuc: HinhThucThue.TheoDem,
        soDem, 0, soGioTre, tienDem, phuThu, cach);
    }

    // làm tròn lên theo giờ (không có thời gian ân hạn - GC-3)
    private static long LamTronLenGio(TimeSpan khoang)
        => (khoang.Ticks + TimeSpan.TicksPerHour - 1) / TimeSpan.TicksPerHour;

    private static long Nhan(long a, long b) => checked(a * b);
}
