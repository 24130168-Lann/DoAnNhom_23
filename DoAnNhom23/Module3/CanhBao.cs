namespace khachSan;

/// <summary>
/// một cảnh báo dữ liệu
/// DongId là id bản ghi liên quan(dòng phòng); với cảnh báo cấpdđặt phonòng là id phòng, dc nói r trong nội dung
/// là record nên hai caảnh báo giống nhau dc coi la một
/// </summary>
/// <param name="Loai"></param>
/// <param name="NoiDung"></param>
/// <param name="DongIF"></param>
public sealed record CanhBao(LoaiCanhBao Loai, string NoiDung, int? DongId = null);

internal static class canhBaoExtensions
{
    // thêm cảnh báo nếu chua co canh bao y hệt(tránh lập lại khi quét lại theo tùng tháng)
    public static void ThemDuyNhat(this List<CanhBao> ds, CanhBao canhBao)
    {
        if(!ds.Contains(canhBao))
        {
            ds.Add(canhBao);
        }
    }
}