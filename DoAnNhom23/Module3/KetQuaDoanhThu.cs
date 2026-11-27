namespace khachSan;

public sealed class KetQuaDoanhThuThang
{
    public int Thang {get;init;}
    public bool DaBatDau {get;init;}
    public long DoanhThuPhong {get;init;}
    public long DoanhThuDichVu {get;init;}
    public long CocGiuLai {get;init;}
    // doanh thu phong + dich vu, khong gom coc giu lai
    public long TongDoanhThu => DoanhThuPhong + DoanhThuDichVu;
    // tong doanh thu + coc giu lai. ko pk tien mat thuc nhan
    public long TongGiaTriGhiNhan => TongDoanhThu + CocGiuLai;
}

public sealed class KetQuaDoanhThu
{
    public int Nam { get;init; }
    public DateTime ThoiDiemBaoCao {get;init;}
    public List<KetQuaDoanhThuThang> CacThang {get;init;}
    // Dong tong nam = tong cac thang da bat dau
    public KetQuaDoanhThuThang TongNam { get; init; } = new();
    public List<CanhBao> CanhBaos { get; init; } = new();
    // true neu co ban ghi bi bo khoi phep tinh vi thieu/sai du lieu
    public bool BaoCaoKhongDayDu { get; init; }
}