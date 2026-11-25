using System.Globalization;

namespace DoAnNhom23.Module2;

// Cấu hình giờ trả chuẩn (định dạng HH:mm, mặc định 12:00)
public record CauHinhGioTra(TimeSpan GioTraChuan, int? SoGioTreToiDa = null, int? SoGioThueTheoGioToiDa = null)
{
    public static CauHinhGioTra MacDinh { get; } = new(new TimeSpan(12, 0, 0));

    // Đọc từ chuỗi trong CAU_HINH, ví dụ "12:00"
    public static CauHinhGioTra TuChuoi(string? gioTraChuan)
    {
        if (!TimeSpan.TryParseExact(gioTraChuan?.Trim(), @"hh\:mm", CultureInfo.InvariantCulture, out var gio))
            throw new FormatException($"gio_tra_chuan phải có dạng HH:mm (nhận được \"{gioTraChuan}\").");
        var cauHinh = new CauHinhGioTra(gio);
        cauHinh.KiemTra();
        return cauHinh;
    }

    public void KiemTra()
    {
        if (GioTraChuan < TimeSpan.Zero || GioTraChuan >= TimeSpan.FromDays(1))
            throw new ArgumentOutOfRangeException(nameof(GioTraChuan),
                "Giờ trả chuẩn phải nằm trong một ngày (00:00 đến 23:59).");
        if (SoGioTreToiDa is <= 0)
            throw new ArgumentOutOfRangeException(nameof(SoGioTreToiDa), "Số giờ trễ tối đa phải là số dương.");
        if (SoGioThueTheoGioToiDa is <= 0)
            throw new ArgumentOutOfRangeException(nameof(SoGioThueTheoGioToiDa), "Số giờ thuê theo giờ tối đa phải là số dương.");
    }
}