namespace DoAnNhom23.Module2;

// Một dòng dịch vụ đã gọi (DV-02)
public class DongDichVu
{
    public string TenDichVu { get; set; } = string.Empty;
    public int SoLuong { get; set; }
    public long DonGia { get; set; }
    // xóa mềm (da_xoa): dòng đã xóa không tính tiền nhưng vẫn được giữ để kiểm toán (DV-05)
    public bool DaXoa { get; set; }

    public DongDichVu() { }

    public DongDichVu(string tenDichVu, int soLuong, long donGia, bool daXoa = false)
    {
        TenDichVu = tenDichVu;
        SoLuong = soLuong;
        DonGia = donGia;
        DaXoa = daXoa;
    }

    // thành tiền = so_luong x don_gia (DV-03)
    public long ThanhTien => checked((long)SoLuong * DonGia);
}

// tổng tiền dịch vụ của một dòng phòng (DV-08)
public class TinhTienDichVu
{
    // GC-8: số lượng là số nguyên từ 1-99. schema CHECK so_luong BETWEEN 1 AND 99
    public const int SoLuongToiThieu = 1;
    public const int SoLuongToiDa = 99;

    // ghi chú xóa  tối đa 200 ký tự sau khi trim
    public const int GhiChuXoaToiDa = 200;
    // số lượng dịch vụ
    public static bool SoLuongHopLe(int soLuong) => soLuong >= SoLuongToiThieu && soLuong <= SoLuongToiDa;

    public static bool GhiChuXoaHopLe(string? ghiChu)
    {
        if (ghiChu is null) return false;
        int doDai = ghiChu.Trim().Length;
        return doDai >= 1 && doDai <= GhiChuXoaToiDa;
    }

    // tổng thành tiền các dòng chưa xoá, bỏ qua dòng xoá mềm
    public static long Tinh(IEnumerable<DongDichVu> cacDong)
    {
        ArgumentNullException.ThrowIfNull(cacDong);
        long tong = 0;
        foreach (var dong in cacDong)
        {
            if (dong.DaXoa) continue;

            if (!SoLuongHopLe(dong.SoLuong))
                throw new ArgumentException(
                    $"Số lượng phải là số nguyên từ {SoLuongToiThieu} đến {SoLuongToiDa} (dịch vụ \"{dong.TenDichVu}\", số lượng {dong.SoLuong})");
            if (dong.DonGia < 0)
                throw new ArgumentException(
                    $"Đơn giá không được âm (dịch vụ \"{dong.TenDichVu}\", đơn giá {dong.DonGia})");
            tong = checked(tong + dong.ThanhTien);
        }

        return tong;
    }
}