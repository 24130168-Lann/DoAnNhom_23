using System.Text.RegularExpressions;

namespace DoAnNhom23;

public class KhachHang
{
    public long Id { get; set; }
    public string HoTen { get; set; } = "";
    public string? SoDienThoai { get; set; }
    public LoaiGiayTo? LoaiGiayTo { get; set; }
    public string? SoGiayTo { get; set; }

   
}