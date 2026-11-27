namespace khachSan;

// Kỳ tháng theo khoảng nửa [BatDau, KetThuc] .Tháng đang diễn ra bị cắt tại ThoiDiemBaoCao
public readonly record struct KyThang(int Nam, int Thang, DateTime BatDau, DateTime KetThuc, DateTime ThoiDiemBaoCao)
{
    //Tháng đã bắt đầu 00:00 ngày bắt đầu tháng không sau thời điểm chốt
    public bool DaBatDau => BatDau <= ThoiDiemBaoCao;
    
    // Điểm kết thúc hệu lực 00:00 tháng kế tiếp, hoặc thời điểm chốt nếu tháng đang diễ ra
    public DateTime CuoiKy => ThoiDiemBaoCao < KetThuc ? ThoiDiemBaoCao : KetThuc;
    
    // số giờ thực tế của k (không làm trong theo ngày). Tháng chưa bắt đàu = 0
    public double SoGio => DaBatDau ? (CuoiKy - BatDau).TotalHours : 0d;
    
    // Thời điểm t thuộc tháng này(theo lịch, [BatDau, KetThuc]
    public bool Chua(DateTime t) => t >= BatDau && t < KetThuc;
    
    // Số giowd giao giữ khoảng [tu, den] và kỳ[BatDau, CuoiKy] ko âm
    public double GioGiao(DateTime tu, DateTime den)
    {
        var batDau = tu > BatDau ? tu : BatDau;
        var ketThuc = den < CuoiKy ? den : CuoiKy;
        return ketThuc > batDau ? (ketThuc - batDau).TotalHours : 0d;
    }
    
    // Tạo đủ 12 kỳ tháng của năm; tháng tương lai co DaBatDau = false
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