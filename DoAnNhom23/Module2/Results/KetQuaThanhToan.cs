namespace DoAnNhom23.Module2.Results;

// Kết quả mô phỏng thanh toán; GhiNhanMoi = false nghĩa là đã xử lý trước đó (idempotent)
public record KetQuaThanhToan(KetQuaHoaDon HoaDon, bool GhiNhanMoi);