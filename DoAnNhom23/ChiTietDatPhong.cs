namespace DoAnNhom23;
//Nguyễn Thị Hoài Lan
//Chi tie
public class ChiTietDatPhong
{
    public int Id { get; set; }
    public LoaiPhong LoaiPhong { get; set; }
    public Phong? Phong  { get; set; }
    public decimal  GiaMoiDem { get; set; }

    public ChiTietDatPhong(int Id, loaiPhong loaiPhong, decimal giaMoiDem)
    {
        this.Id = Id;
        this.LoaiPhong = loaiPhong;
        this.GiaMoiDem = giaMoiDem;
        Phong = null;
    }
    
}