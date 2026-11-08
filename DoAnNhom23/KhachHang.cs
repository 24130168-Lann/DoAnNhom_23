using System.Text.RegularExpressions;

namespace DoAnNhom23;

public class KhachHang
{
    public int Id { get; set; }
    public string HoTen { get; set; } = string.Empty;
    public string? SoDienThoai { get; set; }
    
    public LoaiGiayTo? LoaiGiayTo { get; set; }
    public string? SoGiayTo { get; set; }

    public ICollection<DatPhong> DatPhongs { get; set; } = new List<DatPhong>();
    public ICollection<KhachOPhong> KhachOPhongs { get; set; } = new List<KhachOPhong>();
    
    //kiem tra du lieu
    public bool HoTenHopLe() => !string.IsNullOrWhiteSpace(HoTen) && HoTen.Length <= 100;

    public bool SoDienThoaiHappen() => SoDienThoai != null && Regex.IsMatch(SoDienThoai, @"^0\d{9}$");
    
    public bool GiayToHopLe()
    {
        if (LoaiGiayTo == null || string.IsNullOrEmpty(SoGiayTo)) return false;
        return LoaiGiayTo switch
        {
            DoAnNhom23.LoaiGiayTo.Cccd => Regex.IsMatch(SoGiayTo, @"^0\d{12}$"),
            DoAnNhom23.LoaiGiayTo.HoChieu => Regex.IsMatch(SoGiayTo, @"^[A-Za-z0-9]{6,12}$"), _ => false
        };  
    }
}