namespace khachSan;

public sealed record CanhBao(LoaiCanhBao Loai, string NoiDung, int? DongIF = null);

internal static class canhBaoExten
{
    public static void ThemDuyNhat(this List<CanhBao> ds, CanhBao canhBao)
    {
        if(!ds.Contains(canhBao))
        {
            ds.Add(canhBao);
        }
    }
}