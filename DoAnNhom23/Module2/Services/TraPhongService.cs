using DoAnNhom23.Module2.Models;
using DoAnNhom23.Module2.Results;

namespace DoAnNhom23.Module2.Services;

// CO-03: check-out một dòng phòng
public static class TraPhongService
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

        var tienPhong = TinhTienPhongService.Tinh(checkinThucTe!.Value, thoiDiem, giaMoiDem, giaMoiGio, cauHinh.GioTraChuan);
        long tienDichVu = TinhTienDichVuService.Tinh(dichVu);
        var don = DonPhongService.BatDau(thoiDiem, thoiGianDonPhut);

        return new KetQuaTraPhong(thoiDiem, tienPhong, tienDichVu, don.DonXongLuc!.Value);
    }
}