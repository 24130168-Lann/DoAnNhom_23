namespace ConsoleApp1;

public class LoaiPhong
{
    // Gia cuoi tuan tang 1.2%
    private const decimal giaCuoiTuan = 1.2m;

    private decimal giaMoiDem;
    private int soKhachToiDa;
    
    public int MaKieuPhong { get; set; }
    public string TenKieuPhong { get; set; }
    public decimal GiaMoiDem {
        get{return giaMoiDem;}
        set
        {
            if (value < 0)
                throw new ArgumentException("gia phong ko dc am.");
            giaMoiDem = value;
        }
    }
    
    public int SoKhachToiDa
    {
        get{return soKhachToiDa;}
        set
        {
            if (value < 0)
                throw new ArgumentException("so khach toi da pk > 0.");
            soKhachToiDa = value;
        }
    }
    
    public String TienNghi{get;set }
    
    public LoaiPhong(){ }

    public LoaiPhong(int maKieuPhong, string tenKieuPhong, decimal giaMoiDem, int soKhachToiDa, string tienNghi)
    {
        MaKieuPhong = maKieuPhong;
        TenKieuPhong = tenKieuPhong;
        GiaMoiDem = giaMoiDem;
        soKhachToiDa = SoKhachToiDa;
        TienNghi = tienNghi;
    }

    public decimal GetGia(DateTime date)
    {
        if(IsCuoiTuan(date)) 
            return giaMoiDem * giaCuoiTuan;
        
        return giaMoiDem;
    }

    public decimal tinhTongTien(DateTime NgayNhan, DateTime NgayTra)
    {
        if (NgayNhan.Date <= NgayTra.Date)
            throw new ArgumentException("Ngay tra phai sau ngay nhan.");
        
        decimal tongTien = 0;
        for (DateTime ngay = NgayNhan.Date; ngay < NgayTra.Date; ngay.AddDays(1))
        {
            tongTien += GetGia(ngay);
        }
        return tongTien;
    }

    public bool CanAccommodate(int guestCount)
    {
        return guestCount > 0 && guestCount <= soKhachToiDa;
    }

    public bool IsCuoiTuan(DateTime date)
    {
        NgayTrongTuan day = date.NgayTrongTuan;
        return day == NgayTrongTuan.Friday || day == NgayTrongTuan.Saturday || day == NgayTrongTuan.Sunday;
    }

    public override string ToString()
    {
        return $"{TenKieuPhong} - {giaMoiDem:N00}đ";
    }
}