namespace khachSan;

public class KiemTraDongPhong
{
    public static string? LyDoKhongHopLe(ChiTietDatPhong dong, DateTime thoiDiemBaoCao)
    {
        if (dong.CheckinThucTe is null)
        {
            return dong.CheckoutThucTe is null ? null : "co check out thuc te nhung thieu check in thuc te.";
        }

        if (dong.HuyLuc is not null)
        {
            return "Dong co huy luc(noshow) nhungw dong thoi co checkin thuc te.";
        }

        var vao = dong.CheckinThucTe.Value;
        if (vao > thoiDiemBaoCao)
        {
            return "checkin thuc te nam sau thoi diem chot bao cao.";
        }

        if (dong.CheckoutThucTe is { } ra)
        {
            if (ra <= vao)
            {
                return "checkout thuc te ko lon hon checkin thuc te";
            }
            if (ra > thoiDiemBaoCao)
            {
                return "checkout thuc te nam sau thoi diem chot bao cao.";
            }
        }

        return null;
    }

    public static CanhBao TaoCanhBao(ChiTietDatPhong dong, string lyDo)
        => new(LoaiCanhBao.TimestampKhongHopLe, $"dong phong #{dong.Id}: {lyDo}", dong.Id);

    public static bool LienQuanDenNam(ChiTietDatPhong dong, int nam, DateTime thoiDiemBaoCao)
    {
        var batDau = dong.CheckinThucTe ?? dong.CheckoutThucTe ?? dong.HuyLuc;
        if (batDau is null)
        {
            return false;
        }
        
        var bd = batDau.Value;
        var kt = dong.CheckoutThucTe ?? thoiDiemBaoCao;
        if (kt < bd)
        {
            kt = bd;
        }

        return bd.Year <= nam && kt.Year >= nam;
    }
}