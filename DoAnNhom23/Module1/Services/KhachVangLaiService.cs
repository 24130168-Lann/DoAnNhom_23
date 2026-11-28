using Microsoft.Data.Sqlite;
using DoAnNhom23.Module1.Models;
using DoAnNhom23.Module1.Repositories;
using DoAnNhom23.Module1.Results;

namespace DoAnNhom23.Module1.Services;

public static class KhachVangLaiService
{
    // Danh sách phòng để khách chọn: đúng quỹ, đúng loại, trống thực tế, không vướng đặt trước
    public static List<Phong> LietKePhongTrong(
        CheDoPhong cheDo, long loaiPhongId, DateTime now, DateTime ngayDi,
        List<Phong> phongs, List<DatPhong> datPhongs, List<ChiTietDatPhong> chiTiets, int thoiGianDon)
    {
        var datTheoId = datPhongs.ToDictionary(d => d.Id);
        var khoang = new KhoangThoiGian(now, ngayDi.AddMinutes(thoiGianDon));
        return phongs
            .Where(p => p.LoaiPhongId == loaiPhongId && p.CheDo == cheDo
                        && KiemTraConPhongService.PhongTrongThucTe(p, chiTiets, now)
                        && !chiTiets.Any(c => c.PhongId == p.Id
                            && KiemTraConPhongService.KhoangChiem(c, datTheoId[c.DatPhongId], thoiGianDon) is { } k
                            && k.Giao(khoang)))
            .ToList();
    }

    public static KetQuaThaoTac Tao(SqliteConnection conn, YeuCauVangLai yc, DateTime now)
    {
        now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second); // bỏ phần lẻ giây

        // 1. Kiểm tra dữ liệu nhập
        var loi = KiemTraDauVao(yc, now);
        if (loi != null) return KetQuaThaoTac.Loi(loi);

