using Microsoft.Data.Sqlite;
namespace DoAnNhom23.Data;
//hàm hỗ trợ truyền tham só
public static class GhiDuLieu
{
  public static void Them(this SqliteCommand cmd, string ten, object? giaTri) =>
    cmd.Parameters.AddWithValue(ten, giaTri ?? DBNull.Value);
  public static void ThemGio(this SqliteCommand cmd, string ten, DateTime? t)
      => cmd.Parameters.AddWithValue(ten, t == null ? DBNull.Value : Gio.ThanhChuoi(t.Value));
}