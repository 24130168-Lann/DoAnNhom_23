


namespace DoAnNhom23.Module1.Results;

public class YeuCauVangLai
{
    public CheDoPhong HinhThucThue { get; set; }
    public DateTime NgayDiDuKien { get; set; }
    public KhachNhap NguoiDat { get; set; } = new();
    public List<PhongChon> PhongChons { get; set; } = new();
}