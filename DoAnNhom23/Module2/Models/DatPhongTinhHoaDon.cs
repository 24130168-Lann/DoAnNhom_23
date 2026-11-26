namespace DoAnNhom23.Module2.Models;

// Chỉ đọc, đủ để quyết định và tính hóa đơn
public record DatPhongTinhHoaDon(
    string MaDatPhong,
    string TenNguoiDat,
    TrangThaiDatPhong TrangThai,
    bool VangLai,
    long TienCoc,
    bool DaCoHoaDon,
    IReadOnlyList<DongPhongTinhHoaDon> CacDong,
    string? TkHoanTen = null,
    string? TkHoanSo = null,
    string? TkHoanNganHang = null);