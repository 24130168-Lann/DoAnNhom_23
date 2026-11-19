namespace khachSan;

public class DongPhong
{
    public DateTime? CheckinThucTe { get; set; }
    public DateTime? CheckoutThucTe { get; set; } // rỗng nếu khách đang ở(chưa trả phòng)
    public decimal GiaMoiDem{get; set;}
    public decimal TienPhong{get; set;}
    public bool DaTra => CheckinThucTe.HasValue && CheckoutThucTe.HasValue;
}