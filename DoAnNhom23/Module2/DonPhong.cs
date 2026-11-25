namespace DoAnNhom23.Module2;

public class DonPhong
{
    // CAU_HINH.thoi_gian_don_phut, mặc định 30 phút 
    public const int ThoiGianDonMacDinhPhut = 30;
    
    public static bool ThoiGianDonHopLe(int thoiGianDonPhut) => thoiGianDonPhut > 0;

    // thời gian dọn phòng xong: don_xong_luc = checkout_thuc_te + thoi_gian_don_phut, tính từ giờ check-out thực tế
    public static DateTime TinhDonXongLuc(DateTime checkoutThucTe, int thoiGianDonPhut)
    {
        if (!ThoiGianDonHopLe(thoiGianDonPhut))
            throw new ArgumentOutOfRangeException(nameof(thoiGianDonPhut), "Thời gian dọn phải là số nguyên dương (phút)");
        return checkoutThucTe.AddMinutes(thoiGianDonPhut);
    }

    // trạng thái phòng: phòng từ đỏ sang vàng
    public static void BatDauDon(Phong phong, DateTime checkoutThucTe, int thoiGianDonPhut)
    {
        ArgumentNullException.ThrowIfNull(phong);
        phong.DonXongLuc = TinhDonXongLuc(checkoutThucTe, thoiGianDonPhut);
        phong.CanDonDep = true;
    }
    
    public static bool DaHetHanDon(DateTime hienTai, DateTime donXongLuc) => hienTai >= donXongLuc;

    // kiểm tra: can_don_dep=true thì don_xong_luc có giá trị; can_don_dep=false thì don_xong_luc là NULL
    public static void KiemTraBatBien(Phong phong)
    {
        ArgumentNullException.ThrowIfNull(phong);
        if (phong.CanDonDep && !phong.DonXongLuc.HasValue)
            throw new InvalidOperationException("Dữ liệu không nhất quán: đang dọn nhưng thiếu don_xong_luc");
        if (!phong.CanDonDep && phong.DonXongLuc.HasValue)
            throw new InvalidOperationException("Dữ liệu không nhất quán: không dọn nhưng còn don_xong_luc");
    }

    // Đồng bộ hết hạn dọn. Trả true nếu phòng vừa chuyển từ vàng sang xanh
    public static bool DongBo(Phong phong, DateTime hienTai)
    {
        KiemTraBatBien(phong);
        if (!phong.CanDonDep) return false;
        if (!DaHetHanDon(hienTai, phong.DonXongLuc!.Value)) return false;

        phong.CanDonDep = false;
        phong.DonXongLuc = null;
        return true;
    }

    // nút "Dọn xong sớm" chỉ có khi phòng đang dọn
    public static bool CoTheDonXongSom(Phong phong)
    {
        ArgumentNullException.ThrowIfNull(phong);
        return phong.CanDonDep;
    }

    public static void XacNhanDonXongSom(Phong phong)
    {
        if (!CoTheDonXongSom(phong))
            throw new InvalidOperationException("Phòng không ở trạng thái đang dọn");
        phong.CanDonDep = false;
        phong.DonXongLuc = null;
    }

    // phòng vàng hiển thị "Dọn xong lúc HH:mm"; phòng không dọn trả null
    public static string? NhanDangDon(Phong phong)
    {
        KiemTraBatBien(phong);
        return phong.CanDonDep ? $"Dọn xong lúc {phong.DonXongLuc!.Value:HH:mm}" : null;
    }
}