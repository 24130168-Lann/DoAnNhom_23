using Microsoft.Data.Sqlite;
using DoAnNhom23.Data;
using DoAnNhom23.Models;

namespace DoAnNhom23.Module1.Repositories;


public class LoaiPhongRepository
{

    
    private const string CacCot =
        @"id, ten_loai, gia_theo_gio, gia_theo_ngay";

    public static List<LoaiPhong> LayTatCa(SqliteConnection conn)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = $"SELECT {CacCot} FROM LOAI_PHONG ORDER BY id";
        using var r = cmd.ExecuteReader();
        var ds = new List<LoaiPhong>();
        while (r.Read()) ds.Add(DocMotDong(r));
        return ds;
        
            
    }
    //tách riêng để các hàm tìm kiếm sau này dừng lại
    internal static LoaiPhong DocMotDong(SqliteDataReader r) => new LoaiPhong
    {
        Id = r.Long("id"),
        TenLoai = r.Chuoi("ten_loai"),
        GiaTheoGio = r.Long("gia_theo_gio"),
        GiaTheoNgay = r.Long("gia_theo_ngay"),
    };

}