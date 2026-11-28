using System.Collections.Generic;
namespace DoAnNhom23.Module1.Models;
public class DatPhong
{
    //một lần giữ chỗ gồm 1 hoặc là nhiều phòng
    public long Id  { get; set; }
    public string MaDatPhong { get; set; } = string.Empty;
    public long KhachHangId { get; set; }
    //khách vãng lai: tạo và check-in cùng lúc, không đặt cọc
    public bool LaVangLai { get; set; }
    //thời gian dự kiến
    public DateTime NgayDen { get; set; }
    public DateTime NgayDi { get; set; }
    
    public long TienCoc  { get; set; }
    public DateTime? HanThanhToanCoc  { get; set; } //chỉ có khi ChoCoc
    public DateTime? ThoiDiemCoc   { get; set; }
    public DateTime? GioGiuPhongDen { get; set; } //giờ giữ phòng khi khách báo đến muộn
    public TrangThaiDatPhong TrangThai { get; set; }
    
    //thông tin hủy, chỉ có khi DaHuy
    public LyDoHuy? LyDoHuy  { get; set; }  
    public DateTime? ThoiDiemHuy  { get; set; }
    public string? GhiChuHuy  { get; set; }
    
    //tài khoản nhận hoàn cọc (mô phỏng)
    public long TienCocHoan { get; set; }
    public string? MaHoanCoc  { get; set; }
    public DateTime? ThoiDiemHoanCoc  { get; set; }
    
    
    public string? TkHoanTen { get; set; }
    public string? TkHoanSo { get; set; }
    public string? TkHoanNganHang { get; set; }
    public string? NoiDungHoanCoc { get; set; }
}