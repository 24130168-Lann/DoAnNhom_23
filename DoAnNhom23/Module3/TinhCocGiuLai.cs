namespace khachSan;

public class TinhCocGiuLai
{
    
    // coc giu lai cua 1 phong da huy
    public static decimal CuaMotDatPhong(DatPhongHuy dat)
    {
        if (dat.ThoiDiemCoc is null) return 0; // chua thu coc
        return Math.Max(0, dat.TienCoc - dat.TienCocHoan);
        
    }

    // tong coc giu lai cua cac phong huy trong thang(theo thoiDiemHuy)
    public static decimal Tinh(ThangNam thang, IEnumerable<DatPhongHuy> datPhongs)
    {
        ArgumentNullException.ThrowIfNull(datPhongs);
        return datPhongs.Where(d => thang.ChuaNgay(d.ThoiDiemHuy)).Sum(CuaMotDatPhong);
        
    }
    
    // tong thu = tong doanh thu + coc giu lai
    public decimal TongThu(KetQuaDoanhThu doanhThu, decimal cocGiuLai)
        => KetQuaDoanhThu.TongDoanhThu + cocGiuLai;
}