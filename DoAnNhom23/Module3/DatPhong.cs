namespace khachSan;
/// <summary>
/// bảng DAT_PHONG module 1 là nguồn của các trường cọc/hủy
/// </summary>
public sealed class DatPhong
{
    public int Id { get; init; }
    public TrangThaiDatPhong TrangThai { get; init; }
    public long TienCoc { get; init; }
    public DateTime? ThoiDiemCoc{ get; init; }
    public long TienCocHoan { get; init; }
    public DateTime? ThoiDiemHuy{ get; init; }
    public string? LyDoHuy { get; init; }
    public List<ChiTietDatPhong> CacDong { get; init; } = new();

}