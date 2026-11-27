namespace khachSan;

/// <summary>
/// anh chup du lieu nguon dung cho 1 lan tinh bao cao
/// va du lieu doc trong cung mot read transaction
/// Chi doc module3 ko ghi vao bat ki bang nao
/// </summary>
public sealed class DuLieuNguon
{
    public DuLieuNguon(DateTime thoiDiemBaoCao, IEnumerable<Phong> phongs, 
        IEnumerable<DatPhong> datPhongs, CauHinh? cauHinh)
    {
        ArgumentNullException.ThrowIfNull(phongs);
        ArgumentNullException.ThrowIfNull(datPhongs);
        ThoiDiemBaoCao = thoiDiemBaoCao;
        Phongs = phongs.ToList().AsReadOnly();
        DatPhongs = datPhongs.ToList().AsReadOnly();
        CauHinh = cauHinh ?? new CauHinh();

    }

    public DateTime ThoiDiemBaoCao { get; }
    public IReadOnlyList<Phong> Phongs { get; }
    public IReadOnlyList<DatPhong> DatPhongs { get; }
    public CauHinh CauHinh { get; }
    
    //
    public IEnumerable<ChiTietDatPhong> TatCaDong() => DatPhongs.SelectMany(d => d.CacDong);
}
