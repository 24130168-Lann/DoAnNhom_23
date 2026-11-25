using DoAnNhom23.Data;

Db.KhoiTao();
using var conn = Db.MoKetNoi();
using var cmd = conn.CreateCommand();
cmd.CommandText = "SELECT COUNT(*) FROM DAT_PHONG;";
Console.WriteLine(cmd.ExecuteScalar());