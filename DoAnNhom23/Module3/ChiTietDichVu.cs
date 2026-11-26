namespace khachSan;

public class ChiTietDichVu
{
    public int Id { get; set; }
    public int ChiTietDichVuId { get; set; }
    public int SoLuong { get; set; }
    public long DonGia { get; set; }
    public DateTime ThoiDiem { get; set; }
    public bool DaXoa { get; set; }

    // Thanh tien = so luong * don gia
    public long ThanhTien => (long)SoLuong * DonGia;
}