using Microsoft.Data.Sqlite;

namespace DoAnNhom23.Data;

public static class Db
{
    // File .db tự tạo lần đầu mở kết nối, nằm cạnh file .exe. Muốn làm lại từ đầu thì xóa file này.
    private static readonly string FileDb =
        Path.Combine(AppContext.BaseDirectory, "QuanLyKhachSan.db");

    // schema.sql và data.sql được copy ra thư mục Data cạnh file .exe.
    private static readonly string ThuMucSql =
        Path.Combine(AppContext.BaseDirectory, "Data");

    // Mở kết nối đã bật khóa ngoại và trigger đệ quy.
    // Nơi gọi phải đóng kết nối: using var conn = Db.MoKetNoi();
    public static SqliteConnection MoKetNoi()
    {
        var cs = new SqliteConnectionStringBuilder
        {
            DataSource = FileDb,
            ForeignKeys = true
        }.ToString();

        var conn = new SqliteConnection(cs);
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = "PRAGMA recursive_triggers = ON;";
        cmd.ExecuteNonQuery();

        return conn;
    }

    // Chạy một lần khi app khởi động. DB đã có (user_version khác 0) thì bỏ qua.
    public static void KhoiTao()
    {
        using var conn = MoKetNoi();

        if (LayUserVersion(conn) != 0) return;

        ChayFile(conn, "schema.sql");
        ChayFile(conn, "data.sql");

        // schema.sql đặt PRAGMA user_version = 3 ở cuối; nếu vẫn là 0 nghĩa là script chưa chạy đúng
        if (LayUserVersion(conn) == 0)
            throw new InvalidOperationException(
                "Chay schema.sql xong nhung database van chua duoc khoi tao. Kiem tra noi dung file schema.sql.");
    }

    private static long LayUserVersion(SqliteConnection conn)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "PRAGMA user_version;";
        return (long)cmd.ExecuteScalar()!;
    }

    private static void ChayFile(SqliteConnection conn, string tenFile)
    {
        string duongDan = Path.Combine(ThuMucSql, tenFile);

        if (!File.Exists(duongDan))
            throw new FileNotFoundException("Khong tim thay file SQL: " + duongDan);

        string sql = File.ReadAllText(duongDan);

        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        cmd.ExecuteNonQuery();
    }
}