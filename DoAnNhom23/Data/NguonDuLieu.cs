using DateTime = System.DateTime;

using System.Globalization;
using Microsoft.Data.Sqlite;

namespace khachSan.Data;

/// <summary>
/// Đọc dữ liệu nguồn từ SQLite (PHONG, CAU_HINH, DAT_PHONG, CHI_TIET_DAT_PHONG, CHI_TIET_DICH_VU)
/// rồi đổ vào DuLieuNguon cho Module 3. Chỉ đọc, không ghi; mọi bảng được đọc trong một transaction.
/// </summary>
public sealed class NguonDuLieuSqlite : INguonDuLieu
{
    public DuLieuNguon TaiDuLieu(DateTime thoiDiemBaoCao)
    {
        using var conn = Db.MoKetNoi();
        using var tran = conn.BeginTransaction();   // một Read transaction: số liệu nhất quán

        var phongs = DocPhong(conn);
        var cauHinh = DocCauHinh(conn);
        var datPhongs = DocDatPhong(conn);

        // Gắn dòng phòng vào đặt phòng, dịch vụ vào dòng phòng
        var theoDat = datPhongs.ToDictionary(d => d.Id);
        var theoDong = new Dictionary<int, ChiTietDatPhong>();

        foreach (var dong in DocChiTietDatPhong(conn))
        {
            if (!theoDat.TryGetValue(dong.DatPhongId, out var dp)) continue;
            dp.CacDong.Add(dong);
            theoDong[dong.Id] = dong;
        }

        foreach (var dv in DocChiTietDichVu(conn))
        {
            if (theoDong.TryGetValue(dv.ChiTietDatPhongId, out var dong))
                dong.DichVus.Add(dv);
        }

        tran.Commit();
        return new DuLieuNguon(thoiDiemBaoCao, phongs, datPhongs, cauHinh);
    }

    /// <summary>Các năm từ năm có dữ liệu sớm nhất đến năm hiện tại (để đổ vào ô chọn năm).</summary>
    public List<int> CacNamCoDuLieu(DateTime bayGio)
    {
        using var conn = Db.MoKetNoi();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT MIN(nam) FROM (
                SELECT MIN(CAST(substr(ngay_den, 1, 4) AS INTEGER)) AS nam FROM DAT_PHONG
                UNION ALL
                SELECT MIN(CAST(substr(checkin_thuc_te, 1, 4) AS INTEGER)) FROM CHI_TIET_DAT_PHONG
            );";
        var ketQua = cmd.ExecuteScalar();

        var ds = new List<int>();
        if (ketQua is null or DBNull) return ds;

        int dau = Math.Max(Convert.ToInt32(ketQua, CultureInfo.InvariantCulture), KiemTraDauVao.NamToiThieu);
        for (int n = dau; n <= bayGio.Year; n++) ds.Add(n);
        return ds;
    }

    // ───────────────────────── Đọc từng bảng ─────────────────────────

