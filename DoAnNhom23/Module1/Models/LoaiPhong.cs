namespace DoAnNhom23.Module1.Models;

public class LoaiPhong
{
    public long Id  { get; set; }
    public string TenLoai { get; set; } = "";
    public long GiaTheoGio { get; set; }
    public long GiaTheoNgay  { get; set; } //giá 1 đêm
}