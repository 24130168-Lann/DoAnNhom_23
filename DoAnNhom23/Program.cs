using DoAnNhom23;
using DoAnNhom23.Data;

Db.KhoiTao();
using var conn = Db.MoKetNoi();
var phongs = PhongRepository.LayTatCa(conn);
Console.WriteLine($"Số phòng: {phongs.Count}");
foreach (var p in phongs.Take(3))
    Console.WriteLine($"{p.SoPhong} | tầng {p.Tang} | {p.CheDo} | dọn dẹp: {p.CanDonDep}");
{
    
}