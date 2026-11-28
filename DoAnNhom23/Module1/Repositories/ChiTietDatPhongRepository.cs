using Microsoft.Data.Sqlite;
using DoAnNhom23.Data;
using DoAnNhom23.Module1.Models;
namespace DoAnNhom23.Module1.Repositories;


public class ChiTietDatPhongRepository
{

    
    private const string CacCot =
        @"id, dat_phong_id, loai_phong_id, phong_id, gia_moi_dem, gia_moi_gio, checkin_thuc_te, checkout_thuc_te, huy_luc, coc_giu_lai";

    public static List<ChiTietDatPhong> LayTatCa(SqliteConnection conn)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = $"SELECT {CacCot} FROM CHI_TIET_DAT_PHONG ORDER BY id";
        using var r = cmd.ExecuteReader();
        var ds = new List<ChiTietDatPhong>();
        while (r.Read()) ds.Add(DocMotDong(r));
        return ds;
        
            
    }
    //tách riêng để các hàm tìm kiếm sau này dừng lại
    internal static ChiTietDatPhong DocMotDong(SqliteDataReader r) => new ChiTietDatPhong
    {
       Id = r.Long("id"),
       DatPhongId = r.Long("dat_phong_id"),
       LoaiPhongId = r.Long("loai_phong_id"),
       PhongId = r.LongNull("phong_id"),
       GiaMoiDem = r.Long("gia_moi_dem"),
       GiaMoiGio = r.Long("gia_moi_gio"),
       CheckinThucTe = r.ThoiGianNull("checkin_thuc_te"),
       CheckoutThucTe = r.ThoiGianNull("checkout_thuc_te"),
       HuyLuc = r.ThoiGianNull("huy_luc"),
       CocGiuLai  = r.Long("coc_giu_lai"),
    };

    public static long Them(SqliteConnection conn, ChiTietDatPhong c)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"INSERT INTO CHI_TIET_DAT_PHONG
        (dat_phong_id, loai_phong_id, phong_id, gia_moi_dem, gia_moi_gio, checkin_thuc_te)
        VALUES ($dp, $lp, $ph, $gd, $gg, $ci);
        SELECT last_insert_rowid();";
        cmd.Them("$dp", c.DatPhongId);
        cmd.Them("$lp", c.LoaiPhongId);
        cmd.Them("$ph", c.PhongId);
        cmd.Them("$gd", c.GiaMoiDem);
        cmd.Them("$gg", c.GiaMoiGio);
        cmd.ThemGio("$ci", c.CheckinThucTe);
        return (long)cmd.ExecuteScalar()!;
    }

}
