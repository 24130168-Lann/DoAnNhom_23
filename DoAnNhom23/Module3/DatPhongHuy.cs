namespace khachSan;

public class DatPhongHuy
{
    public const string LyDOHetHanCoc = "HET_HAN_COC";
    
    public DateTime? ThoiDiemCoc{ get; set; }
    public decimal TienCoc { get; set; }
    public decimal TienCocHoan{get; set;}
    public string? LyDoHuy { get; set; }
    public DateTime ThoiDiemHuy { get; set; }
}