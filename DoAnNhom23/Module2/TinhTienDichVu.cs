namespace DoAnNhom23.Module2;

// một dòng dịch vụ đã gọi dùng để tính tiền
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

// tổng tiền dịch vụ của một phòng (DV-03, DV-08)
public class TinhTienDichVu
{
    public const int SoLuongToiThieu = 1;
    public const int SoLuongToiDa = 99;   // GC-8
    public const int GhiChuXoaToiDa = 200;

    public static long Tinh(IEnumerable<DongDichVu> cacDong)
    {
        ArgumentNullException.ThrowIfNull(cacDong);
        long tong = 0;
        foreach (var dong in cacDong)
        {
            if (dong.DaXoa) continue;

            KiemTraSoLuong(dong.SoLuong, dong.TenDichVu);
            if (dong.DonGia < 0)
                throw new ArgumentException($"Đơn giá không được âm (dịch vụ \"{dong.TenDichVu}\", đơn giá {dong.DonGia})");
            tong = checked(tong + dong.ThanhTien);
        }

        return tong;
    }

    // số lượng là số nguyên từ 1 đến 99 (GC-8)
    public static void KiemTraSoLuong(int soLuong, string tenDichVu = "")
    {
        if (soLuong < SoLuongToiThieu || soLuong > SoLuongToiDa)
            throw new ArgumentException(
                $"Số lượng phải là số nguyên từ {SoLuongToiThieu} đến {SoLuongToiDa} (dịch vụ \"{tenDichVu}\", số lượng {soLuong})");
    }

    public static long TinhThanhTien(int soLuong, long donGia) => checked((long)soLuong * donGia);

    // ghi chú xóa bắt buộc, không rỗng, tối đa 200 ký tự sau khi trim 
    public static string ChuanHoaGhiChuXoa(string? ghiChu)
    {
        var sach = ghiChu?.Trim() ?? string.Empty;
        if (sach.Length == 0)
            throw new ArgumentException("Ghi chú xóa là bắt buộc", nameof(ghiChu));
        if (sach.Length > GhiChuXoaToiDa)
            throw new ArgumentException($"Ghi chú xóa tối đa {GhiChuXoaToiDa} ký tự", nameof(ghiChu));
        return sach;
    }
}
