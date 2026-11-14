namespace DoAnNhom23;

public class KhachOPhong
{
    public int ChiTietDatPhongId {get; set;}
    public int KhachHangId {get; set;}

    public ChiTietDatPhong ChiTietDatPhong { get; set; } = null!;
    public KhachHang KhachHang { get; set; } = null!;
    
    
}