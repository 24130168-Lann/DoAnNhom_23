using System.Globalization;

namespace DoAnNhom23.Module2;

//cấu hình giờ trả phòng chuẩn(CAU_HINH.gio_tra_chuan, định dạng theo giờ:phút,mặc định 12:00)
public record CauHinhGioTra(TimeSpan GioTraChuan, int? SoGioTreToiDa = null, int? SoGioThueTheoGioToiDa = null)
{
    public static CauHinhGioTra MacDinh { get; } = new(new TimeSpan(12,0,0));
//giờ trả chun từ 00:00 đến 23:59
    public void KiemTra()
    {
        if(GioTraChuan < TimeSpan.Zero || GioTraChuan >= TimeSpan.FromDays(1))
            throw new ArgumentOutOfRangeException(nameof(GioTraChuan),
                "Giờ trả chuẩn phải nằm trong một ngày(00:00 đến 23:59)");
    }

    public static CauHinhGioTra TuChuoi(string? giaTri)
    {
        if (!TimeSpan.TryParseExact(giaTri, @"hh\:mm", CultureInfo.InvariantCulture, out var gio))
            throw new FormatException($"gio_tra_chuan phảicosos định dạng HH:mm hợp lệ(giá trị: \"{giaTri}\")");
        var cauHinh = new CauHinhGioTra(gio);
        cauHinh.KiemTra();
        return cauHinh;
    }
}
