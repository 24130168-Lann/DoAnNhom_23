

namespace DoAnNhom23.Module1.Results;

public class YeuCauKiemTra
{
    public CheDoPhong HinhThucThue {get; set;}
    public DateTime ThoiDiemDen { get; set; }
    public DateTime ThoiDiemDi { get; set; }
    public List<YeuCauLoaiPhong> DanhSach { get; set; } = new();
    public long? DatPhongDangSuaId { get; set; } // dùng khi gia hạn DP-07, tạm để trống
    
}