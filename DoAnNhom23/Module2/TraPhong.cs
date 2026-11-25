namespace DoAnNhom23.Module2;

// Kết quả tính khi trả phòng (CO-03)
public record KetQuaTraPhong(
    DateTime CheckoutThucTe,
    KetQuaTienPhong TienPhong,
    long TienDichVu,
    DateTime DonXongLuc)   // DN-01: checkout_thuc_te + thoi_gian_don_phut; phòng chuyển vàng (can_don_dep = true)
{
    public long TongTien => checked(TienPhong.TienPhong + TienDichVu);
}

public static class TraPhong
{
    public const int ThoiGianDonMacDinhPhut = 30;

    // Dòng phòng phải là DANG_O: có checkin, chưa checkout, chưa huy_luc
    public static void KiemTraDangO(DateTime? checkinThucTe, DateTime? checkoutThucTe, DateTime? huyLuc)
    {
        if (huyLuc.HasValue)
            throw new InvalidOperationException("Dòng phòng đã bị hủy/no-show, không thể trả phòng.");
        if (checkoutThucTe.HasValue)
            throw new InvalidOperationException("Phòng này đã được trả.");
        if (!checkinThucTe.HasValue)
            throw new InvalidOperationException("Dòng phòng chưa check-in, không thể trả phòng.");
    }
    // Giá phòng lấy từ giá đã chụp ở CHI_TIET_DAT_PHONG (TP-05)
    public static KetQuaTraPhong Tinh(
        DateTime? checkinThucTe,
        DateTime? checkoutThucTe,
        DateTime? huyLuc,
        long giaMoiDem,
        long giaMoiGio,
        IEnumerable<DongDichVu> dichVu,
        DateTime thoiDiem,
        int thoiGianDonPhut = ThoiGianDonMacDinhPhut,
        CauHinhGioTra? cauHinh = null)
    {
        ArgumentNullException.ThrowIfNull(dichVu);
        KiemTraDangO(checkinThucTe, checkoutThucTe, huyLuc);
        if (thoiGianDonPhut <= 0)
            throw new ArgumentOutOfRangeException(nameof(thoiGianDonPhut), "Thời gian dọn chuẩn phải là số nguyên dương (phút).");

        cauHinh ??= CauHinhGioTra.MacDinh;
        cauHinh.KiemTra();

        var tienPhong = TinhTienPhong.Tinh(checkinThucTe!.Value, thoiDiem, giaMoiDem, giaMoiGio, cauHinh.GioTraChuan);
        long tienDichVu = TinhTienDichVu.Tinh(dichVu);
        var don = DonPhong.BatDau(thoiDiem, thoiGianDonPhut);

        return new KetQuaTraPhong(thoiDiem, tienPhong, tienDichVu, don.DonXongLuc!.Value);
    }
}
