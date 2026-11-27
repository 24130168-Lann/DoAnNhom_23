namespace DoAnNhom23.Module1.Models;
//Nguyễn Thị Hoài Lan
//một phòng trong 1 đặt phòng (dòng phòng)
public class ChiTietDatPhong
{
    public long Id { get; set; }
    public long DatPhongId { get; set; }

    public long LoaiPhongId { get; set; }

    //để trống cho đến khi gán phòng cụ thể
    public long? PhongId { get; set; }

    //giá lưu lúc đặt
    public long GiaMoiDem { get; set; }
    public long GiaMoiGio { get; set; }
    public DateTime? CheckinThucTe { get; set; }

    public DateTime? CheckoutThucTe { get; set; }
    public DateTime? HuyLuc { get; set; }
    public long CocGiuLai { get; set; }
    //chốt lúc checkout (nằm ở module 2), trống trước đó
    public long? TienPhong { get; set; }

    public long? TienDichVu { get; set; }

    public TrangThaiDongPhong TrangThai()
    {
        if (HuyLuc != null) return TrangThaiDongPhong.NO_SHOW;
        if (CheckinThucTe == null) return TrangThaiDongPhong.CHUA_NHAN;
        if (CheckoutThucTe == null) return TrangThaiDongPhong.DANG_O;
        return TrangThaiDongPhong.DA_TRA;
    }
    
}

