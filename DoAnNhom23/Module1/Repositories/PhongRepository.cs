using Microsoft.Data.Sqlite;
using DoAnNhom23.Data;
using DoAnNhom23.Module1.Models;

namespace DoAnNhom23;

public static class PhongRepository
{
    public static List<Phong> LayTatCa(SqliteConnection conn)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"SELECT id, loai_phong_id, so_phong, tang, che_do, can_don_dep, don_xong_luc
                                FROM PHONG ORDER BY tang, so_phong";
        using var r = cmd.ExecuteReader();
        var ds = new List<Phong>();
        while (r.Read())
        {
            ds.Add(new Phong
            {
                Id = r.Long("id"),
                LoaiPhongId = r.Long("loai_phong_id"),
                SoPhong = r.Chuoi("so_phong"),
                Tang = r.Int("tang"),
                CheDo = r.Enum<CheDoPhong>("che_do"),
                CanDonDep = r.Bool("can_don_dep"),
                DonXongLuc = r.ThoiGianNull("don_xong_luc")
            });
        }

        return ds;
    }
}