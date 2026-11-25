namespace DoAnNhom23.Module2;

public enum VaiTro { Khach, NhanVien, Chu }

// CO-04 (lập hóa đơn) là tự động theo HD-01 nên không có quyền thao tác thủ công
public enum ChucNang
{
    CO01_GoiDichVu,
    CO02_XemDichVu,
    CO02_XoaDichVu,
    CO03_TraPhong,
    CO05_ThanhToan,
    CO06_XemInHoaDon,
    CO07_DonXongSom,
    CO08_HoaDonChuaThanhToan
}

public static class PhanQuyen
{
    public static bool ChoPhep(VaiTro vaiTro, ChucNang chucNang)
    {
        bool nhanVienHoacChu = vaiTro is VaiTro.NhanVien or VaiTro.Chu;
        return chucNang switch
        {
            // khách được dùng (sau khi xác thực AC-01, trên đặt phòng của mình)
            ChucNang.CO02_XemDichVu or ChucNang.CO03_TraPhong
                or ChucNang.CO05_ThanhToan or ChucNang.CO06_XemInHoaDon => true,
            // còn lại chỉ Nhân viên/Chủ
            _ => nhanVienHoacChu
        };
    }

    // Khách chỉ được thao tác trên đặt phòng của mình sau khi xác thực AC-01
    public static bool ChoPhepTrenDatPhong(VaiTro vaiTro, ChucNang chucNang, bool khachDaXacThucDungDatPhong)
        => ChoPhep(vaiTro, chucNang) && (vaiTro != VaiTro.Khach || khachDaXacThucDungDatPhong);
}