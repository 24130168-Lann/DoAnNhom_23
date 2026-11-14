namespace DoAnNhom23.Module2;
//cm
public class DongDichVu
{
    public string TenDichVu { get; set; } = string.Empty;
    public int SoLuong { get; set; }
    public long DonGia { get; set; }
    //cm
    public bool DaXoa { get; set; }
    
    public DongDichVu(){}

    public DongDichVu(string tenDichVu, int soLuong, long donGia, bool daXoa = false)
    {
        TenDichVu = tenDichVu;
        SoLuong = soLuong;
        DonGia = donGia;
        DaXoa = daXoa;
    }

    public long ThanhTien => checked((long)SoLuong * DonGia);
}
//tổng tiền dịch vụ của một phòng
public class TinhTienDichVu
{
    //cm
    public static long Tinh(IEnumerable<DongDichVu> cacDong)
    {
        ArgumentNullException.ThrowIfNull(cacDong);
        long tong = 0;
        foreach (var dong in cacDong)
        {
            if (dong.DaXoa) continue;

            if (dong.SoLuong <= 0)
                throw new ArgumentException(
                    $"Số lượng phải là số nguyên dương (dịch vụ\"{dong.TenDichVu}\",số lượng{dong.SoLuong})");
            if (dong.DonGia < 0)
                throw new ArgumentException($"Đơn giá không được âm(dịch vụ\"{dong.TenDichVu}\",đơn giá{dong.DonGia})");
            tong = checked((tong + dong.ThanhTien));
        }

        return tong;

    }
    }

