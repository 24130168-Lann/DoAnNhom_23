namespace khachSan;
/// <summary>
/// Điểm vào của module: kiểm tra quyền, kiểm tra năm, lấy một thời điểm chốt,
/// tải dữ liệu nguồn một ần ồi giao choa TinhCongSyat / TinhDoanhThu.
/// Báo cáo Tính lại lúc xem, không lưu snapshot
/// </summary>
public sealed class DichVuBaoCao
{
    private readonly INguonDuLieu Nguon;
    private readonly Func<DateTime> DongHo;

    public DichVuBaoCao(INguonDuLieu nguon, Func<DateTime>? dongHo = null)
    {
        Nguon = nguon ?? throw new ArgumentNullException(nameof(nguon));
        DongHo = dongHo ?? (() => DateTime.Now);
    }

    public KetQuaCongSuat BaoCaoCongSuat(VaiTro vaiTro, int nam)
    {
        var thoiDiemBaoCao = ChuanBi(vaiTro, nam);
        var du = Nguon.TaiDuLieu(thoiDiemBaoCao);
        return new TinhCongSuat(du).Tinh(nam);
    }

    public KetQuaDoanhThu BaoCaoDoanhThu(VaiTro vaiTro, int nam)
    {
        var thoiDiemBaoCao = ChuanBi(vaiTro, nam);
        var du = Nguon.TaiDuLieu(thoiDiemBaoCao);
        return new TinhDoanhThu(du, new TinhCocGiuLai(du)).Tinh(nam);
    }
    private DateTime ChuanBi(VaiTro vaiTro, int nam)
    {
        if (vaiTro != VaiTro.Chu)
        {
            throw new UnauthorizedAccessException("Chi tai khoan chu moiws duoc xem bao cao");
        }

        var thoiDiemBaoCao = DongHo();
        if (!KiemTraDauVao.NamHopLe(nam, thoiDiemBaoCao))
        {
            throw new ArgumentOutOfRangeException(nameof(nam), nam, "nam phai tinh tu 2000 den nam hien tai");
        }

        return thoiDiemBaoCao;
    }
}