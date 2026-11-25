namespace DoAnNhom23.Module2;

public enum HinhThucThue
{
    TheoGio, // check-in va check-out cung ngay lich
    TheoDem  // check-out khac ngay lich
}
//ket qua tinh tien mot dong phong  
public record KetQuaTienPhong(
    long TienPhong, //tien_phong luu vao CHI_TIET_DAT_PHONG =  tien co ban + phu thu tre
    int SoDem,   // so dem theo ngay lich; 0 neu cung ngay
    int SoGio,  //so gio tinh tien khi cung ngay; 0 neu qua dem
    int SoGioTre,   // so gio tre sau gio tra chuan
    long TienDem,   //tien co ban = so dem x gia dem
    long TienGio,   // cung ngay: tien theo gio; qua dem: phu thu tre (ca hai deu da ap tran)
    string CachTinh) //mo ta cach tinh de hien tren hoa don
{
public HinhThucThue HinhThuc => SoDem == 0 ? HinhThucThue.TheoGio : HinhThucThue.TheoDem;
}

    
