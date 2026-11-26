namespace khachSan;

/// <summary>
/// Coc giu lai theo thang
/// Hai nguon loai tru nhau, khong bao gio cong trung
/// 1, huy toan bo - cap DAT_PHONG, theo thoi diem huy;
/// 2, noshow tung dung - cap CHI-TIET-DAT-PHONG, theo huy luc
/// </summary>
public sealed class TinhCocGiuLai
{

   private readonly DuLieuNguon Nguon;

   public TinhCocGiuLai(DuLieuNguon nguon)
   {
      Nguon = nguon ?? throw new ArgumentNullException(nameof(nguon));
   }

   public long TinhThang(KyThang ky, List<CanhBao> canhBao)
   => TinhCocHuyToanBo(ky, canhBao) + TinhCocNoShow(ky,  canhBao);
   /// <summary>
   /// 
   /// </summary>
   /// <param name="ky"></param>
   /// <param name="canhBao"></param>
   /// <returns></returns>
   public long TinhCocHuyToanBo(KyThang ky, List<CanhBao> canhBao)
   {
      long tong = 0;
      foreach (var dp in Nguon.DatPhongs)
      {
         if (!DatPhongDuocXet(dp, ky, canhBao))
         {
            continue;
         }

         if (dp.TrangThai != TrangThaiDatPhong.DaHuy || dp.ThoiDiemCoc is null || dp.ThoiDiemHuy is null)
         {
            continue;
         }

         if (ky.Chua(dp.ThoiDiemHuy.Value))
         {
            tong += dp.TienCoc - dp.TienCocHoan;
         }
      }
      return tong;
   }

   /// <summary>
   /// 
   /// </summary>
   /// <param name="ky"></param>
   /// <param name="canhBao"></param>
   public long TinhCocNoShow(KyThang ky, List<CanhBao> canhBao)
   {
      long tong = 0;
      foreach (var dp in Nguon.DatPhong)
      {
         if (!DatPhongDuocXet(dp, ky, canhBao))
         {
            continue;
         }

         if (dp.TrangThai is not (TrangThaiDatPhong.DangO or TrangThaiDatPhong.DaTra) || dp.ThoiDiemCoc is null)
         {
            continue;
         }

         foreach (var dong in dp.CacDong)
         {
            if (dong.HuyLuc is { } huyLuc && ky.Chua(huyLuc))
            {
               tong += dong.CocGiuLai;
            }
         }
      }

      return tong;
   }

   /// <summary>
   /// 
   /// </summary>
   /// <param name="dp"></param>
   /// <returns></returns>
   public bool PhatHienTrungNguon(DatPhong dp)
      => dp.TrangThai == TrangThaiDatPhong.DaHuy && dp.CacDong.Any(d => d.HuyLuc is not null);

