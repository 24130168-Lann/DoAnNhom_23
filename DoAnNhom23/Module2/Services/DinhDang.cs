using System.Globalization;
using System.Text;

namespace DoAnNhom23.Module2.Services;

// Hàm định dạng dùng chung trong Module 2
internal static class DinhDang
{
    public static string Tien(long soTien)
        => soTien.ToString("N0", CultureInfo.InvariantCulture).Replace(',', '.');

    public static string BoDauVietHoa(string chuoi)
    {
        var phanTach = chuoi.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder(phanTach.Length);
        foreach (var c in phanTach)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.NonSpacingMark) continue;
            sb.Append(c switch { 'đ' => 'd', 'Đ' => 'D', _ => c });
        }
        var ketQua = sb.ToString().Normalize(NormalizationForm.FormC).ToUpperInvariant();
        return string.Join(' ', ketQua.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));
    }
}