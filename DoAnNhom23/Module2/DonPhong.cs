namespace DoAnNhom23.Module2;

// Trạng thái dọn của một phòng 
public record TrangThaiDon(bool CanDonDep, DateTime? DonXongLuc);

// CO-07 / DN-01..DN-05: không dùng bộ đếm nền, tính đúng nhờ đồng bộ trước mọi thao tác liên quan phòng
public static class DonPhong
{
    // DN-05: can_don_dep = true <=> don_xong_luc có giá trị
    public static void KiemTraBatBien(bool canDonDep, DateTime? donXongLuc)
    {
        if (canDonDep && donXongLuc is null)
            throw new DuLieuKhongNhatQuanException("can_don_dep = true thì don_xong_luc phải có giá trị (DN-05).");
        if (!canDonDep && donXongLuc is not null)
            throw new DuLieuKhongNhatQuanException("can_don_dep = false thì don_xong_luc phải là NULL (DN-05).");
    }

    // DN-01: tính từ checkout_thuc_te thực tế, không phải ngay_di dự kiến
    public static TrangThaiDon BatDau(DateTime checkoutThucTe, int thoiGianDonPhut)
    {
        if (thoiGianDonPhut <= 0)
            throw new ArgumentOutOfRangeException(nameof(thoiGianDonPhut), "Thời gian dọn chuẩn phải là số nguyên dương (phút).");
        return new TrangThaiDon(true, checkoutThucTe.AddMinutes(thoiGianDonPhut));
    }

    // DN-02: hết hạn khi hiện tại >= don_xong_luc
    public static TrangThaiDon DongBo(bool canDonDep, DateTime? donXongLuc, DateTime hienTai)
    {
        KiemTraBatBien(canDonDep, donXongLuc);
        if (canDonDep && hienTai >= donXongLuc!.Value)
            return new TrangThaiDon(false, null);
        return new TrangThaiDon(canDonDep, donXongLuc);
    }

    // Phòng không còn dọn (về mặt trạng thái hệ thống) sau khi đồng bộ
    public static bool DaDonXong(bool canDonDep, DateTime? donXongLuc, DateTime hienTai)
        => !DongBo(canDonDep, donXongLuc, hienTai).CanDonDep;

    // DN-03: Nhân viên/Chủ xác nhận dọn xong sớm, chỉ khi phòng còn đang dọn (sau khi đồng bộ)
    public static TrangThaiDon XacNhanDonXongSom(VaiTro vaiTro, bool canDonDep, DateTime? donXongLuc, DateTime hienTai)
    {
        if (!PhanQuyen.ChoPhep(vaiTro, ChucNang.CO07_DonXongSom))
            throw new UnauthorizedAccessException("Chỉ Nhân viên/Chủ được xác nhận dọn xong sớm.");
        var hienTrang = DongBo(canDonDep, donXongLuc, hienTai);
        if (!hienTrang.CanDonDep)
            throw new InvalidOperationException("Phòng không ở trạng thái đang dọn.");
        return new TrangThaiDon(false, null);
    }

    // Chuỗi hiển thị cho phòng vàng, ví dụ "Dọn xong lúc 14:30"; null nếu không đang dọn
    public static string? NhanHienThi(bool canDonDep, DateTime? donXongLuc, DateTime hienTai)
    {
        var t = DongBo(canDonDep, donXongLuc, hienTai);
        return t.CanDonDep ? $"Dọn xong lúc {t.DonXongLuc:HH:mm}" : null;
    }
}
