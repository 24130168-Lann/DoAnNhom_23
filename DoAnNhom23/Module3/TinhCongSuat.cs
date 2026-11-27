using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace khachSan;

public sealed class TinhCongSuat
{
    private readonly DuLieuNguon Nguon;

    public TinhCongSuat(DuLieuNguon nguon)
    {
        Nguon = nguon ?? throw new ArgumentNullException(nameof(nguon));
    }

    public KetQuaCongSuat Tinh(int nam)
    {
        var thoiDiemBaoCao = Nguon.ThoiDiemBaoCao;
        if (!KiemTraDauVao.NamHopLe(nam, thoiDiemBaoCao))
        {
            throw new ArgumentOutOfRangeException(nameof(nam), nam, "nam phai tu 200 den nam hien.");
        }

        var canhBao = new List<CanhBao>();
        var soPhong = Nguon.Phongs.Count;
        var cacThang = new List<KetQuaCongSuatThang>(12);
        var tongGioKhach = 0d;
        var tongGioDanhNghia = 0d;

        foreach (var ky in KyThang.TaoCacThang(nam, thoiDiemBaoCao))
        {
            if (!ky.DaBatDau)
            {
                //
                cacThang.Add(new KetQuaCongSuatThang { Thang = ky.Thang, TrangThai = TrangThaiThang.ChuaBatDau });
                continue;
            }

            var gioCoKhach = TinhGioCoKhach(ky);
            var gioDanhNghia = TinhGioDanhNghia(ky);

            // 
            double? congSuat = gioDanhNghia > 0d ? gioCoKhach / gioDanhNghia * 100d : null;

            cacThang.Add(new KetQuaCongSuatThang
            {
                Thang = ky.Thang,
                TrangThai = gioDanhNghia > 0d ? TrangThaiThang.DaTinh : TrangThaiThang.ChuaCoThoiGian,
                GioPhongCoKhach = gioCoKhach,
                GioPhongDanhNghia = gioDanhNghia,
                CongSuat = congSuat,
                CongSuatHienThi = LamTronHienThi(congSuat)
            });
            tongGioKhach += gioCoKhach;
            tongGioDanhNghia += gioDanhNghia;
        }

        PhatHienChongNhau(nam, canhBao);
        var mucThamChieu = TinhMucThamChieu(canhBao);

        //
        double? congSuatNam = tongGioDanhNghia > 0d ? tongGioKhach / tongGioDanhNghia * 100 : null;

        var ketQua = new KetQuaCongSuat
        {
            Nam = nam,
            ThoiDiemBaoCao = thoiDiemBaoCao,
            CacThang = cacThang,
            MucThamChieu = mucThamChieu,
            CongSuatNam = congSuatNam,
            CongSuatNamHienThi = LamTronHienThi(congSuatNam),
            TongGioPhongCoKhach = tongGioKhach,
            TongGioPhongDanhNghia = tongGioDanhNghia,
            CanhBaos = canhBao,
            SoDongBiLoai = soDongLoai,
            KhongCoPhong = soPhong == 0
        };

        foreach (var cb in KiemTraDuLieu(ketQua))
        {
            canhBao.ThemDuyNhat(cb);
        }

        return ketQua;
    }

    //Dem cac dong phong bi loai(timestamp sai) có liên quan đến năm ssang xem, dong thoi ghi cảnh báo
    public int DemDongBiLoai(int nam, List<CanhBao> canhBao)
    {
        var thoiDiemBaoCao = Nguon.ThoiDiemBaoCao;
        var dem = 0;

        foreach (var dong in Nguon.TatCaDong())
        {
            var lyDo = KiemTraDongPhong.LyDoKhongHopLe(dong, thoiDiemBaoCao);
            if (lyDo is not null || !KiemTraDongPhong.LienQuanDenNam(dong, nam, thoiDiemBaoCao))
            { 
                continue; 
            }

            dem++;
            canhBao.ThemDuyNhat(KiemTraDongPhong.TaoCanhBao(dong, lyDo));
        }

        return dem;
    }

