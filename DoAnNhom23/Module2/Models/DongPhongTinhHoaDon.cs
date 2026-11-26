using DoAnNhom23.Module2.Exceptions;

namespace DoAnNhom23.Module2.Models;

// chỉ đọc, dùng cho CO-04 (Module 2 không ghi huy_luc/coc_giu_lai)
// TrangThaiDongPhong dùng chung enum của Module 1 (namespace DoAnNhom23)
public record DongPhongTinhHoaDon(
    int Id,
    DateTime? CheckinThucTe,
    DateTime? CheckoutThucTe,
    DateTime? HuyLuc,
    long CocGiuLai = 0,
    long? TienPhong = null,
    long? TienDichVu = null)
{
    public TrangThaiDongPhong XacDinhTrangThai()
    {
        if (CocGiuLai < 0)
            throw new DuLieuKhongNhatQuanException($"Dòng phòng {Id}: coc_giu_lai không được âm.");

        if (HuyLuc.HasValue)
        {
            if (CheckinThucTe.HasValue || CheckoutThucTe.HasValue)
                throw new DuLieuKhongNhatQuanException($"Dòng phòng {Id}: đã có huy_luc nhưng vẫn có check-in/check-out.");
            return TrangThaiDongPhong.NO_SHOW;
        }

        if (CocGiuLai != 0)
            throw new DuLieuKhongNhatQuanException($"Dòng phòng {Id}: có coc_giu_lai nhưng không phải NO_SHOW.");

        if (!CheckinThucTe.HasValue)
        {
            if (CheckoutThucTe.HasValue)
                throw new DuLieuKhongNhatQuanException($"Dòng phòng {Id}: có check-out nhưng chưa check-in.");
            return TrangThaiDongPhong.CHUA_NHAN;
        }

        if (!CheckoutThucTe.HasValue) return TrangThaiDongPhong.DANG_O;

        if (CheckoutThucTe.Value < CheckinThucTe.Value)
            throw new DuLieuKhongNhatQuanException($"Dòng phòng {Id}: check-out trước check-in.");
        return TrangThaiDongPhong.DA_TRA;
    }
}