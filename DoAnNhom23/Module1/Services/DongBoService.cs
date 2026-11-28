using Microsoft.Data.Sqlite;
using DoAnNhom23.Data;
namespace DoAnNhom23.Module1.Services;

public static class DongBoService
{
    // CP-14: đặt phòng CHO_COC đã hết hạn thì tự hủy, trả suất phòng
    public static int XuLyHetHanCoc(SqliteConnection conn, DateTime now)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"UPDATE DAT_PHONG
            SET trang_thai = 'DA_HUY', ly_do_huy = 'HET_HAN_COC',
                thoi_diem_huy = $now, han_thanh_toan_coc = NULL
            WHERE trang_thai = 'CHO_COC' AND datetime(han_thanh_toan_coc) <= datetime($now)";
        cmd.ThemGio("$now", now);
        return cmd.ExecuteNonQuery();
    }

    // BR-14: phòng đã dọn xong (don_xong_luc đã qua) thì đồng bộ về trống
    public static int DongBoDonDep(SqliteConnection conn, DateTime now)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"UPDATE PHONG SET can_don_dep = 0, don_xong_luc = NULL
            WHERE can_don_dep = 1 AND datetime(don_xong_luc) <= datetime($now)";
        cmd.ThemGio("$now", now);
        return cmd.ExecuteNonQuery();
    }
}