namespace ConsoleApp1;

public record DongDichVu(
    DateTime ThoiDiem,
    int SoLuong,
    long DonGia,
    bool DaXoa)
{
    public long ThanhTien => (long)SoLuong * DonGia;
}