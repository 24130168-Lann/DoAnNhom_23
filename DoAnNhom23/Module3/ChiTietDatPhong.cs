namespace khachSan;
/// <summary>
/// Bảng CHI_TIET_DAT_PHONG (một dòng phòng của một đặt phòng)
/// </summary>
public sealed class ChiTietDatPhong
{
    public int Id { get; init; }
    public int DatPhongId { get; init; }
    public int PhongId { get; init; }
    public DateTime? CheckinThucTe { get; init; }
    public DateTime? CheckoutThucTe { get; init; }
    public long GiaMoiDem{get; init;}
    public long TienPhong{get; init;}
    public long TienDichVu{get; init;}
    
    // Thoi diem xac nhan no-show cua rieng dong nay
    public DateTime? HuyLuc{get; init;}
    
    // Coc giu lai cua dong no-show doModele1 da tinh va luu(ko tinh lai)
    public long CocGiuLai{get; init;}

    public List<ChiTietDichVu> DichVus { get; init; } = new();
    
    public bool DaCheckin => CheckinThucTe.HasValue;
    
    // dong phong da tra: co checkinThucTe vaf CheckoutThucTe
    public bool DaCheckout => CheckinThucTe.HasValue && CheckoutThucTe.HasValue;
}