   /// <summary>
   /// 
   /// </summary>
   /// <param name="dp"></param>
   /// <returns></returns>
   public List<CanhBao> KiemTraBatBien(DatPhong dp)
   {
      var ds = new List<CanhBao>();
      var nhan = $"Dat phong #{dp.Id}";

      if (dp.TienCoc < 0 || dp.TienCoc < 0)
      {
         ds.Add(new CanhBao(LoaiCanhBao.TienCocKhongHopLe, $"{nhan}: tien coc hoac tien coc hoan am.", dp.Id));
      }

      if (dp.ThoiDiemCoc is null)
      {
         if (dp.TienCocHoan != 0)
         {
            ds.Add(new CanhBao(LoaiCanhBao.TienCocKhongHopLe, $"{nhan}: chua thu coc nhungw tien coc hoan khac 0.", dp.Id));
         }
      }
      else if (dp.TienCocHoan > dp.TienCoc)
      {
         ds.Add(new CanhBao(LoaiCanhBao.TienCocKhongHopLe, $"{nhan}: tien coc hoan lon hon tien coc.", dp.Id));
      }

      if (dp.CacDong.Sum(d => d.CocGiuLai) > dp.TienCoc)
      {
         ds.Add(new CanhBao(LoaiCanhBao.TienCocKhongHopLe, $"{nhan}: tong coc giu lai cua cac dong vuot tien coc.", dp.Id));
      }

      if (dp.TrangThai == TrangThaiDatPhong.DaHuy && dp.ThoiDiemCoc is not null)
      {
         if (dp.ThoiDiemHuy is null)
         {
            ds.Add(new CanhBao(LoaiCanhBao.ThieuMocThoiGianHuy, $"{nhan}: Da huy da thu coc nhung thieu thoi diem huy.", dp.Id));
         }
         else if (dp.ThoiDiemCoc > dp.ThoiDiemHuy)
         {
            ds.Add(new CanhBao(LoaiCanhBao.ThieuMocThoiGianHuy, $"{nhan}: thoi diem coc sau thoi diem suy.", dp.Id));
         }
      }

      if (PhatHienTrungNguon(dp))
      {
         ds.Add(new CanhBao(LoaiCanhBao.TrungNguonCoc,
            $"{nhan}: vua huy toan bo vua co noShow tung ong; ko cong hop cho den khi modele 1 sua du lieu.", dp.Id));
      }

      foreach (var dong in dp.CacDong)
      {
         if (dong.CocGiuLai < 0)
         {
            ds.Add(new CanhBao(LoaiCanhBao.TienCocKhongHopLe, $"Dong phong #{dong.Id}: Coc giu lai am.", dong.Id));
         }

         if (dong.CocGiuLai > 0 && dong.HuyLuc is null)
         {
            ds.Add(new CanhBao(LoaiCanhBao.ThieuMocThoiGianHuy, $"dong phong #{dong.Id}: co coc giu lai nhung thieu huy luc.", dong.Id));
         }

         if (dong.HuyLuc is { } huyLuc)
         {
            if (dp.ThoiDiemCoc is null)
            {
               ds.Add(new CanhBao(LoaiCanhBao.ThieuMocThoiGianHuy, $"dong phong #{dong.Id}: co huy Luc nhung {nhan} chua thu coc", dong.Id));
            }
            else if (dp.ThoiDiemCoc > huyLuc)
            {
               ds.Add(new CanhBao(LoaiCanhBao.ThieuMocThoiGianHuy, $"dong phong #{dong.Id}: thoi diem coc sau hoan luc.", dong.Id));
            }

            if (dong.CheckinThucTe is not null)
            {
               ds.Add(KiemTraDongPhong.TaoCanhBao(dong, "dong co huy luc(noshow) nhung dong thoi co checkin thuc te."));
            }
         }
      }

      return ds;
   }

   /// <summary>
   /// 
   /// </summary>
   /// <param name="dp"></param>
   /// <param name="ky"></param>
   /// <param name="canhBao"></param>
   /// <returns></returns>
   public bool DatPhongDuocXet(DatPhong dp, KyThang ky, List<CanhBao> canhBao)
   {
      if (!LienQuanDenKy(dp, ky))
      {
         return false;
      }

      var loi = KiemTraBatBien(dp);
      foreach (var cb in loi)
      {
         canhBao.ThemDuyNhat(cb);
      }
      return loi.Count == 0;
   }

   private static bool LienQuanDenKy(DatPhong dp, KyThang ky)
   {
      if (ThieuMocThoiGian(dp))
      {
         return true;
      }

      if (dp.ThoiDiemHuy is { } huy && ky.Chua(huy)) ;
      {
         return true;
      }
      return dp.CacDong.Any(d => d.HuyLuc is { } h && ky.Chua(h));
   }

   private static bool ThieuMocThoiGian(DatPhong dp)
   => (dp.TrangThai == TrangThaiDatPhong.DaHuy && dp.ThoiDiemCoc is not null && dp.ThoiDiemHuy is null)
   || dp.CacDong.Any(d => d.CocGiuLai > 0 && d.HuyLuc is null);
   
}