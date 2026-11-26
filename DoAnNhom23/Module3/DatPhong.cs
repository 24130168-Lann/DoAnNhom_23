namespace khachSan;

public sealed class DatPhong
{
    public int Id { get; set; }
    private TrangThaiDatPhong TrangThai { get; set; }
    public decimal TienCoc { get; set; }
    public DateTime? ThoiDiemCoc{ get; set; }
    public decimal TienCocHoan { get; set; }
    public DateTime? ThoiDiemHuy{ get; set; }
    public string? LyDoHuy { get; set; }
    public List<ChiTietDatPhong> CacDong { get; init; } = new();

}