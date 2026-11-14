namespace ConsoleApp1;

public record KetQuaCongSuat(long GioCoKhach, long GioCoThaoCo, double CongSuatPhanTram);
public record KetQuaDoanhThu( long DoanhThuPhong, long DoanhThuDichVu)
{
public long TongDoanhThu => DoanhThuPhong + DoanhThuDichVu;
}

}