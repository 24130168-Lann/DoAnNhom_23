using System.ComponentModel.DataAnnotations;

namespace khachSan;

public class TinhCongSuat
{
    public static KetQuaCongSuat? Tinh(ThangNam thang, int soPhong, IEnumerable<DongPhong> dongPhongs, DateTime hienTai)
    {
        if (soPhong <= 0)
            throw new ArgumentException("so phong phai lon hon 0, khong the tinh cong suat", nameof(soPhong));
        ArgumentNullException.ThrowIfNull(dongPhongs);

        DateTime dau = thang.DauThang;
        if (hienTai <= dau) return null;

        DateTime cuoi = Min(thang.DauThangSau, hienTai);

        decimal soPhut = 0;
        foreach (var d in dongPhongs)
        {
            if(d.CheckinThucTe is not { } vao) continue;
            DateTime ra = d.CheckoutThucTe ?? hienTai;

            DateTime batDau = Max(vao, dau);
            DateTime ketThuc = Min(ra, cuoi);
            if(ketThuc > batDau)
                soPhut += (decimal)(ketThuc - batDau).TotalMinutes;
        }

        decimal tuSo = soPhut / 60m;

        decimal mauSo = soPhong * ((decimal)(cuoi - dau).TotalMinutes / 60m);

        decimal phanTram = Math.Round(tuSo * 100m / mauSo, 1, MidpointRounding.AwayFromZero);
        return new KetQuaCongSuat(tuSo, mauSo, phanTram);
    }

    public static decimal MucThamChieu(TimeOnly gioNhanChuan, TimeOnly gioTraChuan)
    {
        decimal chenh = (decimal)(gioNhanChuan.ToTimeSpan() - gioTraChuan.ToTimeSpan()).TotalHours;
        return Math.Round((24m - chenh) / 24m * 100m, 1, MidpointRounding.AwayFromZero);
    }
    private static DateTime Min(DateTime a, DateTime b) => a < b ? a : b;
    private static DateTime Max(DateTime a, DateTime b) => a > b ? a : b;
}