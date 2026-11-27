using System.Reflection.Metadata.Ecma335;

namespace khachSan;

public sealed class TinhDoanhThu
{
    private readonly DuLieuNguon Nguon;
    private readonly TinhCocGiuLai TinhCoc;

    public TinhDoanhThu(DuLieuNguon nguon, TinhCocGiuLai tinhCoc)
    {
        Nguon = nguon ?? throw new ArgumentNullException(nameof(nguon));
        TinhCoc = tinhCoc ?? throw new ArgumentNullException(nameof(tinhCoc));
    }

    public KetQuaDoanhThu Tinh(int nam)
    {
        var thoiDiemBaoCao = Nguon.ThoiDiemBaoCao;
        if (!KiemTraDauVao.NamHopLe(nam, thoiDiemBaoCao))
        {
            throw new ArgumentOutOfRangeException(nameof(nam), nam, "nam phai tu 2000 den nam hien tai.");
        }

        var canhBao = new List<CanhBao>();
        var phongTheoThang = new long[13];

        // 
        foreach (var dong in Nguon.TatCaDong())
        {
            var lyDo = KiemTraDongPhong.LyDoKhongHopLe(dong, thoiDiemBaoCao);
            if (lyDo is not null)
            {
                if (KiemTraDongPhong.LienQuanDenNam(dong, nam, thoiDiemBaoCao))
                {
                    canhBao.ThemDuyNhat(KiemTraDongPhong.TaoCanhBao(dong, lyDo));
                }

                continue;
            }

            // chua tra phong thi chua ghi nhan doanh thu
            if (!dong.DaCheckout)
            {
                continue;
            }

            var phanBo = PhanBoTienPhong(dong);
            var trongNam = phanBo.Where(x => x.Nam == nam).ToList();
            foreach (var khoan in trongNam)
            {
                phongTheoThang[khoan.Thang] += khoan.SoTien;
            }

            if (trongNam.Count > 0)
            {
                var cbPhong = KiemTraKhopTienPhong(dong, phanBo);
                if (cbPhong is not null)
                {
                    canhBao.ThemDuyNhat(cbPhong);
                }
            }

            var lienQuanDichVu = dong.CheckoutThucTe!.Value.Year == nam ||
                dong.DichVus.Any((dv => dv.ThoiDiem.Year == nam));
            if (lienQuanDichVu)
            {
                var cbDichVu = KiemTraKhopTienDichVu(dong);
                if (cbDichVu is not null)
                {
                    canhBao.ThemDuyNhat(cbDichVu);
                }
            }
        }

        //
        var cacThang = new List<KetQuaDoanhThuThang>(12);
        long tongPhong = 0, tongDichVu = 0, tongCoc = 0;

        foreach (var ky in KyThang.TaoCacThang(nam, thoiDiemBaoCao))
        {
            if (!ky.DaBatDau)
            {
                // thang tuong lai - de trong
                cacThang.Add(new KetQuaDoanhThuThang
                {
                    Thang = ky.Thang, DaBatDau = false
                });
                continue;
        }

            var phong = phongTheoThang[ky.Thang];
            var dichVu = TinhDoanhThuDichVu(ky);
            var coc = TinhCoc.TinhThang(ky, canhBao);

            cacThang.Add(new KetQuaDoanhThuThang
            {
                Thang = ky.Thang,
                DaBatDau = true,
                DoanhThuPhong = phong,
                DoanhThuDichVu = dichVu,
                CocGiuLai = coc
            });

            tongPhong += phong;
            tongDichVu += dichVu;
            tongCoc += coc;
        }

        var tongNam = new KetQuaDoanhThuThang()
        {
            Thang = 0,
            DaBatDau = true,
            DoanhThuPhong = tongPhong,
            DoanhThuDichVu = tongDichVu,
            CocGiuLai = tongCoc
        };

        var khongDayDu = canhBao.Any(c => c.Loai is LoaiCanhBao.TimestampKhongHopLe
            or LoaiCanhBao.ThieuMocThoiGianHuy
            or LoaiCanhBao.TienCocKhongHopLe
            or LoaiCanhBao.TrungNguonCoc);

        return new KetQuaDoanhThu
        {
            Nam = nam,
            ThoiDiemBaoCao = thoiDiemBaoCao,
            CacThang = cacThang,
            TongNam = tongNam,
            CanhBaos = canhBao,
            BaoCaoKhongDayDu = khongDayDu
        };
    }

        public int TinhSoDem(ChiTietDatPhong dong)
        {
            if (!dong.DaCheckout)
            {
                return 0;
            }

            var doDem = (dong.CheckoutThucTe!.Value.Date - dong.CheckinThucTe!.Value.Date).Days;
            return Math.Max(0, soDem);
        }

