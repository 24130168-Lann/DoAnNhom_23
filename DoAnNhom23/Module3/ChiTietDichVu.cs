namespace khachSan;

public class ChiTietDichVu
{
    public int Id { get; init; }
    public int ChiTietDatPhongId { get; init; }
    public int SoLuong { get; init; }
    public long DonGia { get; init; }
    public DateTime ThoiDiem { get; init; }
    public bool DaXoa { get; init; }

    // Thanh tien = so luong * don gia
    public long ThanhTien => (long)SoLuong * DonGia;
}