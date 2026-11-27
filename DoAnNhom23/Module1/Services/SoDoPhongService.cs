using DoAnNhom23.Module1.Models;


namespace DoAnNhom23.Module1.Services;
public static class SoDoPhongService
{
	public static MauPhong TinhMau(
		Phong phong,
		KhoangThoiGian xem,
		List<ChiTietDatPhong> dongCuaPhong,
		Dictionary<long, DatPhong> datPhongTheoId,
		DateTime now
	)
	{
		//1. đỏ: có dòng đang ở (chưa checkout) chiếm từ giờ checkin trở đi
		foreach (var d in dongCuaPhong)
		{
			if (d.TrangThai() != TrangThaiDongPhong.DANG_O) continue;
			var chiem = new KhoangThoiGian(d.CheckinThucTe!.Value, DateTime.MaxValue);
			if (chiem.Giao(xem)) return MauPhong.DO;

		}
		//2. vàng: đang dọn, khoảng dọn từ giờ checkout thực tế đến dọn_xong_luc
		if (phong.CanDonDep && phong.DonXongLuc != null && phong.DonXongLuc > now)
		{
			var donXong = phong.DonXongLuc.Value;
			var batDau = dongCuaPhong
				.Where(d => d.CheckoutThucTe != null)
				.Select(d => d.CheckoutThucTe !. Value)
				.DefaultIfEmpty(now)
				.Max();
			if (batDau < donXong && new KhoangThoiGian(batDau, donXong).Giao(xem)) 
				
				return MauPhong.VANG;
		}
		//3. cam: dòng chưa nhận, đặt phòng cha cÒN Hiệu lực (DA_DAT / DANG_O), khoảng ở dự kiến giao khoảng xem
		foreach (var d in dongCuaPhong)
		{
			if (d.TrangThai() != TrangThaiDongPhong.CHUA_NHAN) continue;
			var dp = datPhongTheoId[d.DatPhongId];
			if (dp.TrangThai != TrangThaiDatPhong.DA_DAT && dp.TrangThai != TrangThaiDatPhong.DANG_O) continue;
			if (new KhoangThoiGian(dp.NgayDen, dp.NgayDi).Giao(xem)) return MauPhong.CAM;
			
		}

		return MauPhong.XANH;
	}

	public static SuatConLai TinhSuatConLai( 
		long loaiPhongId,
		KhoangThoiGian xem,
		List<Phong> phongs,
		List<DatPhong> datPhongs,
		List<ChiTietDatPhong> chiTiets,
		int thoiGianDonPhut,
		DateTime now)
	{
		var phongTheoId = phongs.ToDictionary(p => p.Id);
		var datTheoId = datPhongs.ToDictionary(d => d.Id);
		int tongNgay = phongs.Count(p => p.LoaiPhongId == loaiPhongId && p.CheDo == CheDoPhong.THEO_NGAY);
		var phongGio = phongs.Where(p => p.LoaiPhongId == loaiPhongId && p.CheDo == CheDoPhong.THEO_GIO).ToList();
		
		//quỹ theo ngày: số phòng bị giữ nhiều nhất trong khoảng xem
		var chiem = new List<KhoangThoiGian>();
		foreach (var d in chiTiets)
		{
			if (d.LoaiPhongId != loaiPhongId) continue;
			if (d.PhongId != null && phongTheoId[d.PhongId.Value].CheDo == CheDoPhong.THEO_GIO) continue;

			var k = KiemTraConPhongService.KhoangChiem(d, datTheoId[d.DatPhongId], thoiGianDonPhut);
			if (k != null) chiem.Add(k);
		}

		var moc = new List<DateTime> { xem.Tu };
		moc.AddRange(chiem.Where(k => k.Tu > xem.Tu && k.Tu < xem.Den).Select(k => k.Tu));
		int giuNhieuNhat = moc.Max(t => chiem.Count(k => k.Tu <= t && t < k.Den));
		
		//quỹ theo giờ: só phòng trống thực tế tại thời điểm xem
		int trongGio = phongGio.Count(p => KiemTraConPhongService.PhongTrongThucTe(p, chiTiets, now));
		return new SuatConLai
		{
			LoaiPhongId = loaiPhongId,
			TongTheoNgay = tongNgay,
			ConTheoNgay = Math.Max(0, tongNgay - giuNhieuNhat),
			TongTheoGio = phongGio.Count,
			ConTheoGio = trongGio
		};
	}
		
			
	
}

           
       
    
