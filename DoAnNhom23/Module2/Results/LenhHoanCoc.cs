namespace DoAnNhom23.Module2.Results;

// Lệnh hoàn cọc dư mô phỏng, ghi lên DAT_PHONG trước khi INSERT HOA_DON (HD-07)
public record LenhHoanCoc(
    string MaHoanCoc,
    long SoTien,
    DateTime ThoiDiem,
    string NoiDung,
    string TkHoanTen,
    string TkHoanSo,
    string TkHoanNganHang);