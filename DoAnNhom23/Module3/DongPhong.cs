namespace ConsoleApp1;

public record DongPhong(DateTime? CheckinThucTe,
    DateTime? CheckoutThucTe,
    long GiaMoiDem,
    long TienPhong) {
    public bool DaTra => CheckinThucTe.HasValue && CheckoutThucTe.HasValue;
    }