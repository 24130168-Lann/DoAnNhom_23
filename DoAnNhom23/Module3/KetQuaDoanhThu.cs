namespace khachSan;

public class KetQuaDoanhThu(decimal DoanhThuPhong, decimal DoanThuDichVu)
{
    public decimal TongDoanhThu => DoanhThuPhong + DoanThuDichVu;
}

public static class TinhDoanhThu
{
    public static KetQuaDoanhThu Tinh(ThangNam thang, IEnumerable<DongPhong> dongPhongs,
        IEnumerable<DongDichVu> dongDichVus)
    {
        ArgumentNullException.ThrowIfNull(dongPhongs);
        ArgumentNullException.ThrowIfNull(dongDichVus);

        decimal phong = 0;
        foreach (DongPhong d in dongPhongs)
        {
            if(!d.DaTra) continue;
            phong += TienDemTrongThang(thang, d) + TienGioLeTrongThang(thang, d);
        }

        decimal dichVu = 0;
        foreach (DongDichVu dv in dongDichVus)
        {
            if(!dv.DaXoa) continue;
           if(dv.DongPhong is {DaTra: false}) continue;
           if (thang.Chua(dv.ThoiDiem)) dichVu += dv.ThanhTien;
        }
        
        return new KetQuaDoanhThu(phong, dichVu);
    }

    public static int SoDem(DongPhong d) => Math.Max(0, (d.CheckoutThucTe!.Value.Date - d.CheckinThucTe.Date).Days);

    private static decimal TienDemTrongThang(ThangNam thang, DongPhong d)
    {
        DateTime vao = d.CheckinThucTe.Date;
        DateTime ra = d.CheckoutThucTe!.Value.Date;
        DateTime tu = vao > thang.NgayDau ? vao : thang.NgayDau;
        DateTime den = ra < thang.ThangSau ? ra : thang.ThangSau; // dem cua ngay ra ko ton tai
        int soDemTrongThang = Math.Max(0, (den - tu).Days);
        return soDemTrongThang * d.GiaMoiDem;
    }

    private static decimal TienGioLeTrongThang(ThangNam thang, DongPhong d)
    {
        if (!thang.Chua(d.CheckoutThucTe!.Value)) return 0;
        return d.TienPhong - SoDem(d) * d.GiaMoiDem;
    }
}