    // Tổng giờ phòng có khách giao với kỳ tháng.
    // Dòng không hợp lệ hoặc chưa checkin bij bỏ qua
    public double TinhGioCoKhach(KyThang ky)
    {
        var thoiDiemBaoCao = Nguon.ThoiDiemBaoCao;
        var tong = 0d;

        foreach (var dong in Nguon.TatCaDong())
        {
            if (KiemTraDongPhong.LyDoKhongHopLe(dong, thoiDiemBaoCao) is not null)
            {
                continue;
            }

            if (dong.CheckinThucTe is null)
            {
                continue;
            }

            tong += ky.GioGiao(dong.CheckinThucTe.Value, DiemKetThuc(dong));
        }
        return tong;
    }
    
    // mau so = so phong * so gio thuc te cua thang
    public double tinhGioDanhNghia(KyThang ky) => Nguon.Phongs.Count * ky.SoGio;
    
    // dong chua checkout thi tinh den thoi diem bao cao
    public DateTime DiemKetThuc(ChiTietDatPhong dong) => dong.CheckoutThucTe ?? Nguon.ThoiDiemBaoCao;
    
    public double? TinhMucThamChieu(List<CanhBao> canhBao)
    {
        var nhan = Nguon.CauHinh.GioNhanChuan;
        var tra = Nguon.CauHinh.GioTraChuan;

        if (nhan is null || tra is null || !TrongNgay(nhan.Value) || !TrongNgay(tra.Value) || nhan.Value <= tra.Value)
        {
            canhBao.ThemDuyNhat(new CanhBao(LoaiCanhBao.CauHinhGioChuanSai,
                "cau hinh gio nhan chuan / gio tra chuan thieu hoac khong hop le (gio nhan phai muon hon gio tra); khong hien thi muc tham chieu"));
            return null;
        }

        var khoangTrongGio = (nhan.Value - tra.Value).TotalHours;
        return (24 - khoangTrongGio) / 24d * 100d;
    }

    public List<CanhBao> KiemTraDuLieu(KetQuaCongSuat ketQua)
    {
        const double nguong = 100d + 1e-9;
        var ds = new List<CanhBao>();
        foreach (var thang in ketQua.CacThang)
        {
            if (thang.CongSuat is { } cs && cs > nguong)
            {
                ds.Add(new CanhBao(LoaiCanhBao.CongSuatVuot100,
                    $"thang {thang.Thang}: cong suat {cs:F1}% vuot 100%, can kiem tra khoang o chong nhau hoac danh muc phong; so lieu khong bi cat ve 1000%"));
            }
        }

        if (ketQua.CongSuatNam is { } csNam && csNam > nguong)
        {
            ds.Add(new CanhBao(LoaiCanhBao.CongSuatVuot100, $"cong suat ca nam{csNam:F1}% vuot 100%."));
        }

        return ds;
    }

    // hai dong cung phong co khoang o giao nhau
        public void PhatHienChongNhau(int nam, List<CanhBao> canhBao)
        {
            var thoiDiemBaoCao = Nguon.ThoiDiemBaoCao;
            var theoPhong = Nguon.TatCaDong()
                .Where(d => d.CheckinThucTe is not null && KiemTraDongPhong.LyDoKhongHopLe(d, thoiDiemBaoCao) is null)
                .GroupBy(d => d.PhongId);
        
            foreach (var nhom in theoPhong)
            {
                var ds = nhom.OrderBy(d => d.CheckinThucTe!.Value).ToList();
                ChiTietDatPhong? dongKetThucMuonNhat = null;
                var ketThucMuonNhat = DateTime.MinValue;

                foreach (var dong in ds)
                {
                    var vao = dong.CheckinThucTe!.Value;
                    if (dongKetThucMuonNhat is not null && vao < ketThucMuonNhat 
                    && KiemTraDongPhong.LienQuanDenNam(dong, nam, thoiDiemBaoCao))
                    {
                        canhBao.ThemDuyNhat(new CanhBao(LoaiCanhBao.KhoangOChongNhau,
                            $"phong #{nhom.Key}: dong #{dong.Id} co khoang o giao nhau voi dong #{dongKetThucMuonNhat.Id}", dong.Id));
                    }

                    var ra = DiemKetThuc(dong);
                    if (ra > ketThucMuonNhat)
                    {
                        ketThucMuonNhat = ra;
                        dongKetThucMuonNhat = dong;
                    }
                }
            }
        }
        
public static bool TrongNgay(TimeSpan gio) => gio >= TimeSpan.Zero && gio < TimeSpan.FromDays(1);

private static double? LamTronHienThi(double? giaTri)
    => giaTri.HasValue ? Math.Round(giaTri.Value, 1, MidpointRounding.AwayFromZero) : null;

}
