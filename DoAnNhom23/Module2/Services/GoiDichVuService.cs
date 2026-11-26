using DoAnNhom23.Module2.Enums;
using DoAnNhom23.Module2.Results;

namespace DoAnNhom23.Module2.Services;

// CO-01 và CO-02: kiểm tra nghiệp vụ gọi / xóa mềm dịch vụ
public static class GoiDichVuService
{
    // CO-01: tất cả dòng hợp lệ mới lưu, không lưu một phần
    public static KetQuaGoiDichVu Tao(
        VaiTro vaiTro,
        int? taiKhoanId,
        bool taiKhoanDangHoatDong,
        TrangThaiDongPhong trangThaiDongPhong,
        IReadOnlyList<YeuCauGoiDichVu> yeuCau,
        DateTime thoiDiem)
    {
        ArgumentNullException.ThrowIfNull(yeuCau);

        if (!PhanQuyenService.ChoPhep(vaiTro, ChucNang.CO01_GoiDichVu))
            throw new UnauthorizedAccessException("Chỉ Nhân viên/Chủ được ghi nhận dịch vụ; khách hãy yêu cầu nhân viên.");
        if (taiKhoanId is not int taiKhoan || !taiKhoanDangHoatDong)
            throw new UnauthorizedAccessException("Cần tài khoản Nhân viên/Chủ đang hoạt động để ghi nhận dịch vụ.");

        if (trangThaiDongPhong != TrangThaiDongPhong.DANG_O)
            throw new InvalidOperationException(trangThaiDongPhong == TrangThaiDongPhong.DA_TRA
                ? "Phòng đã trả, không thể gọi dịch vụ."
                : "Chỉ gọi dịch vụ cho phòng đang ở.");

        if (yeuCau.Count == 0)
            throw new ArgumentException("Chưa chọn dịch vụ nào.", nameof(yeuCau));

        var cacDong = new List<DongDichVuDaGoi>();
        long tong = 0;
        foreach (var y in yeuCau)
        {
            if (!y.DangBan)
                throw new InvalidOperationException($"Dịch vụ \"{y.Ten}\" đã ngừng bán.");
            TinhTienDichVuService.KiemTraSoLuong(y.SoLuong, y.Ten);
            if (y.DonGiaHienHanh < 0)
                throw new ArgumentException($"Đơn giá dịch vụ \"{y.Ten}\" không được âm.");

            var dong = new DongDichVuDaGoi(y.DichVuId, y.Ten, y.DonVi, y.SoLuong, y.DonGiaHienHanh, thoiDiem, taiKhoan);
            cacDong.Add(dong);
            tong = checked(tong + dong.ThanhTien);
        }
        return new KetQuaGoiDichVu(cacDong, tong);
    }

    // CO-02: kiểm tra được phép xóa mềm; trả về ghi chú đã chuẩn hóa (trim)
    public static string KiemTraXoa(
        VaiTro vaiTro,
        TrangThaiDongPhong trangThaiDongPhong,
        bool dongDichVuDaXoa,
        string? ghiChu)
    {
        if (!PhanQuyenService.ChoPhep(vaiTro, ChucNang.CO02_XoaDichVu))
            throw new UnauthorizedAccessException("Chỉ Nhân viên/Chủ được xóa dòng dịch vụ.");
        // DV-07: sau check-out tiền đã chốt, không xóa/sửa
        if (trangThaiDongPhong != TrangThaiDongPhong.DANG_O)
            throw new InvalidOperationException("Dòng phòng không còn ở, tiền dịch vụ đã chốt nên không được xóa (DV-07).");
        if (dongDichVuDaXoa)
            throw new InvalidOperationException("Dòng dịch vụ này đã được xóa.");
        return TinhTienDichVuService.ChuanHoaGhiChuXoa(ghiChu);
    }
}