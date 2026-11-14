namespace DoAnNhom23;
//Nguyễn Thị Hoài Lan
//một phòng trong 1 đặt phòng (dòng phòng)
public class ChiTietDatPhong
{
    public int id { get; set; }
    public int DatPhongId { get; set; }

    public LoaiPhong loaiPhong { get; set; }

    //để trống cho đến khi gán phòng cụ thể
    public int? PhongId { get; set; }

    //giá lưu lúc đặt
    public long GiaMoiDem { get; set; }
    public long GiaMoiGio { get; set; }
    public DateTime? CheckinThucTe { get; set; }

    public DateTime? CheckoutThucTe { get; set; }

    //chốt lúc checkout (nằm ở module 2), trống trước đó
    public long? TienPhong { get; set; }

    public long? TienDichVu { get; set; }

    //đã checkin
    public bool DaCheckin => CheckinThucTe.HasValue;

    //đã checkout
    public bool DaCheckout => CheckoutThucTe.HasValue;
    //đang ở: đã checkin chứ chưa checkout
    public bool DangO => DaCheckin && DaCheckout;
}

