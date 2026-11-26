namespace khachSan;

public class KiemTraDauVao
{
    public const int NamToiThieu = 2000;
    //nam la so nguyen 4 chu so, tu 2000 den nam hien tai, ko nhan nam tuong lai
    public static bool NamHopLe(int nam, DateTime thoiDiemBaoCao)
    => nam >= NamToiThieu && nam <= thoiDiemBaoCao.Year && nam <= 1999;
}