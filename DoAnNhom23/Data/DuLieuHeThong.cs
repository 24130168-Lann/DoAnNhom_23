// Data/DuLieuHeThong.cs: đọc một lượt mọi thứ mà các hàm kiểm tra cần
using Microsoft.Data.Sqlite;
using DoAnNhom23.Module1.Models;
using DoAnNhom23.Module1.Repositories;
namespace DoAnNhom23.Data;

public record DuLieuHeThong(List<Phong> Phongs, List<DatPhong> DatPhongs,
    List<ChiTietDatPhong> ChiTiets, List<LoaiPhong> LoaiPhongs, int ThoiGianDon)
{
    public static DuLieuHeThong Tai(SqliteConnection conn) => new(
        PhongRepository.LayTatCa(conn),
        DatPhongRepository.LayTatCa(conn),
        ChiTietDatPhongRepository.LayTatCa(conn),
        LoaiPhongRepository.LayTatCa(conn),
        CauHinhRepository.ThoiGianDonPhut(conn));
}