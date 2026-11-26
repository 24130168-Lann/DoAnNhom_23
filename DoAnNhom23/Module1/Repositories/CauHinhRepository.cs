using System.Runtime.InteropServices;
using Microsoft.Data.Sqlite;
using DoAnNhom23.Data;
namespace DoAnNhom23.Module1.Repositories;

public class CauHinhRepository
{
    public static string LayChuoi(SqliteConnection conn, string tenThamSo)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT gia_tri FROM CAU_HINH WHERE ten_tham_so = $ten";
        cmd.Parameters.AddWithValue("$ten", tenThamSo);
        var kq = cmd.ExecuteScalar();
        if (kq is null or DBNull)
            throw new InvalidOperationException($"Thiếu tham số cấu hình: {tenThamSo}");
        return (string)kq;
    }
    public static int LayInt(SqliteConnection conn, string tenThamSo)
    => int.Parse(LayChuoi(conn, tenThamSo));
    
    //các tham số hay dùng
    public static int ThoiGianDonPhut(SqliteConnection conn) => LayInt(conn, "thoi_gian_don_phut");
    public static int ThoiGianCocPhut(SqliteConnection conn) =>  LayInt(conn, "thoi_gian_coc_phut");
}