        // 2. Từ đây mọi thứ nằm trong một giao dịch: không commit thì tự hủy hết
        using var tx = conn.BeginTransaction();
        try
        {
            var phongs = PhongRepository.LayTatCa(conn);
            var datPhongs = DatPhongRepository.LayTatCa(conn);
            var chiTiets = ChiTietDatPhongRepository.LayTatCa(conn);
            var loaiTheoId = LoaiPhongRepository.LayTatCa(conn).ToDictionary(l => l.Id);
            int don = CauHinhRepository.ThoiGianDonPhut(conn);
            var phongTheoId = phongs.ToDictionary(p => p.Id);

            // 3. Phòng đã chọn phải tồn tại, đúng quỹ, không chọn trùng
            if (yc.PhongChons.Select(x => x.PhongId).Distinct().Count() != yc.PhongChons.Count)
                return KetQuaThaoTac.Loi("Một phòng bị chọn hai lần.");
            var chon = new List<Phong>();
            foreach (var pc in yc.PhongChons)
            {
                if (!phongTheoId.TryGetValue(pc.PhongId, out var p))
                    return KetQuaThaoTac.Loi("Phòng không tồn tại.");
                if (p.CheDo != yc.HinhThucThue)
                    return KetQuaThaoTac.Loi($"Phòng {p.SoPhong} không thuộc quỹ phòng {yc.HinhThucThue}.");
                chon.Add(p);
            }

            // 4. DP-02: đủ sức chứa theo loại (đúng loại và số lượng đã chọn)
            var kt = new YeuCauKiemTra
            {
                HinhThucThue = yc.HinhThucThue,
                ThoiDiemDen = now,
                ThoiDiemDi = yc.NgayDiDuKien,
                DanhSach = chon.GroupBy(p => p.LoaiPhongId)
                               .Select(g => new YeuCauLoaiPhong { LoaiPhongId = g.Key, SoLuong = g.Count() }).ToList()
            };
            var ketQuaKt = KiemTraConPhongService.KiemTra(kt, phongs, datPhongs, chiTiets, don, now);
            if (!ketQuaKt.DatDuoc) return KetQuaThaoTac.Loi(ketQuaKt.ThongBao);

            // 5. Từng phòng phải trống thực tế và không vướng đặt trước
            foreach (var p in chon)
            {
                var trong = LietKePhongTrong(yc.HinhThucThue, p.LoaiPhongId, now, yc.NgayDiDuKien,
                                             phongs, datPhongs, chiTiets, don);
                if (!trong.Any(x => x.Id == p.Id))
                    return KetQuaThaoTac.Loi($"Phòng {p.SoPhong} chưa sẵn sàng (đang có khách, đang dọn hoặc đã có người đặt).");
            }

            // 6. Ghi dữ liệu
            long khachId = TimHoacTaoKhach(conn, yc.NguoiDat);
            var dp = new DatPhong
            {
                MaDatPhong = "WI-" + now.ToString("yyyyMMdd-HHmmss"),
                KhachHangId = khachId,
                LaVangLai = true,
                NgayDen = now,
                NgayDi = yc.NgayDiDuKien,
                TienCoc = 0,
                TrangThai = TrangThaiDatPhong.DANG_O
            };
            long dpId = DatPhongRepository.Them(conn, dp);

            for (int i = 0; i < chon.Count; i++)
            {
                var p = chon[i];
                var lp = loaiTheoId[p.LoaiPhongId];
                long ctId = ChiTietDatPhongRepository.Them(conn, new ChiTietDatPhong
                {
                    DatPhongId = dpId,
                    LoaiPhongId = p.LoaiPhongId,
                    PhongId = p.Id,
                    GiaMoiDem = lp.GiaTheoNgay,    // giá chụp tại lúc tạo (BR-13)
                    GiaMoiGio = lp.GiaTheoGio,
                    CheckinThucTe = now
                });
                var khachO = yc.PhongChons[i].KhachO.Count > 0
                    ? yc.PhongChons[i].KhachO : new List<KhachNhap> { yc.NguoiDat };
                foreach (var k in khachO)
                    KhachOPhongRepository.Them(conn, ctId, TimHoacTaoKhach(conn, k));
            }

            tx.Commit();
            return new KetQuaThaoTac { ThanhCong = true, DatPhongId = dpId, MaDatPhong = dp.MaDatPhong,
                                       ThongBao = "Đã nhận phòng." };
        }
        catch (SqliteException e)
        {
            return KetQuaThaoTac.Loi("Không lưu được: " + e.Message);   // tx tự rollback khi thoát
        }
    }

    private static string? KiemTraDauVao(YeuCauVangLai yc, DateTime now)
    {
        if (yc.PhongChons.Count == 0) return "Chưa chọn phòng nào.";
        if (yc.NgayDiDuKien <= now) return "Thời điểm đi phải sau thời điểm hiện tại.";
        if (yc.HinhThucThue == CheDoPhong.THEO_GIO)
        {
            if (yc.PhongChons.Count != 1)
                return "Thuê theo giờ: mỗi đặt phòng chỉ một phòng; muốn thuê nhiều phòng hãy tạo đặt phòng riêng.";
            if (yc.NgayDiDuKien.Date != now.Date)
                return "Thuê theo giờ phải trả phòng trong cùng ngày (qua ngày thì gia hạn sau).";
        }
        else if (yc.NgayDiDuKien.Date <= now.Date)
            return "Ngày đi phải sau ngày đến.";

        return KiemTraDuLieu.HoTen(yc.NguoiDat.HoTen)
            ?? KiemTraDuLieu.SoDienThoai(yc.NguoiDat.SoDienThoai)
            ?? KiemTraDuLieu.GiayTo(yc.NguoiDat.LoaiGiayTo, yc.NguoiDat.SoGiayTo, batBuoc: true);
    }

    // Khách có giấy tờ thì dùng lại khách cũ nếu đã có; không thì tạo mới
    private static long TimHoacTaoKhach(SqliteConnection conn, KhachNhap k)
    {
        if (k.LoaiGiayTo != null && !string.IsNullOrWhiteSpace(k.SoGiayTo))
        {
            var cu = KhachHangRepository.TimTheoGiayTo(conn, k.LoaiGiayTo.Value, k.SoGiayTo);
            if (cu != null) return cu.Id;
        }
        return KhachHangRepository.Them(conn, new KhachHang
        {
            HoTen = k.HoTen.Trim(),
            SoDienThoai = k.SoDienThoai,
            LoaiGiayTo = k.LoaiGiayTo,
            SoGiayTo = k.SoGiayTo
        });
    }
}