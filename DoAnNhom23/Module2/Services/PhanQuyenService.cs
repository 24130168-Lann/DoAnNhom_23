using DoAnNhom23.Module2.Enums;

namespace DoAnNhom23.Module2.Services;

public static class PhanQuyenService
{
    public static bool ChoPhep(VaiTro vaiTro, ChucNang chucNang)
    {
        bool nhanVienHoacChu = vaiTro is VaiTro.NHAN_VIEN or VaiTro.CHU;
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
        => ChoPhep(vaiTro, chucNang) && (vaiTro != VaiTro.KHACH || khachDaXacThucDungDatPhong);
}