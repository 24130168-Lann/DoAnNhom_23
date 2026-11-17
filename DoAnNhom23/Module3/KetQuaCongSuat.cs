namespace khachSan;

public sealed record KetQuaCongSuat(double GioCoKhach, double GioCoTheCoKhach, double CongSuatPhanTram)
{
    public static KetQuaCongSuat? Tinh(ThangNam thang, int soPhong, IEnumerable<DongPhong> dongPhongs, DateTime hienTai)
    {
        if (soPhong <= 0) throw new ArgumentException("Chua co phong nao de tinh cong suat", nameof(soPhong));
        ArgumentNullException.ThrowIfNull(dongPhongs);
        DateTime dau = thang.NgayDau;
        if (hienTai <= dau) return null;
        
        //
        DateTime cuoi = hienTai < thang.ThangSau ? hienTai : thang.ThangSau;
        
        //
        double mauSo = soPhong * (cuoi - dau).TotalHours;
        
        //
        double tuSo = 0;
        foreach (DongPhong d in dongPhongs)
        {
            DateTime batDau = d.CheckinThucTe > dau ? d.CheckinThucTe : dau;
            DateTime ra = d.CheckoutThucTe ?? hienTai; // chua tra thi tinh den hien tai
            DateTime ketThuc = ra < cuoi ? ra : cuoi;
            if (ketThuc > dau) tuSo += (ketThuc - batDau).TotalHours;
        }
        
        //
        double phanTram = Math.Round(tuSo / mauSo * 100, 1, MidpointRounding.AwayFromZero);
        return new KetQuaCongSuat(tuSo, mauSo, phanTram);
    }
    
    //
    public static double MucThamChieu(TimeSpan gioNhanChuan, TimeSpan gioTraChuan)
    {
        double ty = (24 - (gioNhanChuan - gioTraChuan).TotalHours) / 24;
        return Math.Round(ty*100, 1, MidpointRounding.AwayFromZero);
    }
}