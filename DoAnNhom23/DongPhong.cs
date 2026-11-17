namespace khachSan;

public class DongPhong
{
    public DateTime CheckinThucTe { get; set; }
    // rỗng nếu khách đang ở(chưa trả phòng)
    public DateTime? CheckoutThucTe { get; set; }
    public decimal GiaMoiDem{get; set;}
    public decimal TienPhong{get; set;}
    public bool DaTra => CheckoutThucTe.HasValue;
}