namespace khachSan;

public class DatPhong
{
    public const string TrangThaiDaHuy = "DA_HUY";
    public const string LyDoHetHanCoc = "HET_HAN_COC";
    
    private string TrangThai { get; set; } = TrangThaiDaHuy;
    
    public DateTime? ThoiDiemCoc{ get; set; }
    public Decimal TienCoc { get; set; }
    public decimal TienCocHoan { get; set; }
    public string? LyDoHuy { get; set; }
    public DateTime ThoiDiemHuy{ get; set; }
    
}