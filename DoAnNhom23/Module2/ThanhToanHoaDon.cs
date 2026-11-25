namespace DoAnNhom23.Module2;

// Kết quả mô phỏng thanh toán; GhiNhanMoi = false nghĩa là đã xử lý trước đó (idempotent)
public record KetQuaThanhToan(KetQuaHoaDon HoaDon, bool GhiNhanMoi);

// CO-05 (thanh toán ) và CO-08 (danh sách hóa đơn chưa thanh toán)
public static class ThanhToanHoaDon
{
    // hệ thống tự lấy so_tien_con_lai. Gọi lại / bấm lặp không ghi nhận lần hai.
    public static KetQuaThanhToan MoPhongThanhToan(KetQuaHoaDon hoaDon, DateTime thoiDiem)
    {
        ArgumentNullException.ThrowIfNull(hoaDon);

        if (hoaDon.TrangThai == TrangThaiHoaDon.DA_THANH_TOAN || hoaDon.SoTienConLai <= 0)
            return new KetQuaThanhToan(hoaDon, false);

        // CHECK của HOA_DON: thoi_diem_thanh_toan >= thoi_diem_lap
        if (thoiDiem < hoaDon.ThoiDiemLap)
            throw new ArgumentException("Thời điểm thanh toán không được trước thời điểm lập hóa đơn.", nameof(thoiDiem));

        return new KetQuaThanhToan(
            hoaDon with { TrangThai = TrangThaiHoaDon.DA_THANH_TOAN, ThoiDiemThanhToan = thoiDiem },
            true);
    }

    // Nội dung QR chứa mã hóa đơn và đúng số tiền còn phải trả; chỉ có khi còn dương (M-14)
    public static string? NoiDungQr(KetQuaHoaDon hoaDon)
    {
        ArgumentNullException.ThrowIfNull(hoaDon);
        return hoaDon.CanThanhToan ? $"{hoaDon.MaHoaDon}|{hoaDon.SoTienConLai}" : null;
    }

    // CO-08: CHO_THANH_TOAN và so_tien_con_lai > 0, mới lập nhất trước
    public static IReadOnlyList<KetQuaHoaDon> HoaDonChuaThanhToan(IEnumerable<KetQuaHoaDon> cacHoaDon)
    {
        ArgumentNullException.ThrowIfNull(cacHoaDon);
        return cacHoaDon
            .Where(h => h.TrangThai == TrangThaiHoaDon.CHO_THANH_TOAN && h.SoTienConLai > 0)
            .OrderByDescending(h => h.ThoiDiemLap)
            .ToList();
    }
}