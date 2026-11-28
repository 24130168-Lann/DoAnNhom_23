namespace DoAnNhom23.Module1.Services;

public class KiemTraDuLieu
{
    public static string? HoTen(string? s)
    {
        if (string.IsNullOrWhiteSpace(s)) return "Họ tên không được để trống.";
        if (s.Trim().Length > 100) return "Họ tên tối đa 100 ký tự.";
        return null;
    }

    public static string? SoDienThoai(string? s)
        => s != null && s.Length == 10 && s[0] == '0' && s.All(char.IsAsciiDigit)
            ? null : "Số điện thoại gồm đúng 10 chữ số, bắt đầu bằng 0.";

    public static string? GiayTo(LoaiGiayTo? loai, string? so, bool batBuoc)
    {
        bool trongSo = string.IsNullOrWhiteSpace(so);
        if (loai == null && trongSo) return batBuoc ? "Giấy tờ của người đặt là bắt buộc." : null;
        if (loai == null || trongSo) return "Loại giấy tờ và số giấy tờ phải cùng có hoặc cùng để trống.";
        if (loai == LoaiGiayTo.CCCD)
            return so!.Length == 12 && so.All(char.IsAsciiDigit) ? null : "Số CCCD gồm đúng 12 chữ số.";
        return so!.Length is >= 6 and <= 12 && so.All(char.IsAsciiLetterOrDigit)
            ? null : "Số hộ chiếu gồm 6 đến 12 ký tự chữ và số.";
    }
}