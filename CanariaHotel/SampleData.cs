namespace khachSan.GiaoDien;
/// <summary>
/// Số liệu mẫu lấy theo ảnh thiết kế, chỉ để hiển thị giao diện.
/// Khi nối CSDL, thay các mảng này bằng dữ liệu thật (null = tháng chưa có số liệu).
/// </summary>
internal static class SampleData
{
    public static readonly string[] Months =
        { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

    // ── BC-01 · Công suất phòng ──────────────────────────
    public const int NominalRoomHours = 28_800;

    public static readonly double?[] OccupancyThisYear =
        { 65.2, 68.7, 71.5, 73.6, 76.1, 74.8, 77.3, 73.9, 73.7, 71.0, 72.4, null };

    public static readonly double?[] OccupancyPrevious =
        { 62.0, 64.3, 67.8, 70.1, 72.9, 71.7, 73.6, 72.2, 70.8, 67.2, 68.1, null };

    public static readonly int?[] OccupiedRoomHours =
        { 18_720, 19_560, 20_520, 21_168, 21_888, 21_558, 22_266, 21_888, 21_168, 20_448, 20_851, null };

    // ── BC-02 · Doanh thu (đơn vị: tỷ đồng, dùng cho biểu đồ) ──
    public static readonly double?[] RoomRevenue    = { 1.1, 1.2, 1.3, 1.4, 1.5, 1.5, 1.6, 1.5, 1.4, 1.3, 1.4, null };
    public static readonly double?[] ServiceRevenue = { 0.6, 0.7, 0.7, 0.8, 0.8, 0.9, 1.0, 0.9, 0.8, 0.7, 0.8, null };
    public static readonly double?[] DepositKept    = { 0.2, 0.2, 0.3, 0.3, 0.3, 0.3, 0.4, 0.4, 0.3, 0.3, 0.3, null };

    // Bảng chi tiết (VNĐ): phòng, dịch vụ, tổng doanh thu, cọc giữ lại, tổng giá trị ghi nhận
    public static readonly long[][] RevenueRows =
    {
        new long[] { 1_100_000_000, 600_000_000, 1_700_000_000, 200_000_000, 1_900_000_000 },
        new long[] { 1_200_000_000, 700_000_000, 1_900_000_000, 220_000_000, 2_120_000_000 },
        new long[] { 1_300_000_000, 700_000_000, 2_000_000_000, 300_000_000, 2_300_000_000 },
    };

    public static string Vnd(long value) => value.ToString("N0", Theme.Vi);
}