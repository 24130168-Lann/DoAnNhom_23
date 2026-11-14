using System.Collections.Generic;
namespace DoAnNhom23;
public class DatPhong
{
    //một lần giữ chỗ gồm 1 hoặc là nhiều phòng
    public int id  { get; set; }
    public string MaDatPhong { get; set; } = string.Empty;
    public KhachHang KhachHang { get; set; }
    //khách vãng lai: tạo và check-in cùng lúc, không đặt cọc
    public bool VangLai { get; set; }
    //thời gian dự kiến
    public DateTime NgayDen { get; set; }
    public DateTime NgayDi { get; set; }
    
    public decimal TienCoc  { get; set; }
    public DateTime? HanThanhToanCoc  { get; set; } //chỉ có khi ChoCoc
    public DateTime GioGiuPhongDen { get; set; } //giờ giữ phòng khi khách báo đến muộn
    
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
    //thông tin hủy, chỉ có khi DaHuy
    public LyDoHuy? LyDoHuy  { get; set; }  
    public DateTime? ThoiDiemHuy  { get; set; }
    public string? GhiChuHuy  { get; set; }
    
    //tài khoản nhận hoàn cọc (mô phỏng)
    public string? TkHoanTien { get; set; }
    public string? TkHoanSo { get; set; }
    public string? TkHoanNganHang { get; set; }
    public string? NoiDungHoanCoc { get; set; }
}