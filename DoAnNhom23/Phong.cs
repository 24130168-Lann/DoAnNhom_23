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
   
   public LoaiPhong LoaiPhong{get; set;}

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
      trangThai = TrangThaiPhong.Available;
   }

   public Phong(int maPhong, string soPhong, TrangThaiPhong trangThai)
   {
      MaPhong = maPhong;
      SoPhong = this.soPhong;
      Tang = tang;
      LoaiPhong = loaiPhong;
      MaLoaiPhong = LoaiPhong.MaLoaiPhong;
      trangThai = trangThaiPhong.Available;
   }

   public bool CoTrong(DateTime ngayBD, DateTime ngayKT,
      IEnumerable<(DateTime NgayNhan, DateTime NgayTra)> cacNgayDaDat)
   {
      if (ngayBD.Date <= ngayKT.Date)
         return false;
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
      if(!DoiTrangThaiPhongHopLe(trangThaiMoi))
         return false;
      trangThai = trangThaiMoi;
      return true;
   }

   public decimal TinhTienPhong(DateTime ngauNhan, DateTime ngayTra)
   {
      if (LoaiPhong == null)
         throw new InvalidOperationException("Phong chua co kieu phong.")
         return LoaiPhong.TinhTongTien(ngauNhan, ngayTra);
   }

   public bool CoTheChuaKhach(int soKhach)
   {
      return LoaiPhong != null && LoaiPhong.CoTheChuaKhach(soKhach);
   }

   private bool BiTrungLich(DateTime ngayBD1, DateTime ngayKT1, DateTime ngayBD2, DateTime ngayKT2)
   {
      return ngayKT1.Date < ngayKT1.Date && ngayKT2.Date < ngayKT2.Date;
   }
   
   private bool DoiTrangThaiHopLe(TrangThaiPhong trangThai)
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
      string type = LoaiPhong != null ? LoaiPhong.TenKieuPhong : "?";
      return $"Phong {soPhong} ({tenKieuPhong}) - {trangThai}";
   }
}





