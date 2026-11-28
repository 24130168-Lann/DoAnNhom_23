using Microsoft.Data.Sqlite;
using DoAnNhom23.Data;
using DoAnNhom23.Module1.Models;
namespace DoAnNhom23.Module1.Repositories;

public static class KhachHangRepository
{
    // Tìm khách đã có theo giấy tờ (cặp loại + số là duy nhất)
    public static KhachHang? TimTheoGiayTo(SqliteConnection conn, LoaiGiayTo loai, string soGiayTo)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"SELECT id, ho_ten, so_dien_thoai, loai_giay_to, so_giay_to
                            FROM KHACH_HANG WHERE loai_giay_to = $loai AND so_giay_to = $so";
        cmd.Them("$loai", loai.ToString());
        cmd.Them("$so", soGiayTo);
        using var r = cmd.ExecuteReader();
        if (!r.Read()) return null;
        return new KhachHang
        {
            Id = r.Long("id"),
            HoTen = r.Chuoi("ho_ten"),
            SoDienThoai = r.ChuoiNull("so_dien_thoai"),
            LoaiGiayTo = r.EnumNull<LoaiGiayTo>("loai_giay_to"),
            SoGiayTo = r.ChuoiNull("so_giay_to")
        };
    }

    public static long Them(SqliteConnection conn, KhachHang kh)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"INSERT INTO KHACH_HANG (ho_ten, so_dien_thoai, loai_giay_to, so_giay_to)
                            VALUES ($ten, $sdt, $loai, $so);
                            SELECT last_insert_rowid();";
        cmd.Them("$ten", kh.HoTen);
        cmd.Them("$sdt", kh.SoDienThoai);
        cmd.Them("$loai", kh.LoaiGiayTo?.ToString());
        cmd.Them("$so", kh.SoGiayTo);
        return (long)cmd.ExecuteScalar()!;
    }
}