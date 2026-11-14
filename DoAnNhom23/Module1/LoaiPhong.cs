namespace ConsoleApp1;

public class LoaiPhong
{
    // Gia cuoi tuan tang 1.2% = gia goc + 20%
    private const decimal giaCuoiTuan = 1.2m;

    private decimal giaMoiDem;
    private int soKhachToiDa;
    
    public int MaLoaiPhong { get; set; }
    public string TenLoaiPhong { get; set; }
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
    
    public String TienNghi{get; set; }
    
    public LoaiPhong(){ }

    public LoaiPhong(int maLoaiPhong, string tenLoaiPhong, decimal giaMoiDem, int soKhachToiDa, string tienNghi)
    {
        MaLoaiPhong = maLoaiPhong;
        TenLoaiPhong = tenLoaiPhong;
        GiaMoiDem = giaMoiDem;
        SoKhachToiDa = soKhachToiDa;
        TienNghi = tienNghi;
    }

    public decimal GetGia(DateTime date)
    {
        if(IsCuoiTuan(date)) 
            return giaMoiDem * giaCuoiTuan;
        
        return giaMoiDem;
    }

    public decimal TinhTongTien(DateTime NgayNhan, DateTime NgayTra)
    {
        if (NgayNhan.Date <= NgayTra.Date)
            throw new ArgumentException("Ngay tra phai sau ngay nhan.");
        
        decimal tongTien = 0;
        for (DateTime ngay = NgayNhan.Date; ngay < NgayTra.Date; ngay = ngay.AddDays(1))
        {
            tongTien += GetGia(ngay);
        }
        return tongTien;
    }

    public bool CoTheChuaKhach(int soKhach)
    {
        return soKhach > 0 && soKhach <= soKhachToiDa;
    }

    public bool IsCuoiTuan(DateTime date)
    {
        DayOfWeek day = date.DayOfWeek;
        return day == DayOfWeek.Friday || day == DayOfWeek.Saturday || day == DayOfWeek.Sunday;
    }

    public override string ToString()
    {
        return $"{TenLoaiPhong} - {giaMoiDem:N00}đ";
    }
}