    private static List<Phong> DocPhong(SqliteConnection conn)
    {
        var ds = new List<Phong>();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT id, che_do FROM PHONG ORDER BY id;";
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            ds.Add(new Phong
            {
                Id = r.Int("id"),
                CheDo = r.Chuoi("che_do") == "THEO_GIO" ? CheDoPhong.TheoGio : CheDoPhong.TheoNgay,
            });
        }
        return ds;
    }

    private static CauHinh DocCauHinh(SqliteConnection conn)
    {
        var giaTri = new Dictionary<string, string?>();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT ten_tham_so, gia_tri FROM CAU_HINH;";
        using var r = cmd.ExecuteReader();
        while (r.Read()) giaTri[r.Chuoi("ten_tham_so")] = r.ChuoiNull("gia_tri");

        return new CauHinh
        {
            GioNhanChuan = DocGio(giaTri, "gio_nhan_chuan"),
            GioTraChuan = DocGio(giaTri, "gio_tra_chuan"),
        };
    }

    // "14:00" -> TimeSpan; thiếu hoặc sai định dạng -> null (báo cáo sẽ hiện cảnh báo cấu hình)
    private static TimeSpan? DocGio(Dictionary<string, string?> cauHinh, string ten)
        => cauHinh.TryGetValue(ten, out var s)
           && TimeSpan.TryParseExact(s, @"h\:mm", CultureInfo.InvariantCulture, out var gio)
            ? gio
            : null;

    private static List<DatPhong> DocDatPhong(SqliteConnection conn)
    {
        var ds = new List<DatPhong>();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT id, trang_thai, tien_coc, thoi_diem_coc, tien_coc_hoan, thoi_diem_huy, ly_do_huy
            FROM DAT_PHONG ORDER BY id;";
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            ds.Add(new DatPhong
            {
                Id = r.Int("id"),
                TrangThai = TuTrangThai(r.Chuoi("trang_thai")),
                TienCoc = r.Long("tien_coc"),
                ThoiDiemCoc = r.ThoiGianNull("thoi_diem_coc"),
                TienCocHoan = r.Long("tien_coc_hoan"),
                ThoiDiemHuy = r.ThoiGianNull("thoi_diem_huy"),
                LyDoHuy = r.ChuoiNull("ly_do_huy"),
            });
        }
        return ds;
    }

    private static List<ChiTietDatPhong> DocChiTietDatPhong(SqliteConnection conn)
    {
        var ds = new List<ChiTietDatPhong>();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT id, dat_phong_id, phong_id, gia_moi_dem, checkin_thuc_te, checkout_thuc_te,
                   huy_luc, coc_giu_lai, tien_phong, tien_dich_vu
            FROM CHI_TIET_DAT_PHONG ORDER BY id;";
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            ds.Add(new ChiTietDatPhong
            {
                Id = r.Int("id"),
                DatPhongId = r.Int("dat_phong_id"),
                PhongId = (int)(r.LongNull("phong_id") ?? 0),   // chưa gán phòng (vd. no-show) -> 0
                GiaMoiDem = r.Long("gia_moi_dem"),
                CheckinThucTe = r.ThoiGianNull("checkin_thuc_te"),
                CheckoutThucTe = r.ThoiGianNull("checkout_thuc_te"),
                HuyLuc = r.ThoiGianNull("huy_luc"),
                CocGiuLai = r.Long("coc_giu_lai"),
                TienPhong = r.LongNull("tien_phong") ?? 0,      // NULL khi chưa checkout
                TienDichVu = r.LongNull("tien_dich_vu") ?? 0,
            });
        }
        return ds;
    }

    private static List<ChiTietDichVu> DocChiTietDichVu(SqliteConnection conn)
    {
        var ds = new List<ChiTietDichVu>();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT id, chi_tiet_dat_phong_id, so_luong, don_gia, thoi_diem, da_xoa
            FROM CHI_TIET_DICH_VU ORDER BY id;";
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            ds.Add(new ChiTietDichVu
            {
                Id = r.Int("id"),
                ChiTietDatPhongId = r.Int("chi_tiet_dat_phong_id"),
                SoLuong = r.Int("so_luong"),
                DonGia = r.Long("don_gia"),
                ThoiDiem = r.ThoiGian("thoi_diem"),
                DaXoa = r.Bool("da_xoa"),
            });
        }
        return ds;
    }

    // Giá trị trong DB là CHO_COC / DA_DAT / DANG_O / DA_TRA / DA_HUY
    private static TrangThaiDatPhong TuTrangThai(string s) => s switch
    {
        "CHO_COC" => TrangThaiDatPhong.ChoCoc,
        "DA_DAT" => TrangThaiDatPhong.DaDat,
        "DANG_O" => TrangThaiDatPhong.DangO,
        "DA_TRA" => TrangThaiDatPhong.DaTra,
        "DA_HUY" => TrangThaiDatPhong.DaHuy,
        _ => throw new InvalidDataException("Trang thai dat phong khong hop le: " + s),
    };
}
