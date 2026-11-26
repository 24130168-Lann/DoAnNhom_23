using DoAnNhom23.Module2.Services;

namespace DoAnNhom23.Module2.Results;

// Dòng dịch vụ sẽ được lưu: tên, đơn vị, đơn giá là bản chụp lúc gọi (DV-02)
public record DongDichVuDaGoi(int DichVuId, string TenLucGoi, string? DonViLucGoi, int SoLuong, long DonGia,
    DateTime ThoiDiem, int TaiKhoanId)
{
    public long ThanhTien => TinhTienDichVuService.TinhThanhTien(SoLuong, DonGia);
}