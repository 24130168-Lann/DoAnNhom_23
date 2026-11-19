namespace khachSan;

public class TinhDoanhThu
{
    public static KetQuaDoanhThu Tinh(ThangNam thang, IEnumerable<DongPhong> dongPhongs,
        IEnumerable<DongDichVu> dongDichVus)
    {
        ArgumentNullException.ThrowIfNull(dongPhongs);
        ArgumentNullException.ThrowIfNull(dongDichVus);

        return new KetQuaDoanhThu(TinhPhong(thang, dongPhongs), TinhDichVu(thang, dongDichVus));
    } 
    
    public static decimal TinhPhong(ThangNam thang, IEnumerable<DongPhong> dongPhongs)

    {
        decimal tong = 0;
        foreach (var d in dongPhongs)
        {
            
            // chi tinh dong da tra phong
            if(d.CheckinThucTe is not { } vao || d.CheckoutThucTe is not { } ra) continue; 
        
            // so dem = chenh lech ngay lich; moi dem thuoc thang cuar ngay dem do
            int soDem = (ra.Date - vao.Date).Days;
            for (int i = 0; i < soDem; i++)
            {
                if (thang.ChuaNgay(vao.Date.AddDays(i)))
                    tong += d.GiaMoiDem;
            }

            // tien gio le thuoc thang cua ngay ra
            if (thang.ChuaNgay(ra))
                tong += d.TienPhong - soDem * d.GiaMoiDem;
        }
        return tong;
    }

    public static decimal TinhDichVu(ThangNam thang, IEnumerable<DongDichVu> dongDichVus)
    {
        decimal tong = 0;
        foreach (var dv in dongDichVus)
        {
            if(dv.DaXoa) continue;
            if(dv.DongPhong is {DaTra : false}) continue;
            if (thang.ChuaNgay(dv.ThoiDiem))
                tong += dv.ThanhTien;
        }
        return tong;
    }
}