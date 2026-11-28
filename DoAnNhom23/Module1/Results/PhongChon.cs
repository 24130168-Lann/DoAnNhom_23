


namespace DoAnNhom23.Module1.Results;

public class PhongChon
{
    public long PhongId { get; set; }
    public List<KhachNhap> KhachO { get; set; } = new();   // để trống thì lấy người đặt
}