namespace khachSan;

public sealed class KetQuaCongSuatThang
{
    public int Thang { get; init; }
    public TrangThaiThang TrangThai{get;init;}
    public double GioPhongCoKhach{get;init;}
    public double GioPhongDanhNghia{get;init;}
    //Cong suat chinh xac, null neu chua bat dau hoac mau so = 0.
    //Khong bi chan o 100%
    public double? CongSuat{get;init;}
    // cong suat lam tron 1 chu so thap phan
    public double? CongSuatHienThi{get;init;}
}

public sealed class KetQuaCongSuat
{
    public int Id { get; init; }
    public DateTime ThoiDiemBaoCao{get;init;}
    public List<KetQuaCongSuatThang> CacThang { get; init; } = new();
    // muc tham chieu theo gio chuan; null neu cau hinh thieu/sai
    public double? MucThamChieu{get;init;}
    // Ty le ca nam = Tong gio co khach / tong gio danh nghia
    public double? CongSuatNam{get;init;}
    public double? CongSuatNamHienThi{get;init;}
    public double TongGioPhongCoKhach{get;init;}
    public double TongGioPhongDanhNghia{get;init;}
    public List<CanhBao> CanhBaos { get; init; } = new();
    // so dong phong bi loai khoi tinh toan vi du lieu khong hop le
    public int SoDongBiLoai{get;init;}
    // danh muc Phong rong: ko co phong de tinh, ko chia cho 0
    public bool KhongCoPhong{get;init;}
    public bool CoCanhBao => CanhBaos.Count > 0;
}