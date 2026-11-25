namespace DoAnNhom23.Module2;

// Yêu cầu gọi một dịch vụ
public record YeuCauGoiDichVu(int DichVuId, string Ten, string? DonVi, long DonGiaHienHanh, bool DangBan, int SoLuong);

// Dòng dịch vụ sẽ được lưu: tên, đơn vị, đơn giá là bản chụp lúc gọi (DV-02)
public record DongDichVuDaGoi(int DichVuId, string TenLucGoi, string? DonViLucGoi, int SoLuong, long DonGia,
    DateTime ThoiDiem, int TaiKhoanId)
{
    public long ThanhTien => TinhTienDichVu.TinhThanhTien(SoLuong, DonGia);
}

public record KetQuaGoiDichVu(IReadOnlyList<DongDichVuDaGoi> CacDong, long Tong);

// CO-01 và CO-02: kiểm tra nghiệp vụ gọi / xóa mềm dịch vụ
public static class GoiDichVu
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

        if (!PhanQuyen.ChoPhep(vaiTro, ChucNang.CO01_GoiDichVu))
            throw new UnauthorizedAccessException("Chỉ Nhân viên/CHủ được ghi nhận dịch vụ; khách hãy yêu cầu nhân viên.");
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
            TinhTienDichVu.KiemTraSoLuong(y.SoLuong, y.Ten);
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
        if (!PhanQuyen.ChoPhep(vaiTro, ChucNang.CO02_XoaDichVu))
            throw new UnauthorizedAccessException("Chỉ Nhân viên/Chủ được xóa dòng dịch vụ.");
        // DV-07: sau check-out tiền đã chốt, không xóa/sửa
        if (trangThaiDongPhong != TrangThaiDongPhong.DANG_O)
            throw new InvalidOperationException("Dòng phòng không còn ở, tiền dịch vụ đã chốt nên không được xóa (DV-07).");
        if (dongDichVuDaXoa)
            throw new InvalidOperationException("Dòng dịch vụ này đã được xóa.");
        return TinhTienDichVu.ChuanHoaGhiChuXoa(ghiChu);
    }
}
