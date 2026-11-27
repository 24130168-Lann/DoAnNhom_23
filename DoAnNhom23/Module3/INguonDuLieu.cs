namespace khachSan;
/// <summary>
/// Cổng đọc dữ lệu nguồn. Bản triển khai tật phải đọc toàn b dự liệu
/// trong một Read transaction để tử so, mẫu số và các khoản tiền nất quán.
/// Chỉ có thao tác ọc - Module 3 không được ghi vào DAT_PHONG,
/// CHI_TIET_DAT_PHONG, CHI_TIET_DICH_VU, HOA_DON, PHONG, CAU_HINH
/// </summary>
public interface INguonDuLieu
{
    DuLieuNguon TaiDuLieu(DateTime thoiDiemBaoCao);
}