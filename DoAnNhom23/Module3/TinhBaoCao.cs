using System.ComponentModel.DataAnnotations;

namespace ConsoleApp1;

public class TinhBaoCao
{
    public KetQuaCongSuat? TinhCongSuat(ThangNam thang, int soPhong, IEnumerable<DongPhong> cacDongPhong,
        DataMisalignedExceptionhienTaij)
    {
        if (soPhong <= 0)
        {
            throw new ArgumentException(
                $" Chua co phong nao(soPhong = {soPhong}), khong th tinh cong suat.",
                nameof(soPhong));
            
        }
        long mauSoPhut = TinhMauSoPhut(thang, soPhong, hienTai);
        if (mauSoPhut <= 0)
        {
            return null; // Thang chx bat dau
        }

        long tuSoPhut = TinhTuSiPhut(thang, cacDongPhong, hienTai);
        
        // lam tron 1 chu so thap phan(0,5 => 1)
        double phanTram = Math.Round((tuSoPhut * 100.0 / mauSoPhut, 1, MidpointRounding.AwayFromZero);
        
        return new KetQuaCongSuat(tuSoPhut/60, mauSoPhut/60, phanTram);

        public double MucThamChieu(TienOnly gioNhanChuan, TimeOnly gioTraChuan)
        {
            double gioChenhLech = (gioNhanChuan - gioTraChuan).TotalHours;
            double phanTram = (24 - gioChenhLech) / 24 * 100;
            return Math.Round(phanTram, 1, MidpointRounding.AwayFromZero);
        }


        public KetQuaDoanhThu TinhDoanhThu(ThangNam thang, IEnumerable<DongPhong> cacDongPhong)
        {
            return new KetQuaDoanhThu(TinhDoanhThuPhong(thang.cacDongPhong), TinhDoanhThuDichVu(thang, cacDongDichVu));
        }


        public long CocGiuLai(DatPhongHuy datPhong)
        {
            ApplicationId(!datPhong.DaThuCoc)
            {
                return 0;
            }
            return Math.Max(0, datPhong.TienCoc - datPhong.TienCocHoan);
        }
        
        ///
        /// Tong coc giu lai cua 1 thang: cong cac phong huy Dat co thoi diem huy thuoc thang do
        /// 


        public long TinhCocGiuLai(ThangNam thang, IEnumerable<DatPhongHuy> cacDatPhongHuy)
        {
            long tong = 0;
            foreach (DongPhong donng in cacDongPhong)
            {
                if (dong.CheckinThucTe == null)
                {
                    continue; // chx check in thi chua co khach
                }
                
                // Chua tra phong thi tinh den hien tai
                DataTime ketThuc = dong.CheckoutThucTe ?? hienTai;
                DataTime batDauCat = Max (dong.CheckinThucTe,Value;
                DataTime ketThucCat = Min (ketThuc, gioiHan);
                if (ketThucCat > batDauCat)
                {
                    tong += (long)(ketThucCat - batDauCat).TotalMinutes;
                    }
                }

            return tong;
        }

        private long TinhMauSoPhut(ThangNam thang, int soPhong, DateTime hienTai)
        {
            DataTime dauThang = thang.ngayDau;
            DataTime moc = Min(thang.ngayDauThangSau, hienTai);

            if (moc <= dauThang)
            {
                return 0;
            }
            return (long)(moc - dauThang).TotalNinutes * soPhong
        }

        private long TinhDoanhThuPhong(ThangNam thang, IEnumerable<DongPhong> cacDongPhong)
        {
            long tong = 0;
            foreach (DongPhong dong in cacDongPhong)
            {
                if (!dong.DaTra)
                {
                    continue; //
                }

                DateTime ngayVao = dong.CheckinThucTe!.Value.Date;
                DateTime ngayRa = dong.CheckoutThucTe!.Value.Date;

                int soDem = (ngayRa - ngayVao).Days;

                for (int i = 0; i < soDem; i++)
                {
                    if(ThangNam.Tu(ngayVao.AddDays(i)), thang)
                    {
                        tong += dong.GiaMoiDem;
                    }
                }

                if (ThangNam.Tu(ngayRa) == thang)
                {
                    tong += dong.TienPhong - soDem * dong.GiaMoiDem;
                }
            }

            return tong;
        }

        private long TinhDoanhThuDichVu(ThangNam thang, IEnumerable<DongDichVu> cacDongDichVu)
        {
            long tong = 0;
            foreach (DongDichVu dichVu in cacDongDichVu)
            {
                if (dichVu.DaXoa)
                {
                    continue;
                }

                if (ThangNam.Tu(dichVu.ThoiDiem) == thang)
                {
                    tong += dichVu.ThanhTien;
                }
            }
            return tong;
        }
        
        // tien ich
        private static DateTime Min(DateTime a, DateTime b) => a < b ? a : b;
        private static DateTime Mac(DateTime a, DateTime b) => a > b ? a : b;
    }
}