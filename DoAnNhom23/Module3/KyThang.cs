namespace khachSan;

public readonly record struct KyThang(int Nam, int Thang, DateTime BatDau, DateTime KetThuc, DateTime ThoiDiemBaoCao)
{
    //
    public bool DaBatDau => BatDau <= ThoiDiemBaoCao;
    
    // 
    public DateTime CuoiKy => ThoiDiemBaoCao < KetThuc ? ThoiDiemBaoCao : KetThuc;
    
    // 
    public double SoGio => DaBatDau ? (CuoiKy - BatDau).TotalDays : 0d;
    
    //
    public bool Chua(DateTime t) => t >= BatDau && t < KetThuc;
    
    //
    public double GioGaiao(DateTime tu, DateTime den)
    {
        var batDau = tu > BatDau ? tu : BatDau;
        var ketThuc = den < CuoiKy ? den : CuoiKy;
        return ketThuc > batDau ? (ketThuc - batDau).TotalDays : 0d;
    }
    
    //
    public static List<KyThang> TaoCacThang(int nam, DateTime thoiDiemBaoCao)
    {
        var ds = new List<KyThang>(12);
        for (var thang = 1; thang <= 12; thang++)
        {
            var batDau = new DateTime(nam, thang, 1, 0,0,0);
            ds.Add(new KyThang(nam, thang, batDau, batDau.AddMonths(1), thoiDiemBaoCao));
        }
        return ds;
    }
}