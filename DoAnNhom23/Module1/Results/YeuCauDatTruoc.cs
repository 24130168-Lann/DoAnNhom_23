


namespace DoAnNhom23.Module1.Results;

public class YeuCauDatTruoc
{
    public DateTime NgayDen { get; set; }
    public DateTime NgayDi { get; set; }
    public List<YeuCauLoaiPhong> DanhSach { get; set; } = new();
    public KhachNhap NguoiDat { get; set; } = new();
    public ThongTinHoanCoc HoanCoc { get; set; } = new();
}