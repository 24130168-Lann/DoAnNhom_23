using System.Collections.Generic;
namespace DoAnNhom23;
public class DatPhong
{
    public int id  { get; set; }
    public string MaDatPhong { get; set; }
    public KhachHang KhachHang { get; set; }
    public DateTime NgayDen { get; set; }
    public DateTime NgayDi { get; set; }
    public decimal TienCoc  { get; set; }
    public TrangThaiDatPhong TrangThai { get; set; }
    public List<ChiTietDatPhong> DanhSachChiTiet { get; set; }
    
    public DatPhong(int id, string maDatPhong, KhachHang khachHang, DateTime ngayDen, DateTime ngayDi, decimal tienCoc)
    {
        this.id = id;
        this.MaDatPhong = maDatPhong;
        this.KhachHang = khachHang;
        this.NgayDen = ngayDen;
        this.NgayDi = ngayDi;
        this.TienCoc = tienCoc;
        TrangThai = TrangThaiDatPhong.CHO_COC;
        DanhSachChiTiet = new List<ChiTietDatPhong>();
    }
}