        /// <summary>
        /// Moi dem tinh vao thang cua ngay bat dau dem
        /// phan ngoai tien dem(phu thu, tra muon..) tinh vao thang checkout
        /// </summary>
        /// <param name="dong"></param>
        /// <returns></returns>
        public List<KhoanPhanBo> phanBoTienPhong(ChiTietDatPhong dong)
        {
            var ds = new List <KhoanPhanBo>();
            if (!dong.DaCheckout)
            {
                return ds;
            }

            var soDem = TinhSoDem(dong);
            var ngayDauTien = dong.CheckinThucTe!.Value.Date;
            for (var i = 0; i < soDem; i++)
            {
                var NgayBatDauDem = ngayDauTien.AddDays(i);
                ds.Add(new KhoanPhanBo(NgayBatDauDem.Year, NgayBatDauDem.Month, dong.GiaMoiDem, true));
            }

            var phanNgoai = TinhPhanNgoaiTienDem(dong);
            if (phanNgoai != 0)
            {
                var ra = dong.CheckoutThucTe!.Value;
                ds.Add(new KhoanPhanBo(ra.Year, ra.Month, phanNgoai, false));
            }

            return ds;
        }
        
        public long TinhPhanNgoaiTienDem(ChiTietDatPhong dong)
            => dong.TienPhong - (long)TinhSoDem(dong) * dong.GiaMoiDem;
        
        // doanh thu dich vu tinh theo thang cua thoi diem su dyng dich vu, chi voi dong da tra hop le
        public long TinhDoanhThuDichVu(KyThang ky)
        {
            long tong = 0;
            foreach (var dong in DongDaTraHopLe())
            {
                foreach (var dv in dong.DichVus)
                {
                    if (DichVuHopLe(dv) && ky.Chua(dv.ThoiDiem))
                    {
                        tong += dv.ThanhTien;
                    }
                }
                
            }

            return tong;
        }
        
        public CanhBao? KiemTraKhopTienPhong(ChiTietDatPhong dong, List<KhoanPhanBo> phanBo)
        {
            if (dong.TienPhong < 0 || dong.GiaMoiDem < 0)
            {
                return new CanhBao(LoaiCanhBao.TienPhongKhongKhop, 
                    $"dong phong #{dong.Id}: tien phong hoac gia moi dem am.", dong.Id);
            }

            var phanNgoai = TinhPhanNgoaiTienDem(dong);
            if (phanNgoai < 0)
            {
                return new CanhBao(LoaiCanhBao.TienPhongKhongKhop,
                    $"dong phong #{dong.Id}: phan tien ngoai tien am ({phanNgoai:NO}d) tien phong nhi hon so dem * gia moi dem",
                    dong.Id);
            }

            if (phanBo.Sum(x => x.SoTien) != dong.TienPhong)
            {
                return new CanhBao(LoaiCanhBao.TienPhongKhongKhop,
                    $"Dong phong #{dong.Id}: tong phan bo khong bang tien phong da chot.", dong.Id);
            }

            return null;
        }

        public CanhBao? KiemTraKhopTienDichVu(ChiTietDatPhong dong)
        {
            var dichVuSai = dong.DichVus.FirstOrDefault(dv => !dv.DaXoa && !DichVuHopLe(dv));
            if (dichVuSai is not null)
            {
                return new CanhBao(LoaiCanhBao.TienDichVuKhongKhop,
                    $"dong phong %{dong.Id}: dich vu #{dichVuSai.Id} co so luong hoac don giakhong hop le.", dong.Id);
            }

            var tongDichVu = dong.DichVus.Where(DichVuHopLe).Sum(dv => dv.ThanhTien);
            if (dong.TienDichVu < 0 || tongDichVu != dong.TienDichVu)
            {
                return new CanhBao(
                LoaiCanhBao.TienDichVuKhongKhop,
                $"dong phong #{dong.Id}: tien dich vu da chot ({dong.TienDichVu}d) khong khop tong dich vu chua xoa({tongDichVu:NO}d).", 
                dong.Id);
            }

            return null;
        }

        private IEnumerable<ChiTietDatPhong> DongDaTraHopLe()
            => Nguon.TatCaDong().Where(d =>
                d.DaCheckout && KiemTraDongPhong.LyDoKhongHopLe(d, Nguon.ThoiDiemBaoCao) is null);

        private static bool DichVuHopLe(ChiTietDatPhong dv) 
            => !dv.DaXoa && dv.SoLuong > 0 && dv.DonGia >= 0;
    }


















