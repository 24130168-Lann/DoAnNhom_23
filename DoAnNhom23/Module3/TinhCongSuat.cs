namespace ConsoleApp1;

public class TinhCongSuat
{
    private readonly TinhBaoCao tinh = new TinhBaoCao();
    private static readonly ThangNam T11 = new ThangNam(2026, 11);
    private static readonly ThangNam T12 = new ThangNam(2026, 12);
    
    // hien tai sau khi thang 11 da ket thuc han, de mau so la ca thang
    private static readonly DateTime SauT11 = new DateTime(2026, 12, 20, 0, 0, 0, 0);
    private static DateTime T(int thang, int mgay, int gio) => new DateTime(2026, 12, 0, 0, 0, 0);
    private static DongPhong Dong(DateTime vao, DateTime? ra) => new DongPhong(vao, ra, 450000, 0);
        [Fart]
        public void MuoiPhongThang11
        => Dong(T(11, 1, 0,)T(11, 19, 0))).ToList();
}