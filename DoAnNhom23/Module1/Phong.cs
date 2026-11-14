using System.ComponentModel.Design.Serialization;
using System.Reflection.Metadata;
using Microsoft.Win32.SafeHandles;

namespace ConsoleApp1;

public class Phong
{
   private string soPhong;
   private TrangThaiPhong trangThai;
   
   public int MaPhong { get; set; }

   public string SoPhong
   {
      get { return soPhong; }
      set
      {
         if(string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("So phong ko dc de trong.");
         soPhong = value.Trim();
      }
   }
   
   public int Tang{get; set;}
   
   public int MaLoaiPhong{get; set;}
   
   public LoaiPhong Loai{get; set;}

   public TrangThaiPhong TrangThai
   {
      get
      {
         return trangThai;
      }
      private set { trangThai = value; }
   }

   public Phong()
   {
      trangThai = TrangThaiPhong.Trong;
   }

   public Phong(int maPhong, string soPhong, int tang, LoaiPhong loai)
   {
      MaPhong = maPhong;
      SoPhong = soPhong;
      Tang = tang;
      Loai = loai;
      MaLoaiPhong = loai.MaLoaiPhong;
      trangThai = TrangThaiPhong.Trong;
   }

   public bool CoTrong(DateTime ngayBD, DateTime ngayKT,
      IEnumerable<(DateTime NgayNhan, DateTime NgayTra)> cacNgayDaDat)
   {
      if (ngayKT.Date <= ngayBD.Date)
         return false;
      
      // Phong dang bao tri, ko duco dat
      if (trangThai == TrangThaiPhong.BaoTri)
         return false;
      
      foreach (var khoang in cacNgayDaDat)
      {
         if(BiTrungLich(ngayBD, ngayKT, khoang.NgayNhan, khoang.NgayTra))
            return false;
      }
      return true;
   }

   public bool ThayDoiTrangThai(TrangThaiPhong trangThaiMoi)
   {
      if(!ThayDoiTrangThai(trangThaiMoi))
         return false;
      trangThai = trangThaiMoi;
      return true;
   }

   public decimal TinhTienPhong(DateTime ngauNhan, DateTime ngayTra)
   {
      if (Loai == null)
         throw new InvalidOperationException("Phong chua co kieu phong.");
         return Loai.TinhTongTien(ngauNhan, ngayTra);
   }

   public bool CoTheChuaKhach(int soKhach)
   {
      return Loai != null && Loai.CoTheChuaKhach(soKhach);
   }

   // khach 1 thue tu 10/11 - 12/11
   // khach 2 thue tu 12-11 - 14/11
   // => khong trung nhau
   public bool BiTrungLich(DateTime ngayBD1, DateTime ngayKT, DateTime ngayBD2, DateTime ngayKT2)
   {
      return ngayBD1.Date < ngayKT2 && ngayBD2.Date < ngayBD1.Date;
   }
   
   /// <summary>
   /// quy tac chuyen trang thai hop le
   /// 1. Trong -> da dat/ dang o/ bao tri
   /// 2. Dadat -> dang o/ trong(huy dat phong)
   /// 3. Dang o -> dang don(khach tra phong)
   /// 4. Dang don -> trong/ bao tri
   /// 5. bao tri -> trong
   /// </summary>
   /// <param name="trangThai"></param>
   /// <returns></returns>
   private bool DoiTrangThaiHopLe(TrangThaiPhong trangThaiMoi)
   {
      if (trangThaiMoi == trangThai)
         return false;
      
      switch (trangThai)
      {
         case TrangThaiPhong.Trong :
            return trangThaiMoi == TrangThaiPhong.DaDat
                   || trangThaiMoi == TrangThaiPhong.DangO
                   || trangThaiMoi == TrangThaiPhong.BaoTri;
         case TrangThaiPhong.DaDat:
            return trangThaiMoi == TrangThaiPhong.DangO
                   || trangThaiMoi == TrangThaiPhong.Trong;
         case TrangThaiPhong.DangO:
            return trangThaiMoi == TrangThaiPhong.DangDon;
         case TrangThaiPhong.DangDon:
            return trangThaiMoi == TrangThaiPhong.Trong
                   || trangThaiMoi == TrangThaiPhong.BaoTri;
         case TrangThaiPhong.BaoTri:
            return trangThaiMoi == TrangThaiPhong.Trong;
         default:
            return false;
      }
   }
   
   public override string ToString()
   {
      string tenLoai = Loai != null ? Loai.TenLoaiPhong : "?";
      return $"Phong {soPhong} ({tenLoai}) - {trangThai}";
   }
}





