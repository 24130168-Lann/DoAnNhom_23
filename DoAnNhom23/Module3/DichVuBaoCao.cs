namespace khachSan;

public sealed class DichVuBaoCao
{
    private readonly INguonDuLieu nguon;
    private readonly Func<DateTime> dongHo;

    public DichVuBaoCao(INguonDuLieu nguon, Func<DateTime>? dongHo = null)
    {
        nguon = nguon ?? throw new ArgumentNullException(nameof(nguon));
        dongHo = dongHo ?? (() => DateTime.Now);
    }

    public KetQuaCongSuat BaoCaoCongSuat(VaiTro vaiTro, int nam)
    {
        var thoiDiemBaoCao = ChuanBi(vaiTro, nam);
        var du = nguon.TaiDuLieu(thoiDiemBaoCao);
        return new TinhDoanhThu(du, new TinhCocGiuLai(du)).Tinh(nam);
    }

    private DateTime ChuanBi(VaiTro vaiTro, int nam)
    {
        if (vaiTro != VaiTro.Chu)
        {
            throw new UnauthorizedAccessException("Chi tai khoan chu moiws duoc xem bao cao");
        }

        var thoiDiemBaoCao = dongHo();
        if (!KiemTraDauVao.NamHopLe(nam, thoiDiemBaoCao))
        {
            throw new ArgumentOutOfRangeException(nameof(nam), nam, "nam phai tinh tu 2000 den nam hien tai");
        }

        return thoiDiemBaoCao;
    }
}