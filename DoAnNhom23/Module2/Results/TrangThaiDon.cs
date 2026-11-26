namespace DoAnNhom23.Module2.Results;

// Trạng thái dọn của một phòng (PHONG.can_don_dep, PHONG.don_xong_luc)
public record TrangThaiDon(bool CanDonDep, DateTime? DonXongLuc);