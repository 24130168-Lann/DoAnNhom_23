using DoAnNhom23.Module1.Models;


namespace DoAnNhom23.Module1.Services;

public static class KiemTraConPhongService
{
    public static KetQuaKiemTra KiemTra(
        YeuCauKiemTra yc,
        List<Phong> phongs,
        List<DatPhong> datPhongs,
        List<ChiTietDatPhong> chiTiets,
        int thoiGianDonPhut,
        DateTime now
    )
    {
        //kiểm tra dữ liệu nhập
        if (yc.DanhSach.Count == 0)
            return Loi("Chưa chọn loại phòng nào.");
        if (yc.ThoiDiemDi <= yc.ThoiDiemDen)
            return Loi("Thời điểm đi phải sau thời điểm đến.");
        if (yc.DanhSach.Any(y => y.SoLuong < 1))
            return Loi("Số lượng phòng phải từ 1 trở lên.");

        return yc.HinhThucThue == CheDoPhong.THEO_NGAY
            ? KiemTraTheoNgay(yc, phongs, datPhongs, chiTiets, thoiGianDonPhut)
            : KiemTraTheoGio(yc, phongs, chiTiets, now);


    }
    //theo ngày: đếm sức chứa theo khoảng thời gian(BR-01)
    private static KetQuaKiemTra KiemTraTheoNgay(
        YeuCauKiemTra yc, List<Phong> phongs, List<DatPhong> datPhongs,
        List<ChiTietDatPhong> chiTiets, int thoiGianDonPhut)
    {
        var datTheoId = datPhongs.ToDictionary(d => d.Id);
        var phongTheoId = phongs.ToDictionary(p => p.Id);
        //khoảng yêu cầu: [dến, đi + thời gian dọn)
        var khoangYeuCau = new KhoangThoiGian(yc.ThoiDiemDen, yc.ThoiDiemDi.AddMinutes(thoiGianDonPhut));
        foreach (var y in yc.DanhSach)
        {
            int tong = phongs.Count(p => p.LoaiPhongId == y.LoaiPhongId && p.CheDo == CheDoPhong.THEO_NGAY);
            
            //các khoảng đang chiếm của loại này (chỉ quỹ THEO_NGAY)
            var chiem = new List<KhoangThoiGian>();
            foreach (var d in chiTiets)
            {
                if (d.LoaiPhongId != y.LoaiPhongId) continue;
                if (yc.DatPhongDangSuaId == d.DatPhongId) continue;
                if (d.PhongId != null && phongTheoId[d.PhongId.Value].CheDo == CheDoPhong.THEO_GIO) continue;

                var k = KhoangChiem(d, datTheoId[d.DatPhongId], thoiGianDonPhut);
                if (k !=  null) chiem.Add(k);
            }
            
            //số lượng chiếm lớn nhất chỉ xảy ra tại đầu khoảng yêu cầu hoặc tại thời điểm bắt đầu một khoảng có sẵn
            var moc = new List<DateTime> { khoangYeuCau.Tu };
            moc.AddRange(chiem.Where(k => k.Tu > khoangYeuCau.Tu && k.Tu < khoangYeuCau.Den).Select(k => k.Tu));
            foreach (var t in moc.OrderBy(x => x))
            {
                int dangChiem = chiem.Count(k => k.Tu <= t && t < k.Den);
                if (dangChiem + y.SoLuong > tong)
                    return Het(y.LoaiPhongId, t,
                        $"Không đủ phòng (loại {y.LoaiPhongId} tại thời điểm {t:dd/MM/yyyy HH:mm}: đã giữ {dangChiem}/{tong}, cần thêm {y.SoLuong}");
                
            }
        }

        return new KetQuaKiemTra { DatDuoc = true };
    }
    
    //theo giờ: phòng theo giờ trong thực tế tại thời điểm nhận

    private static KetQuaKiemTra KiemTraTheoGio(
        YeuCauKiemTra yc,
        List<Phong> phongs,
        List<ChiTietDatPhong> chiTiets, DateTime now)
    {
        foreach (var y in yc.DanhSach)
        {
            int trong = phongs.Count(p => p.LoaiPhongId == y.LoaiPhongId  && p.CheDo == CheDoPhong.THEO_GIO && PhongTrongThucTe(p, chiTiets, now));
            if (trong < y.SoLuong)
                return Het(y.LoaiPhongId, now,
                    $"Không đủ phòng theo giờ (loại {y.LoaiPhongId}): còn {trong}, cần {y.SoLuong}.");
            
        }

        return new KetQuaKiemTra { DatDuoc = true };
        
    }
    //phòng trống thực tế: không có khách chưa checkout và không đang dọn (DP04, DP06 sẽ dùng lại)
    public static bool PhongTrongThucTe(Phong p, List<ChiTietDatPhong> chiTiets, DateTime now)
    {
        bool coKhach = chiTiets.Any(c => c.PhongId == p.Id && c.TrangThai() == TrangThaiDongPhong.DANG_O);
        bool dangDon = p.CanDonDep && p.DonXongLuc != null && p.DonXongLuc > now;
        return !coKhach && !dangDon;
    }

    public static KhoangThoiGian? KhoangChiem(ChiTietDatPhong dong, DatPhong datPhong, int thoiGianDonPhut)
    {
        switch (dong.TrangThai())
        {
            case TrangThaiDongPhong.NO_SHOW: return null;
            case TrangThaiDongPhong.DANG_O: return new KhoangThoiGian(dong.CheckinThucTe!.Value, DateTime.MaxValue);
            case TrangThaiDongPhong.DA_TRA:
                return new KhoangThoiGian(dong.CheckinThucTe!.Value,
                    dong.CheckoutThucTe!.Value.AddMinutes(thoiGianDonPhut));
            default: //CHUA_NHAN: chỉ chiếm khi đặt phòng cha còn hiệu lực
                if (datPhong.TrangThai == TrangThaiDatPhong.CHO_COC || datPhong.TrangThai == TrangThaiDatPhong.DA_DAT ||
                    datPhong.TrangThai == TrangThaiDatPhong.DANG_O)
                    return new KhoangThoiGian(datPhong.NgayDen, datPhong.NgayDi.AddMinutes(thoiGianDonPhut));
                return null;
        }
    }

    private static KetQuaKiemTra Loi(string msg) => new() { DatDuoc = false, ThongBao = msg };

    private static KetQuaKiemTra Het(long loaiId, DateTime t, string msg) => new()
        { DatDuoc = false, LoaiPhongHetId = loaiId, ThoiDiemHetPhong = t, ThongBao = msg };


}


    
        

    