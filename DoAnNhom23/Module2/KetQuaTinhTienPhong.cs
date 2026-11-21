namespace DoAnNhom23.Module2;

public enum HinhThucThue
{
    TheoGio,
    TheoDem
}

public record KetQuaTinhTienPhong(
    long TienPhong,
    HinhThucThue HinhThuc,
    int SoDem,
    int SoGio,
    int SoGioTre,
    long TienNghiDem,
    long TienTheoGio,
    long TienPhuThuTre,
    bool CanGiaHan,
    string MoTaCachTinh);

    
