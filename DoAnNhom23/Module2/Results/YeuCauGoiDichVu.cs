namespace DoAnNhom23.Module2.Results;

// Yêu cầu gọi một dịch vụ (CO-01)
public record YeuCauGoiDichVu(int DichVuId, string Ten, string? DonVi, long DonGiaHienHanh, bool DangBan, int SoLuong);