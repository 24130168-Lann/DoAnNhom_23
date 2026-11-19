namespace khachSan;

public sealed record KetQuaDoanhThu(decimal DoanhThuPhong, decimal DoanhThuDichVu)
{
    public decimal TongDoanhThu => DoanhThuPhong + DoanhThuDichVu;
}