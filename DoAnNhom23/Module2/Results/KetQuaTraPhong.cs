namespace DoAnNhom23.Module2.Results;

// Kết quả tính khi trả phòng (CO-03)
public record KetQuaTraPhong(
    DateTime CheckoutThucTe,
    KetQuaTienPhong TienPhong,
    long TienDichVu,
    DateTime DonXongLuc)   // DN-01: checkout_thuc_te + thoi_gian_don_phut; phòng chuyển vàng (can_don_dep = true)
{
    public long TongTien => checked(TienPhong.TienPhong + TienDichVu);
}