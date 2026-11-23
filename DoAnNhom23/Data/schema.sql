CREATE TABLE IF NOT EXISTS LOAI_PHONG(
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    ten_loai TEXT NOT NULL UNIQUE,
    gia_theo_gio INTEGER NOT NULL CHECK (gia_theo_gio >= 0),
    gia_theo_ngay INTEGER NOT NULL CHECK (gia_theo_ngay >= 0)
);
CREATE TABLE IF NOT EXISTS PHONG (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    loai_phong_id INTEGER NOT NULL,
    so_phong TEXT NOT NULL UNIQUE,
    tang INTEGẺ NOT NULL,
    che_do TEXT NOT NULL CHECK (
        che_do IN ('THEO_NGAY', 'THEO_GIO')
),
    can_don_dep INTEGER NOT NULL DEFAULT 0 CHECK (
        can_don_dep IN (0, 1)
                ),
    don_xong_luc TEXT,
    FOREIGN KEY (loai_phong_id)
                REFERENCES LOAI_PHONG (id)
);
CREATE TABLE IF NOT EXISTS KHACH_HANG (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    ho_ten TEXT NOT NULL,
    so_dien_thoai TEXT,
    loai_giay_to TEXT,
    so_giay_to TEXT                                    
);
CREATE TABLE IF NOT EXISTS DAT_PHONG (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    ma_dat_phong TEXT NOT NULL UNIQUE,
    khach_hang_id INTEGER NOT NULL,
    la_vang_lai INTEGER NOT NULL DEFAULT 0 CHECK (la_vang_lai IN(0, 1)),
    ngay_den TEXT NOT NULL,
    ngay_di TEXT NOT NULL,
    tien_coc INTEGER NOT NULL DEFAULT 0 
                CHECK (tien_coc >= 0),
    han_thanh_toan_coc TEXT,
    thoi_diem_coc TEXT,
    gio_giu_phong_den TEXT,
    trang_thai TEXT NOT NULL 
                    CHECK (
                        trang_thai IN (
                            'CHO_COC',
                             'DA_DAT',
                              'DANG_O',
                              'DA_TRA',
                               'DA_HUY'
                                      )
                          ),
    ly_do_huy TEXT CHECK (
            ly_do_huy IS NULL OR
            ly_do_huy IN (
                'KHACH_HUY',
                'KHACH_SAN_HUY',
                'BAT_KHA_KHANG',
                'NO_SHOW',
                'HET_HAN_COC'
                         )
                ),
    thoi_diem_huy TEXT,
    ghi_chu_huy TEXT,
    
    tien_coc_hoan INTEGER,
    
    tk_hoan_ten TEXT,
    tk_hoan_so TEXT,
    tk_hoan_ngan_hang TEXT,
    noi_dung_hoan_coc TEXT,
    CHECK (ngay_di > ngay_den),
    FOREIGN KEY (khach_hang_id)
        REFERENCES KHACH_HANG(id)
);
CREATE TABLE IF NOT EXISTS CHI_TIET_DAT_PHONG(
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  dat_phong_id INTEGER NOT NULL,
  loai_phong_id INTEGER NOT NULL,
  phong_id INTEGER,
  
  gia_moi_dem INTEGER NOT NULL
        CHECK (gia_moi_dem >= 0),
    
  gia_moi_gio INTEGER NOT NULL
        CHECK (gia_moi_gio >= 0),
    checkin_thuc_te TEXT,
    checkout_thuc_te TEXT,
    
    huy_luc TEXT,
    coc_giu_lai INTEGER NOT NULL DEFAULT 0,
    tien_phong INTEGER,
    tien_dich_vu INTEGER,
    FOREIGN KEY (dat_phong_id)
        REFERENCES DAT_PHONG(id),
    FOREIGN KEY (loai_phong_id)
        REFERENCES LOAI_PHONG(id),
    FOREIGN KEY (phong_id)
        REFERENCES PHONG(id)
);

CREATE TABLE IF NOT EXISTS CHINH_SACH_HUY(
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    so_gio_toi_thieu INTEGER NOT NULL UNIQUE,
    ty_le_hoan INTEGER NOT NULL CHECK (ty_le_hoan BETWEEN 0 AND 100)
);
CREATE TABLE IF NOT EXISTS CAU_HINH(
    ten_tham_so TEXT PRIMARY KEY,
    gia_tri TEXT
);
CREATE TABLE IF NOT EXISTS TAI_KHOAN(
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    ten_dang_nhap TEXT NOT NULL UNIQUE,
    mat_khau_bam TEXT NOT NULL,
    vai_tro TEXT NOT NULL CHECK (vai_tro IN ('CHU', 'NHAN_VIEN')),
    dang_hoat_dong INTEGER NOT NULL DEFAULT 1 CHECK (dang_hoat_dong IN(0, 1))
);
CREATE TABLE IF NOT EXISTS DICH_VU (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    ten TEXT NOT NULL UNIQUE,
    don_gia INTEGER NOT NULL,
    don_vi TEXT,
    dang_ban INTEGER NOT NULL DEFAULT 1 CHECK (dang_ban IN(0, 1))
);
CREATE TABLE IF NOT EXISTS KHACH_O_PHONG(
    chi_tiet_dat_phong_id INTEGER NOT NULL,
    khach_hang_id INTEGER NOT NULL,
    PRIMARY KEY (
    chi_tiet_dat_phong_id,
    khach_hang_id
),
    FOREIGN KEY (chi_tiet_dat_phong_id)
        REFERENCES CHI_TIET_DAT_PHONG(id),
    FOREIGN KEY (khach_hang_id)
        REFERENCES KHACH_HANG(id)
);
CREATE TABLE IF NOT EXISTS CHI_TIET_DICH_VU (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    chi_tiet_dat_phong_id INTEGER NOT NULL,
    dich_vu_id INTEGER NOT NULL,
    so_luong INTEGER NOT NULL
        CHECK (so_luong > 0),
    don_gia INTEGER NOT NULL
        CHECK (don_gia >= 0),
    thoi_diem TEXT NOT NULL,
    tai_khoan_id INTEGER,
    da_xoa INTEGER NOT NULL DEFAULT 0
        CHECK (da_xoa IN(0, 1)),
    ghi_chu_xoa TEXT,
    
    FOREIGN KEY (chi_tiet_dat_phong_id)
        REFERENCES CHI_TIET_DAT_PHONG(id),
    FOREIGN KEY (dich_vu_id)
        REFERENCES DICH_VU(id),
   FOREIGN KEY (tai_khoan_id)
        REFERENCES TAI_KHOAN(id) 
);
CREATE TABLE IF NOT EXISTS HOA_DON(
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    ma_hoa_don TEXT NOT NULL UNIQUE,
    dat_phong_id INTEGER NOT NULL UNIQUE,
    thoi_diem_lap TEXT NOT NULL,
    tong_tien_phong INTEGER NOT NULL,
    tong_dich_vu INTEGER NOT NULL,
    tien_coc INTEGER NOT NULL,
    
    so_tien_con_lai INTEGER NOT NULL,
    trang_thai TEXT NOT NULL
                    CHECK (
                        trang_thai IN (
                            'CHO_THANH_TOAN',
                            'DA_THANH_TOAN'
            )
        ),
    thoi_diem_thanh_toan TEXT,
    FOREIGN KEY (dat_phong_id)
        REFERENCES DAT_PHONG(id)
);