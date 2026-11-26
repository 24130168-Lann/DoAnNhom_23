namespace DoAnNhom23.Module2.Enums;

public enum LyDoChuaLapHoaDon
{
    KhongCo,                   // đủ điều kiện
    DaCoHoaDon,                // đã có hóa đơn: trả hóa đơn hiện có, không lập thêm
    DatPhongKhongDangO,
    KhongCoDongNaoCheckin,     // thuộc nhánh DA_HUY của Module 1, không lập hóa đơn
    CoDongChuaKetThuc          // còn dòng DANG_O hoặc CHUA_NHAN chưa được Module 1 xử lý no-show
}