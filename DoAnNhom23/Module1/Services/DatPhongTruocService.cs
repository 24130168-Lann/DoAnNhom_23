using Microsoft.Data.Sqlite;
using DoAnNhom23.Data;
using DoAnNhom23.Module1.Models;
using DoAnNhom23.Module1.Repositories;
using DoAnNhom23.Module1.Results;

namespace DoAnNhom23.Module1.Services;

public static class DatTruocService
{
    public static KetQuaThaoTac Tao(SqliteConnection conn, YeuCauDatTruoc yc, DateTime now)
    {
        now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);

        var loi = KiemTraDauVao(yc, now);
        if (loi != null) return KetQuaThaoTac.Loi(loi);

        using var tx = conn.BeginTransaction();
        try
        {
            DongBoService.XuLyHetHanCoc(conn, now);     // CP-14: dọn các đặt phòng quá hạn trước

            var phongs = PhongRepository.LayTatCa(conn);
            var datPhongs = DatPhongRepository.LayTatCa(conn);
            var chiTiets = ChiTietDatPhongRepository.LayTatCa(conn);
            var loaiTheoId = LoaiPhongRepository.LayTatCa(conn).ToDictionary(l => l.Id);
            int don = CauHinhRepository.ThoiGianDonPhut(conn);

            foreach (var y in yc.DanhSach)
                if (!loaiTheoId.ContainsKey(y.LoaiPhongId)) return KetQuaThaoTac.Loi("Loại phòng không tồn tại.");

            // CP-15
            if (DatPhongRepository.CoChoCocTheoSdt(conn, yc.NguoiDat.SoDienThoai!))
                return KetQuaThaoTac.Loi("Số điện thoại này đang có một đặt phòng chờ thanh toán cọc. Hãy hoàn tất thanh toán hoặc chờ hết hạn.");

            // DP-02
            var kt = new YeuCauKiemTra
            {
                HinhThucThue = CheDoPhong.THEO_NGAY,
                ThoiDiemDen = yc.NgayDen,
                ThoiDiemDi = yc.NgayDi,
                DanhSach = yc.DanhSach
            };
            var kq = KiemTraConPhongService.KiemTra(kt, phongs, datPhongs, chiTiets, don, now);
            if (!kq.DatDuoc) return KetQuaThaoTac.Loi(kq.ThongBao);

            // CP-02: cọc = số đêm tính cọc × tổng giá một đêm của các phòng, tính từ giá chụp
            long tongGiaDem = yc.DanhSach.Sum(y => loaiTheoId[y.LoaiPhongId].GiaTheoNgay * y.SoLuong);
            long tienCoc = CauHinhRepository.SoDemCoc(conn) * tongGiaDem;
            var han = now.AddMinutes(CauHinhRepository.ThoiHanCocPhut(conn));

            long khachId = KhachDatTruoc(conn, yc.NguoiDat);
            var dp = new DatPhong
            {
                MaDatPhong = "DP-" + now.ToString("yyyyMMdd-HHmmss"),
                KhachHangId = khachId,
                LaVangLai = false,
                NgayDen = yc.NgayDen,
                NgayDi = yc.NgayDi,
                TienCoc = tienCoc,
                HanThanhToanCoc = han,
                TrangThai = TrangThaiDatPhong.CHO_COC,
                TkHoanTen = yc.HoanCoc.TenChuTaiKhoan.Trim(),
                TkHoanSo = yc.HoanCoc.SoTaiKhoan.Trim(),
                TkHoanNganHang = yc.HoanCoc.NganHang.Trim()
            };
            long dpId = DatPhongRepository.Them(conn, dp);

            foreach (var y in yc.DanhSach)
                for (int i = 0; i < y.SoLuong; i++)
                {
                    var lp = loaiTheoId[y.LoaiPhongId];
                    ChiTietDatPhongRepository.Them(conn, new ChiTietDatPhong
                    {
                        DatPhongId = dpId,
                        LoaiPhongId = y.LoaiPhongId,
                        PhongId = null,                     // chưa gán phòng (CP-03)
                        GiaMoiDem = lp.GiaTheoNgay,
                        GiaMoiGio = lp.GiaTheoGio,
                        CheckinThucTe = null
                    });
                }

            tx.Commit();
            return new KetQuaThaoTac
            {
                ThanhCong = true, DatPhongId = dpId, MaDatPhong = dp.MaDatPhong,
                TienCoc = tienCoc, HanThanhToan = han,
                ThongBao = "Đã giữ chỗ, chờ thanh toán cọc."
            };
        }
        catch (SqliteException e)
        {
            return KetQuaThaoTac.Loi("Không lưu được: " + e.Message);
        }
    }

    // Nút "Mô phỏng: thanh toán thành công"
    public static KetQuaThaoTac ThanhToanCoc(SqliteConnection conn, long datPhongId, DateTime now)
    {
        now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
        using var tx = conn.BeginTransaction();
        try
        {
            DongBoService.XuLyHetHanCoc(conn, now);

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"UPDATE DAT_PHONG
                SET trang_thai = 'DA_DAT', thoi_diem_coc = $now, han_thanh_toan_coc = NULL
                WHERE id = $id AND trang_thai = 'CHO_COC' AND datetime(han_thanh_toan_coc) > datetime($now)";
            cmd.ThemGio("$now", now);
            cmd.Them("$id", datPhongId);
            if (cmd.ExecuteNonQuery() == 0)
                return KetQuaThaoTac.Loi("Không thanh toán được: đặt phòng đã hết hạn hoặc không còn chờ cọc.");

            tx.Commit();
            return new KetQuaThaoTac { ThanhCong = true, DatPhongId = datPhongId, ThongBao = "Thanh toán cọc thành công." };
        }
        catch (SqliteException e)
        {
            return KetQuaThaoTac.Loi("Không lưu được: " + e.Message);
        }
    }

    private static string? KiemTraDauVao(YeuCauDatTruoc yc, DateTime now)
    {
        if (yc.DanhSach.Count == 0) return "Chưa chọn loại phòng nào.";
        if (yc.DanhSach.Any(y => y.SoLuong < 1)) return "Số lượng phòng phải từ 1 trở lên.";
        if (yc.NgayDen <= now) return "Thời điểm đến phải sau thời điểm hiện tại.";
        if (yc.NgayDi <= yc.NgayDen || yc.NgayDi.Date <= yc.NgayDen.Date)
            return "Ngày đi phải sau ngày đến.";

        var h = yc.HoanCoc;
        if (string.IsNullOrWhiteSpace(h.TenChuTaiKhoan) || string.IsNullOrWhiteSpace(h.SoTaiKhoan)
            || string.IsNullOrWhiteSpace(h.NganHang))
            return "Phải nhập đủ thông tin nhận hoàn cọc (tên chủ tài khoản, số tài khoản, ngân hàng).";

        return KiemTraDuLieu.HoTen(yc.NguoiDat.HoTen)
            ?? KiemTraDuLieu.SoDienThoai(yc.NguoiDat.SoDienThoai)
            ?? KiemTraDuLieu.GiayTo(yc.NguoiDat.LoaiGiayTo, yc.NguoiDat.SoGiayTo, batBuoc: true);
    }

    private static long KhachDatTruoc(SqliteConnection conn, KhachNhap k)
    {
        var cu = KhachHangRepository.TimTheoGiayTo(conn, k.LoaiGiayTo!.Value, k.SoGiayTo!);
        if (cu != null) return cu.Id;
        return KhachHangRepository.Them(conn, new KhachHang
        {
            HoTen = k.HoTen.Trim(), SoDienThoai = k.SoDienThoai,
            LoaiGiayTo = k.LoaiGiayTo, SoGiayTo = k.SoGiayTo
        });
    }
}