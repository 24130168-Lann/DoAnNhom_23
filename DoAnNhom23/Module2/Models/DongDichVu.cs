namespace DoAnNhom23.Module2.Models;

// Một dòng dịch vụ đã gọi, dùng để tính tiền 
public class DongDichVu
{
    public string TenDichVu { get; set; } = string.Empty;
    public int SoLuong { get; set; }
    public long DonGia { get; set; }
    // xóa mềm: dòng đã xóa không được tính tiền (DV-05)
    public bool DaXoa { get; set; }

    public DongDichVu() { }

    public DongDichVu(string tenDichVu, int soLuong, long donGia, bool daXoa = false)
    {
        TenDichVu = tenDichVu;
        SoLuong = soLuong;
        DonGia = donGia;
        DaXoa = daXoa;
    }

    public long ThanhTien => checked((long)SoLuong * DonGia);
}