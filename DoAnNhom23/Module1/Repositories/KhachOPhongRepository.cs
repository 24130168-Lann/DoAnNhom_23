using Microsoft.Data.Sqlite;
using DoAnNhom23.Data;
namespace DoAnNhom23.Module1.Repositories;

public static class KhachOPhongRepository
{
    public static void Them(SqliteConnection conn, long chiTietId, long khachHangId)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "INSERT INTO KHACH_O_PHONG (chi_tiet_dat_phong_id, khach_hang_id) VALUES ($ct, $kh)";
        cmd.Them("$ct", chiTietId);
        cmd.Them("$kh", khachHangId);
        cmd.ExecuteNonQuery();
    }
}