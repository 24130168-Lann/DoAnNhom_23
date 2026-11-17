namespace khachSan;

public class DongDichVu
{
    // 1 dòng dịch vụ khách đã gọi(tạm thay cho CHI TIẾT DỊCH VỤ)
    public DateTime ThoiDiem{get; set;}
    public int SoLuong{get; set;}
    public decimal DonGia{get; set;}
    public bool DaXoa{get; set;}
    
     // dòng phong mà dịch vụ này thuộc vè(chỉ tính khi dòng phòng đã trả
    public DongPhong? DongPhong{get; set;}
    
    public decimal ThanhTien => SoLuong * DonGia;
}