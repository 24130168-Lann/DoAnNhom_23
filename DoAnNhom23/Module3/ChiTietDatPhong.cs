namespace khachSan;

public sealed class ChiTietDatPhong
{
    public int Id { get; set; }
    public int DatPhongId { get; set; }
    public int PhongId { get; set; }
    public DateTime? CheckinThucTe { get; set; }
    public DateTime? CheckoutThucTe { get; set; }
    public decimal GiaMoiDem{get; set;}
    public decimal TienPhong{get; set;}
    public decimal TienDichVu{get; set;}
    
    // Thoi diem xac nhan no-show cua rieng dong nay
    public DateTime? HuyLuc{get; set;}
    
    // Coc giu lai cua dong no-show doModele1 da tinh va luu(ko tinh lai)
    public decimal CocGiuLai{get; set;}

    public List<ChiTietDichVu> DichVus { get; set; } = new();
    
    public bool DaCheckin => CheckinThucTe.HasValue;
    
    // dong phong da tra: co checkinThucTe vaf CheckoutThucTe
    public bool DaCheckout => CheckinThucTe.HasValue && CheckoutThucTe.HasValue;
}