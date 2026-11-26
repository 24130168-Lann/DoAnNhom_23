using DoAnNhom23;
using DoAnNhom23.Data;
using DoAnNhom23.Module1.Repositories;

Db.KhoiTao();
using var conn = Db.MoKetNoi();
var phongs = PhongRepository.LayTatCa(conn);
Console.WriteLine($"Số phòng: {phongs.Count}");
foreach (var p in phongs.Take(3))
    Console.WriteLine($"{p.SoPhong} | tầng {p.Tang} | {p.CheDo} | dọn dẹp: {p.CanDonDep}");


var datPhongs  = DatPhongRepository.LayTatCa(conn);
var chiTiets = ChiTietDatPhongRepository.LayTatCa(conn);
Console.WriteLine($"Đặt phòng: {datPhongs.Count}, dòng phòng: {chiTiets.Count}");

var loaiPhongs  = LoaiPhongRepository.LayTatCa(conn);
Console.WriteLine($"Số loại phòng: {loaiPhongs.Count}");
foreach (var lp in LoaiPhongRepository.LayTatCa(conn)) 
    Console.WriteLine($"{lp.Id} | {lp.TenLoai} | {lp.GiaTheoNgay} đ/đêm | {lp.GiaTheoGio}");

Console.WriteLine($"Thời gian dọn: {CauHinhRepository.ThoiGianDonPhut(conn)} phút");
foreach (var lp in LoaiPhongRepository.LayTatCa(conn))
    Console.WriteLine($"{lp.TenLoai}: {lp.GiaTheoNgay} đ/đêm, {lp.GiaTheoGio} đ/giờ");

var theoTrangThai = chiTiets.GroupBy(c => c.TrangThai()).OrderBy(g => g.Key);
foreach (var nhom in theoTrangThai)
    Console.WriteLine($"{nhom.Key}: {nhom.Count()} dòng");
{
    //10 dòng chưa nhận có 4 dòng là thuộc đã đặt, và 6 dòng thuộc đã hủy, 2 loại trạng thái mang ý nghĩa khác nhau nên là khi tô màu phải chú ý
}   
