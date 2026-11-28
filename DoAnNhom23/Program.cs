using DoAnNhom23;
using DoAnNhom23.Data;
using DoAnNhom23.Module1.Repositories;
using DoAnNhom23.Module1.Models;
using DoAnNhom23.Module1.Results;
using DoAnNhom23.Module1.Services;


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
foreach (var lp in LoaiPhongRepository.LayTatCa(conn)) 
    Console.WriteLine($"{lp.Id} | {lp.TenLoai} | {lp.GiaTheoNgay} đ/đêm | {lp.GiaTheoGio}");

Console.WriteLine($"Thời gian dọn: {CauHinhRepository.ThoiGianDonPhut(conn)} phút");
foreach (var lp in LoaiPhongRepository.LayTatCa(conn))
    Console.WriteLine($"{lp.TenLoai}: {lp.GiaTheoNgay} đ/đêm, {lp.GiaTheoGio} đ/giờ");

var theoTrangThai = chiTiets.GroupBy(c => c.TrangThai()).OrderBy(g => g.Key);
foreach (var nhom in theoTrangThai)
    Console.WriteLine($"{nhom.Key}: {nhom.Count()} dòng");
//10 dòng chưa nhận có 4 dòng là thuộc đã đặt, và 6 dòng thuộc đã hủy, 2 loại trạng thái mang ý nghĩa khác nhau nên là khi tô màu phải chú ý


    var now = new DateTime(2026, 11, 24, 12, 0, 0);
    var xem = new KhoangThoiGian(now, new DateTime(2026, 11, 24, 23, 59, 59));
    var datTheoId = datPhongs.ToDictionary(d => d.Id);
    var mauTheoPhong =
        phongs.Select(p => (p, SoDoPhongService.TinhMau(p, xem, chiTiets.Where(c => c.PhongId == p.Id).ToList(), datTheoId, now)));
foreach (var nhom in mauTheoPhong.GroupBy(x => x.Item2).OrderBy(g => g.Key))
    Console.WriteLine(($"{nhom.Key}: {nhom.Count()} phòng"));




int don = CauHinhRepository.ThoiGianDonPhut(conn);
var den = new DateTime(2026, 11, 25, 14, 0, 0);
var di = new DateTime(2026, 11, 27, 12, 0, 0);
//loại phòng id -> số phòng còn lại tối thiểu (đã tính sẵn từ dữ liệu mẫu)
var conLai = new Dictionary<long, int> { [1] = 2, [2] = 2, [3] = 3, [4] = 3 };
foreach (var (loaiId, con) in conLai)
{
    foreach (var soLuong in new[] { con, con + 1 })
    {
        var yc = new YeuCauKiemTra
        {
            HinhThucThue = CheDoPhong.THEO_NGAY, ThoiDiemDen = den, ThoiDiemDi = di,
            DanhSach = { new YeuCauLoaiPhong { LoaiPhongId = loaiId, SoLuong = soLuong } }
        };
        var kq = KiemTraConPhongService.KiemTra(yc, phongs, datPhongs, chiTiets, don, now);
        Console.WriteLine($"Loại {loaiId}, cần {soLuong}: {(kq.DatDuoc ? "ĐẶT ĐƯỢC" : "HẾT PHÒNG")}");
        

    }
}



foreach (var lp in LoaiPhongRepository.LayTatCa(conn))
{
    var s = SoDoPhongService.TinhSuatConLai(lp.Id, xem, phongs, datPhongs, chiTiets, don, now);
    Console.WriteLine($"{lp.TenLoai}: theo ngày còn {s.ConTheoNgay}/{s.TongTheoNgay}; theo giờ còn {s.ConTheoGio}/{s.TongTheoGio}");
}
    

var ngayDi = new DateTime(2026, 11, 26, 12, 0, 0);
var trong = KhachVangLaiService.LietKePhongTrong(CheDoPhong.THEO_NGAY, 4, now, ngayDi, phongs, datPhongs, chiTiets, don);
Console.WriteLine("Phòng VIP trống: " + string.Join(", ", trong.Select(p => p.SoPhong)));

var ycv = new YeuCauVangLai
{
    HinhThucThue = CheDoPhong.THEO_NGAY,
    NgayDiDuKien = ngayDi,
    NguoiDat = new KhachNhap { HoTen = "Nguyễn Văn Test", SoDienThoai = "0911111111",
        LoaiGiayTo = LoaiGiayTo.CCCD, SoGiayTo = "079123456789" },
    PhongChons = { new PhongChon { PhongId = trong[0].Id } }
};
var kqv = KhachVangLaiService.Tao(conn, ycv, now);
Console.WriteLine(kqv.ThanhCong ? $"OK, mã: {kqv.MaDatPhong}" : "LỖI: " + kqv.ThongBao);



YeuCauDatTruoc MoiDat(string sdt, string cccd) => new()
{
    NgayDen = new DateTime(2026, 11, 30, 14, 0, 0),
    NgayDi = new DateTime(2026, 12, 1, 12, 0, 0),
    DanhSach = { new YeuCauLoaiPhong { LoaiPhongId = 4, SoLuong = 1 } },   // 1 VIP
    NguoiDat = new KhachNhap { HoTen = "Trần Test", SoDienThoai = sdt,
        LoaiGiayTo = LoaiGiayTo.CCCD, SoGiayTo = cccd },
    HoanCoc = new ThongTinHoanCoc { TenChuTaiKhoan = "TRAN TEST", SoTaiKhoan = "123456789", NganHang = "VCB" }
};

var a = DatTruocService.Tao(conn, MoiDat("0922222222", "079222222222"), now);
Console.WriteLine(a.ThanhCong ? $"A: {a.MaDatPhong}, cọc {a.TienCoc}, hạn {a.HanThanhToan}" : "A LỖI: " + a.ThongBao);
if (!a.ThanhCong) { Console.WriteLine("Dừng: A không tạo được: " + a.ThongBao); return; }var nowB = now.AddSeconds(30);
var a2 = DatTruocService.Tao(conn, MoiDat("0922222222", "079222222222"), now.AddSeconds(10));
Console.WriteLine(a2.ThanhCong ? "A2 OK (SAI!)" : "A2: " + a2.ThongBao);

var tt = DatTruocService.ThanhToanCoc(conn, a.DatPhongId!.Value, now.AddMinutes(5));
Console.WriteLine("Thanh toán A lúc +5 phút: " + tt.ThongBao);
var b = DatTruocService.Tao(conn, MoiDat("0933333333", "079333333333"), nowB);
if (!b.ThanhCong) { Console.WriteLine("Dừng: B không tạo được: " + b.ThongBao); return; }
Console.WriteLine("B: " + b.MaDatPhong);Console.WriteLine("Hết hạn lúc +11 phút, số đặt phòng bị hủy: " + DongBoService.XuLyHetHanCoc(conn, now.AddMinutes(11)));
var tt2 = DatTruocService.ThanhToanCoc(conn, b.DatPhongId!.Value, now.AddSeconds(30));

Console.WriteLine("Thanh toán B lúc +12 phút: " + tt2.ThongBao);

