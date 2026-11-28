using Microsoft.Data.Sqlite;
using DoAnNhom23.Data;
using DoAnNhom23.Module1.Models;
namespace DoAnNhom23;

public class DatPhongRepository
{
    private const string CacCot =
        @"id, ma_dat_phong, khach_hang_id, la_vang_lai, ngay_den, ngay_di, tien_coc, han_thanh_toan_coc, thoi_diem_coc, gio_giu_phong_den, trang_thai, ly_do_huy,
                                thoi_diem_huy, ghi_chu_huy, tien_coc_hoan, ma_hoan_coc, thoi_diem_hoan_coc, tk_hoan_ten, tk_hoan_so, tk_hoan_ngan_hang, noi_dung_hoan_coc";

    public static List<DatPhong> LayTatCa(SqliteConnection conn)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = $"SELECT {CacCot} FROM DAT_PHONG ORDER BY id";
        using var r = cmd.ExecuteReader();
        var ds = new List<DatPhong>();
        while (r.Read()) ds.Add(DocMotDong(r));
        return ds;


    }

    //tách riêng để các hàm tìm kiếm sau này dừng lại
    internal static DatPhong DocMotDong(SqliteDataReader r) => new DatPhong
    {
        Id = r.Long("id"),
        MaDatPhong = r.Chuoi("ma_dat_phong"),
        LaVangLai = r.Bool("la_vang_lai"),
        NgayDen = r.ThoiGian("ngay_den"),
        NgayDi = r.ThoiGian("ngay_di"),
        TienCoc = r.Long("tien_coc"),
        HanThanhToanCoc = r.ThoiGianNull("han_thanh_toan_coc"),
        ThoiDiemCoc = r.ThoiGianNull("thoi_diem_coc"),
        GioGiuPhongDen = r.ThoiGianNull("gio_giu_phong_den"),
        TrangThai = r.Enum<TrangThaiDatPhong>("trang_thai"),
        LyDoHuy = r.EnumNull<LyDoHuy>("ly_do_huy"),
        ThoiDiemHuy = r.ThoiGianNull("thoi_diem_huy"),
        GhiChuHuy = r.ChuoiNull("ghi_chu_huy"),
        TienCocHoan = r.Long("tien_coc_hoan"),
        MaHoanCoc = r.ChuoiNull("ma_hoan_coc"),
        ThoiDiemHoanCoc = r.ThoiGianNull("thoi_diem_hoan_coc"),
        TkHoanTen = r.ChuoiNull("tk_hoan_ten"),
        TkHoanSo = r.ChuoiNull("tk_hoan_so"),
        TkHoanNganHang = r.ChuoiNull("tk_hoan_ngan_hang"),
        NoiDungHoanCoc = r.ChuoiNull("noi_dung_hoan_coc")
    };

    public static long Them(SqliteConnection conn, DatPhong d)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"INSERT INTO DAT_PHONG
        (ma_dat_phong, khach_hang_id, la_vang_lai, ngay_den, ngay_di, tien_coc,
         han_thanh_toan_coc, thoi_diem_coc, trang_thai, tk_hoan_ten, tk_hoan_so, tk_hoan_ngan_hang)
        VALUES ($ma, $kh, $vl, $den, $di, $coc, $han, $tdc, $tt, $tkten, $tkso, $tkbank);
        SELECT last_insert_rowid();";
        cmd.Them("$ma", d.MaDatPhong);
        cmd.Them("$kh", d.KhachHangId);
        cmd.Them("$vl", d.LaVangLai ? 1 : 0);
        cmd.ThemGio("$den", d.NgayDen);
        cmd.ThemGio("$di", d.NgayDi);
        cmd.Them("$coc", d.TienCoc);
        cmd.ThemGio("$han", d.HanThanhToanCoc);
        cmd.ThemGio("$tdc", d.ThoiDiemCoc);
        cmd.Them("$tt", d.TrangThai.ToString());
        cmd.Them("$tkten", d.TkHoanTen);
        cmd.Them("$tkso", d.TkHoanSo);
        cmd.Them("$tkbank", d.TkHoanNganHang);
        return (long)cmd.ExecuteScalar()!;
    }
    //hàm kiểm tra cp15 mỗi số điện thoại chỉ có 1 đặt phòng chờ cọc
    public static bool CoChoCocTheoSdt(SqliteConnection conn, string soDienThoai)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"SELECT COUNT(*) FROM DAT_PHONG d
        JOIN KHACH_HANG k ON k.id = d.khach_hang_id
        WHERE d.trang_thai = 'CHO_COC' AND k.so_dien_thoai = $sdt";
        cmd.Them("$sdt", soDienThoai);
        return (long)cmd.ExecuteScalar()! > 0;
    }

}