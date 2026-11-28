namespace DoAnNhom23.Module1.Results;

public class KetQuaThaoTac
{
    public bool ThanhCong { get; set; }
    public string ThongBao { get; set; } = "";
    public long? DatPhongId { get; set; }
    public string? MaDatPhong { get; set; }
    public static KetQuaThaoTac Loi(string msg) => new() { ThanhCong = false, ThongBao = msg };
    public long? TienCoc { get; set; }
    public DateTime? HanThanhToan { get; set; }
}