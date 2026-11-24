/*
 -- CƠ SỞ DỮ LIỆU ĐẶT PHÒNG KHÁCH SẠN - LƯỢC ĐỒ V3
-- Được xây dựng lại từ lược đồ gồm 12 bảng và các thông số kỹ thuật của 3 mô-đun đã cung cấp.
-- Dành cho SQLite. Chỉ chạy trên cơ sở dữ liệu MỚI/TRỐNG.
-- Quy ước dấu thời gian (timestamp): theo giờ địa phương tại khách sạn, định dạng chính xác là 'YYYY-MM-DD HH:MM:SS'.
-- Tiền tệ: số nguyên (đơn vị VND). Không sử dụng giá trị tiền tệ dạng số thập phân (dấu phẩy động).
 */
PRAGMA foreign_keys = ON;
PRAGMA recursive_triggers = ON;

BEGIN IMMEDIATE;

--1. dữ liệu gốc

CREATE TABLE LOAI_PHONG (
                            id              INTEGER PRIMARY KEY AUTOINCREMENT,
                            ten_loai        TEXT NOT NULL UNIQUE CHECK (length(trim(ten_loai)) > 0),
                            gia_theo_gio    INTEGER NOT NULL CHECK (typeof(gia_theo_gio) = 'integer' AND gia_theo_gio >= 0),
                            gia_theo_ngay   INTEGER NOT NULL CHECK (typeof(gia_theo_ngay) = 'integer' AND gia_theo_ngay >= 0)
);

CREATE TABLE PHONG (
                       id              INTEGER PRIMARY KEY AUTOINCREMENT,
                       loai_phong_id   INTEGER NOT NULL,
                       so_phong        TEXT NOT NULL UNIQUE CHECK (length(trim(so_phong)) > 0),
                       tang            INTEGER NOT NULL CHECK (typeof(tang) = 'integer'),
                       che_do          TEXT NOT NULL CHECK (che_do IN ('THEO_NGAY', 'THEO_GIO')),
                       can_don_dep     INTEGER NOT NULL DEFAULT 0 CHECK (typeof(can_don_dep) = 'integer' AND can_don_dep IN (0, 1)),
                       don_xong_luc    TEXT,
                       CHECK (
                           don_xong_luc IS NULL OR
                           (length(don_xong_luc) = 19 AND datetime(don_xong_luc) IS NOT NULL AND datetime(don_xong_luc) = don_xong_luc)
                           ),
                       CHECK (
                           (can_don_dep = 0 AND don_xong_luc IS NULL) OR
                           (can_don_dep = 1 AND don_xong_luc IS NOT NULL)
                           ),
                       FOREIGN KEY (loai_phong_id) REFERENCES LOAI_PHONG(id) ON UPDATE RESTRICT ON DELETE RESTRICT
);

CREATE TABLE KHACH_HANG (
                            id              INTEGER PRIMARY KEY AUTOINCREMENT,
                            ho_ten          TEXT NOT NULL CHECK (length(trim(ho_ten)) BETWEEN 1 AND 100),
                            so_dien_thoai   TEXT,
                            loai_giay_to    TEXT CHECK (loai_giay_to IS NULL OR loai_giay_to IN ('CCCD', 'HO_CHIEU')),
                            so_giay_to      TEXT,
                            CHECK (
                                so_dien_thoai IS NULL OR
                                (length(so_dien_thoai) = 10 AND substr(so_dien_thoai, 1, 1) = '0' AND so_dien_thoai NOT GLOB '*[^0-9]*')
                                ),
                            CHECK (
                                (loai_giay_to IS NULL AND so_giay_to IS NULL) OR
                                (loai_giay_to = 'CCCD' AND length(so_giay_to) = 12 AND so_giay_to NOT GLOB '*[^0-9]*') OR
                                (loai_giay_to = 'HO_CHIEU' AND length(so_giay_to) BETWEEN 6 AND 12 AND so_giay_to NOT GLOB '*[^A-Za-z0-9]*')
                                ),
                            UNIQUE (loai_giay_to, so_giay_to)
);

CREATE TABLE TAI_KHOAN (
                           id              INTEGER PRIMARY KEY AUTOINCREMENT,
                           ten_dang_nhap   TEXT NOT NULL UNIQUE CHECK (length(trim(ten_dang_nhap)) > 0),
                           mat_khau_bam    TEXT NOT NULL CHECK (length(trim(mat_khau_bam)) > 0),
                           vai_tro         TEXT NOT NULL CHECK (vai_tro IN ('CHU', 'NHAN_VIEN')),
                           dang_hoat_dong  INTEGER NOT NULL DEFAULT 1 CHECK (typeof(dang_hoat_dong) = 'integer' AND dang_hoat_dong IN (0, 1))
);

CREATE TABLE DICH_VU (
                         id              INTEGER PRIMARY KEY AUTOINCREMENT,
                         ten             TEXT NOT NULL UNIQUE CHECK (length(trim(ten)) > 0),
                         don_gia         INTEGER NOT NULL CHECK (typeof(don_gia) = 'integer' AND don_gia >= 0),
                         don_vi          TEXT,
                         dang_ban        INTEGER NOT NULL DEFAULT 1 CHECK (typeof(dang_ban) = 'integer' AND dang_ban IN (0, 1))
);

CREATE TABLE CHINH_SACH_HUY (
                                id                  INTEGER PRIMARY KEY AUTOINCREMENT,
                                so_gio_toi_thieu    INTEGER NOT NULL UNIQUE CHECK (typeof(so_gio_toi_thieu) = 'integer' AND so_gio_toi_thieu IN (0, 24, 48)),
                                ty_le_hoan          INTEGER NOT NULL CHECK (typeof(ty_le_hoan) = 'integer' AND ((so_gio_toi_thieu = 0 AND ty_le_hoan = 0) OR (so_gio_toi_thieu = 24 AND ty_le_hoan = 50) OR (so_gio_toi_thieu = 48 AND ty_le_hoan = 100)))
);

CREATE TABLE CAU_HINH (
                          ten_tham_so    TEXT PRIMARY KEY CHECK (length(trim(ten_tham_so)) > 0),
                          gia_tri        TEXT
);


-- 2. đặt phòng

CREATE TABLE DAT_PHONG (
                           id                  INTEGER PRIMARY KEY AUTOINCREMENT,
                           ma_dat_phong        TEXT NOT NULL UNIQUE CHECK (length(trim(ma_dat_phong)) > 0),
                           khach_hang_id       INTEGER NOT NULL,
                           la_vang_lai         INTEGER NOT NULL DEFAULT 0 CHECK (typeof(la_vang_lai) = 'integer' AND la_vang_lai IN (0, 1)),
                           ngay_den            TEXT NOT NULL,
                           ngay_di             TEXT NOT NULL,
                           tien_coc            INTEGER NOT NULL DEFAULT 0 CHECK (typeof(tien_coc) = 'integer' AND tien_coc >= 0),
                           han_thanh_toan_coc  TEXT,
                           thoi_diem_coc       TEXT,
                           gio_giu_phong_den   TEXT,
                           trang_thai          TEXT NOT NULL CHECK (trang_thai IN ('CHO_COC', 'DA_DAT', 'DANG_O', 'DA_TRA', 'DA_HUY')),
                           ly_do_huy           TEXT CHECK (ly_do_huy IS NULL OR ly_do_huy IN ('KHACH_HUY', 'KHACH_SAN_HUY', 'BAT_KHA_KHANG', 'NO_SHOW', 'HET_HAN_COC')),
                           thoi_diem_huy       TEXT,
                           ghi_chu_huy         TEXT,
                           tien_coc_hoan       INTEGER NOT NULL DEFAULT 0 CHECK (typeof(tien_coc_hoan) = 'integer' AND tien_coc_hoan >= 0),
                           ma_hoan_coc         TEXT UNIQUE,
                           thoi_diem_hoan_coc  TEXT,
                           tk_hoan_ten         TEXT,
                           tk_hoan_so          TEXT,
                           tk_hoan_ngan_hang   TEXT,
                           noi_dung_hoan_coc   TEXT,

                           CHECK (length(ngay_den) = 19 AND datetime(ngay_den) IS NOT NULL AND datetime(ngay_den) = ngay_den),
                           CHECK (length(ngay_di) = 19 AND datetime(ngay_di) IS NOT NULL AND datetime(ngay_di) = ngay_di),
                           CHECK (datetime(ngay_di) > datetime(ngay_den)),
                           CHECK (han_thanh_toan_coc IS NULL OR (length(han_thanh_toan_coc) = 19 AND datetime(han_thanh_toan_coc) IS NOT NULL AND datetime(han_thanh_toan_coc) = han_thanh_toan_coc)),
                           CHECK (thoi_diem_coc IS NULL OR (length(thoi_diem_coc) = 19 AND datetime(thoi_diem_coc) IS NOT NULL AND datetime(thoi_diem_coc) = thoi_diem_coc)),
                           CHECK (gio_giu_phong_den IS NULL OR (length(gio_giu_phong_den) = 19 AND datetime(gio_giu_phong_den) IS NOT NULL AND datetime(gio_giu_phong_den) = gio_giu_phong_den)),
                           CHECK (thoi_diem_huy IS NULL OR (length(thoi_diem_huy) = 19 AND datetime(thoi_diem_huy) IS NOT NULL AND datetime(thoi_diem_huy) = thoi_diem_huy)),
                           CHECK (thoi_diem_hoan_coc IS NULL OR (length(thoi_diem_hoan_coc) = 19 AND datetime(thoi_diem_hoan_coc) IS NOT NULL AND datetime(thoi_diem_hoan_coc) = thoi_diem_hoan_coc)),
                           CHECK (gio_giu_phong_den IS NULL OR (datetime(gio_giu_phong_den) > datetime(ngay_den) AND datetime(gio_giu_phong_den) <= datetime(ngay_di))),
                           CHECK (tien_coc_hoan <= tien_coc),
                           CHECK (
                               (tien_coc_hoan = 0 AND ma_hoan_coc IS NULL AND thoi_diem_hoan_coc IS NULL AND noi_dung_hoan_coc IS NULL) OR
                               (tien_coc_hoan > 0 AND ma_hoan_coc IS NOT NULL AND length(trim(ma_hoan_coc)) > 0 AND thoi_diem_hoan_coc IS NOT NULL AND noi_dung_hoan_coc IS NOT NULL AND length(trim(noi_dung_hoan_coc)) > 0)
                               ),
                           CHECK (
                               (la_vang_lai = 1 AND tien_coc = 0 AND thoi_diem_coc IS NULL AND han_thanh_toan_coc IS NULL AND tien_coc_hoan = 0 AND tk_hoan_ten IS NULL AND tk_hoan_so IS NULL AND tk_hoan_ngan_hang IS NULL) OR
                               (la_vang_lai = 0 AND length(trim(COALESCE(tk_hoan_ten, ''))) > 0 AND length(trim(COALESCE(tk_hoan_so, ''))) > 0 AND length(trim(COALESCE(tk_hoan_ngan_hang, ''))) > 0)
                               ),
                           CHECK (
                               (trang_thai = 'CHO_COC' AND la_vang_lai = 0 AND han_thanh_toan_coc IS NOT NULL AND thoi_diem_coc IS NULL AND ly_do_huy IS NULL AND thoi_diem_huy IS NULL) OR
                               (trang_thai = 'DA_DAT' AND la_vang_lai = 0 AND han_thanh_toan_coc IS NULL AND thoi_diem_coc IS NOT NULL AND ly_do_huy IS NULL AND thoi_diem_huy IS NULL) OR
                               (trang_thai = 'DANG_O' AND han_thanh_toan_coc IS NULL AND ly_do_huy IS NULL AND thoi_diem_huy IS NULL AND ((la_vang_lai = 1 AND thoi_diem_coc IS NULL) OR (la_vang_lai = 0 AND thoi_diem_coc IS NOT NULL))) OR
                               (trang_thai = 'DA_TRA' AND han_thanh_toan_coc IS NULL AND ly_do_huy IS NULL AND thoi_diem_huy IS NULL AND ((la_vang_lai = 1 AND thoi_diem_coc IS NULL) OR (la_vang_lai = 0 AND thoi_diem_coc IS NOT NULL))) OR
                               (trang_thai = 'DA_HUY' AND la_vang_lai = 0 AND han_thanh_toan_coc IS NULL AND ly_do_huy IS NOT NULL AND thoi_diem_huy IS NOT NULL AND (
                                   (ly_do_huy = 'HET_HAN_COC' AND thoi_diem_coc IS NULL AND tien_coc_hoan = 0) OR
                                   (ly_do_huy = 'KHACH_HUY' AND ((thoi_diem_coc IS NULL AND tien_coc_hoan = 0) OR thoi_diem_coc IS NOT NULL)) OR
                                   (ly_do_huy IN ('KHACH_SAN_HUY', 'BAT_KHA_KHANG', 'NO_SHOW') AND thoi_diem_coc IS NOT NULL)
                                   ))
                               ),
                           CHECK (ghi_chu_huy IS NULL OR length(ghi_chu_huy) <= 200),
                           CHECK (ly_do_huy IS NULL OR ly_do_huy NOT IN ('KHACH_SAN_HUY', 'BAT_KHA_KHANG') OR length(trim(COALESCE(ghi_chu_huy, ''))) BETWEEN 1 AND 200),
                           FOREIGN KEY (khach_hang_id) REFERENCES KHACH_HANG(id) ON UPDATE RESTRICT ON DELETE RESTRICT
);


-- 3. chi tiết đặt phòng

CREATE TABLE CHI_TIET_DAT_PHONG (
                                    id                  INTEGER PRIMARY KEY AUTOINCREMENT,
                                    dat_phong_id        INTEGER NOT NULL,
                                    loai_phong_id       INTEGER NOT NULL,
                                    phong_id            INTEGER,
                                    gia_moi_dem         INTEGER NOT NULL CHECK (typeof(gia_moi_dem) = 'integer' AND gia_moi_dem >= 0),
                                    gia_moi_gio         INTEGER NOT NULL CHECK (typeof(gia_moi_gio) = 'integer' AND gia_moi_gio >= 0),
                                    checkin_thuc_te     TEXT,
                                    checkout_thuc_te    TEXT,
                                    huy_luc             TEXT,
                                    coc_giu_lai         INTEGER NOT NULL DEFAULT 0 CHECK (typeof(coc_giu_lai) = 'integer' AND coc_giu_lai >= 0),
                                    tien_phong          INTEGER CHECK (tien_phong IS NULL OR (typeof(tien_phong) = 'integer' AND tien_phong >= 0)),
                                    tien_dich_vu        INTEGER CHECK (tien_dich_vu IS NULL OR (typeof(tien_dich_vu) = 'integer' AND tien_dich_vu >= 0)),

                                    CHECK (checkin_thuc_te IS NULL OR (length(checkin_thuc_te) = 19 AND datetime(checkin_thuc_te) IS NOT NULL AND datetime(checkin_thuc_te) = checkin_thuc_te)),
                                    CHECK (checkout_thuc_te IS NULL OR (length(checkout_thuc_te) = 19 AND datetime(checkout_thuc_te) IS NOT NULL AND datetime(checkout_thuc_te) = checkout_thuc_te)),
                                    CHECK (huy_luc IS NULL OR (length(huy_luc) = 19 AND datetime(huy_luc) IS NOT NULL AND datetime(huy_luc) = huy_luc)),
                                    CHECK (NOT (checkin_thuc_te IS NOT NULL AND huy_luc IS NOT NULL)),
                                    CHECK (checkout_thuc_te IS NULL OR (checkin_thuc_te IS NOT NULL AND datetime(checkout_thuc_te) > datetime(checkin_thuc_te))),
                                    CHECK (checkin_thuc_te IS NULL OR phong_id IS NOT NULL),
                                    CHECK (
                                        (checkout_thuc_te IS NULL AND tien_phong IS NULL AND tien_dich_vu IS NULL) OR
                                        (checkout_thuc_te IS NOT NULL AND tien_phong IS NOT NULL AND tien_dich_vu IS NOT NULL)
                                        ),
                                    CHECK (
                                        (huy_luc IS NULL AND coc_giu_lai = 0) OR
                                        (huy_luc IS NOT NULL AND checkin_thuc_te IS NULL AND checkout_thuc_te IS NULL AND phong_id IS NULL)
                                        ),
                                    FOREIGN KEY (dat_phong_id) REFERENCES DAT_PHONG(id) ON UPDATE RESTRICT ON DELETE RESTRICT,
                                    FOREIGN KEY (loai_phong_id) REFERENCES LOAI_PHONG(id) ON UPDATE RESTRICT ON DELETE RESTRICT,
                                    FOREIGN KEY (phong_id) REFERENCES PHONG(id) ON UPDATE RESTRICT ON DELETE RESTRICT
);

CREATE TABLE KHACH_O_PHONG (
                               chi_tiet_dat_phong_id   INTEGER NOT NULL,
                               khach_hang_id           INTEGER NOT NULL,
                               PRIMARY KEY (chi_tiet_dat_phong_id, khach_hang_id),
                               FOREIGN KEY (chi_tiet_dat_phong_id) REFERENCES CHI_TIET_DAT_PHONG(id) ON UPDATE RESTRICT ON DELETE RESTRICT,
                               FOREIGN KEY (khach_hang_id) REFERENCES KHACH_HANG(id) ON UPDATE RESTRICT ON DELETE RESTRICT
);


-- 4. chi tiết dịch vụ

CREATE TABLE CHI_TIET_DICH_VU (
                                  id                      INTEGER PRIMARY KEY AUTOINCREMENT,
                                  chi_tiet_dat_phong_id   INTEGER NOT NULL,
                                  dich_vu_id              INTEGER NOT NULL,
                                  ten_dich_vu_luc_goi     TEXT NOT NULL CHECK (length(trim(ten_dich_vu_luc_goi)) > 0),
                                  don_vi_luc_goi          TEXT,
                                  so_luong                INTEGER NOT NULL CHECK (typeof(so_luong) = 'integer' AND so_luong BETWEEN 1 AND 99),
                                  don_gia                 INTEGER NOT NULL CHECK (typeof(don_gia) = 'integer' AND don_gia >= 0),
                                  thoi_diem               TEXT NOT NULL,
                                  tai_khoan_id            INTEGER,
                                  da_xoa                  INTEGER NOT NULL DEFAULT 0 CHECK (typeof(da_xoa) = 'integer' AND da_xoa IN (0, 1)),
                                  ghi_chu_xoa             TEXT,
                                  tai_khoan_xoa_id        INTEGER,
                                  thoi_diem_xoa           TEXT,

                                  CHECK (length(thoi_diem) = 19 AND datetime(thoi_diem) IS NOT NULL AND datetime(thoi_diem) = thoi_diem),
                                  CHECK (thoi_diem_xoa IS NULL OR (length(thoi_diem_xoa) = 19 AND datetime(thoi_diem_xoa) IS NOT NULL AND datetime(thoi_diem_xoa) = thoi_diem_xoa AND datetime(thoi_diem_xoa) >= datetime(thoi_diem))),
                                  CHECK (
                                      (da_xoa = 0 AND ghi_chu_xoa IS NULL AND tai_khoan_xoa_id IS NULL AND thoi_diem_xoa IS NULL) OR
                                      (da_xoa = 1 AND length(trim(COALESCE(ghi_chu_xoa, ''))) BETWEEN 1 AND 200 AND tai_khoan_xoa_id IS NOT NULL AND thoi_diem_xoa IS NOT NULL)
                                      ),
                                  FOREIGN KEY (chi_tiet_dat_phong_id) REFERENCES CHI_TIET_DAT_PHONG(id) ON UPDATE RESTRICT ON DELETE RESTRICT,
                                  FOREIGN KEY (dich_vu_id) REFERENCES DICH_VU(id) ON UPDATE RESTRICT ON DELETE RESTRICT,
                                  FOREIGN KEY (tai_khoan_id) REFERENCES TAI_KHOAN(id) ON UPDATE RESTRICT ON DELETE RESTRICT,
                                  FOREIGN KEY (tai_khoan_xoa_id) REFERENCES TAI_KHOAN(id) ON UPDATE RESTRICT ON DELETE RESTRICT
);

-- ---------------------------
-- 5. INVOICE
-- ---------------------------
CREATE TABLE HOA_DON (
                         id                      INTEGER PRIMARY KEY AUTOINCREMENT,
                         ma_hoa_don              TEXT NOT NULL UNIQUE CHECK (length(trim(ma_hoa_don)) > 0),
                         dat_phong_id            INTEGER NOT NULL UNIQUE,
                         thoi_diem_lap           TEXT NOT NULL,
                         tong_tien_phong         INTEGER NOT NULL CHECK (typeof(tong_tien_phong) = 'integer' AND tong_tien_phong >= 0),
                         tong_tien_dich_vu       INTEGER NOT NULL CHECK (typeof(tong_tien_dich_vu) = 'integer' AND tong_tien_dich_vu >= 0),
                         tien_coc_con_lai        INTEGER NOT NULL CHECK (typeof(tien_coc_con_lai) = 'integer' AND tien_coc_con_lai >= 0),
                         so_tien_con_lai         INTEGER NOT NULL CHECK (typeof(so_tien_con_lai) = 'integer'),
                         trang_thai              TEXT NOT NULL CHECK (trang_thai IN ('CHO_THANH_TOAN', 'DA_THANH_TOAN')),
                         thoi_diem_thanh_toan    TEXT,

                         CHECK (length(thoi_diem_lap) = 19 AND datetime(thoi_diem_lap) IS NOT NULL AND datetime(thoi_diem_lap) = thoi_diem_lap),
                         CHECK (thoi_diem_thanh_toan IS NULL OR (length(thoi_diem_thanh_toan) = 19 AND datetime(thoi_diem_thanh_toan) IS NOT NULL AND datetime(thoi_diem_thanh_toan) = thoi_diem_thanh_toan)),
                         CHECK (thoi_diem_thanh_toan IS NULL OR datetime(thoi_diem_thanh_toan) >= datetime(thoi_diem_lap)),
                         CHECK (
                             (so_tien_con_lai > 0 AND trang_thai = 'CHO_THANH_TOAN' AND thoi_diem_thanh_toan IS NULL) OR
                             (so_tien_con_lai > 0 AND trang_thai = 'DA_THANH_TOAN' AND thoi_diem_thanh_toan IS NOT NULL) OR
                             (so_tien_con_lai <= 0 AND trang_thai = 'DA_THANH_TOAN' AND thoi_diem_thanh_toan IS NULL)
                             ),
                         FOREIGN KEY (dat_phong_id) REFERENCES DAT_PHONG(id) ON UPDATE RESTRICT ON DELETE RESTRICT
);


-- 6. INDEXES

CREATE INDEX idx_phong_loai_chedo ON PHONG(loai_phong_id, che_do);
CREATE INDEX idx_khach_hang_dien_thoai ON KHACH_HANG(so_dien_thoai);
CREATE INDEX idx_dat_phong_khach_status ON DAT_PHONG(khach_hang_id, trang_thai);
CREATE INDEX idx_dat_phong_status_han_coc ON DAT_PHONG(trang_thai, han_thanh_toan_coc);
CREATE INDEX idx_chi_tiet_dat_phong_booking ON CHI_TIET_DAT_PHONG(dat_phong_id);
CREATE INDEX idx_chi_tiet_dat_phong_room ON CHI_TIET_DAT_PHONG(phong_id);
CREATE INDEX idx_chi_tiet_dat_phong_no_show ON CHI_TIET_DAT_PHONG(huy_luc) WHERE huy_luc IS NOT NULL;
CREATE INDEX idx_khach_o_phong_khach ON KHACH_O_PHONG(khach_hang_id);
CREATE INDEX idx_chi_tiet_dich_vu_room ON CHI_TIET_DICH_VU(chi_tiet_dat_phong_id);
CREATE INDEX idx_chi_tiet_dich_vu_time ON CHI_TIET_DICH_VU(thoi_diem);
CREATE INDEX idx_hoa_don_status ON HOA_DON(trang_thai, thoi_diem_lap);

-- Một phòng có thể có tối đa một dòng chi tiết ứng với trạng thái đang nhận phòng
CREATE UNIQUE INDEX uq_room_one_active_stay
    ON CHI_TIET_DAT_PHONG(phong_id)
    WHERE phong_id IS NOT NULL
        AND checkin_thuc_te IS NOT NULL
        AND checkout_thuc_te IS NULL
        AND huy_luc IS NULL;

CREATE TRIGGER trg_cau_hinh_validate_insert
    BEFORE INSERT ON CAU_HINH
BEGIN
    SELECT CASE WHEN NEW.ten_tham_so IN ('so_dem_coc', 'thoi_han_coc_phut', 'thoi_gian_don_phut')
        AND (NEW.gia_tri IS NULL OR trim(NEW.gia_tri) = '' OR CAST(NEW.gia_tri AS INTEGER) <= 0 OR CAST(CAST(NEW.gia_tri AS INTEGER) AS TEXT) <> trim(NEW.gia_tri))
                    THEN RAISE(ABORT, 'Tham so nay phai la so nguyen duong') END;
    SELECT CASE WHEN NEW.ten_tham_so IN ('gio_nhan_chuan', 'gio_tra_chuan')
        AND (NEW.gia_tri IS NULL OR NEW.gia_tri NOT GLOB '[0-2][0-9]:[0-5][0-9]' OR substr(NEW.gia_tri,1,2) > '23')
                    THEN RAISE(ABORT, 'Gio chuan phai co dinh dang HH:MM hop le') END;
END;

CREATE TRIGGER trg_cau_hinh_validate_update
    BEFORE UPDATE OF gia_tri, ten_tham_so ON CAU_HINH
BEGIN
    SELECT CASE WHEN NEW.ten_tham_so IN ('so_dem_coc', 'thoi_han_coc_phut', 'thoi_gian_don_phut')
        AND (NEW.gia_tri IS NULL OR trim(NEW.gia_tri) = '' OR CAST(NEW.gia_tri AS INTEGER) <= 0 OR CAST(CAST(NEW.gia_tri AS INTEGER) AS TEXT) <> trim(NEW.gia_tri))
                    THEN RAISE(ABORT, 'Tham so nay phai la so nguyen duong') END;
    SELECT CASE WHEN NEW.ten_tham_so IN ('gio_nhan_chuan', 'gio_tra_chuan')
        AND (NEW.gia_tri IS NULL OR NEW.gia_tri NOT GLOB '[0-2][0-9]:[0-5][0-9]' OR substr(NEW.gia_tri,1,2) > '23')
                    THEN RAISE(ABORT, 'Gio chuan phai co dinh dang HH:MM hop le') END;
END;

CREATE TRIGGER trg_cau_hinh_delete_required_forbidden
    BEFORE DELETE ON CAU_HINH
    WHEN OLD.ten_tham_so IN ('gio_nhan_chuan', 'gio_tra_chuan', 'so_dem_coc', 'thoi_han_coc_phut', 'thoi_gian_don_phut')
BEGIN
    SELECT RAISE(ABORT, 'Khong xoa tham so cau hinh nghiep vu bat buoc');
END;

CREATE TRIGGER trg_cau_hinh_operational_lock
    BEFORE UPDATE OF gia_tri ON CAU_HINH
    WHEN NEW.gia_tri IS NOT OLD.gia_tri
BEGIN
    SELECT CASE WHEN OLD.ten_tham_so = 'so_dem_coc'
        AND EXISTS (SELECT 1 FROM DAT_PHONG WHERE trang_thai = 'CHO_COC')
                    THEN RAISE(ABORT, 'Khong doi so_dem_coc khi dang co dat phong CHO_COC') END;
    SELECT CASE WHEN OLD.ten_tham_so = 'gio_tra_chuan'
        AND EXISTS (SELECT 1 FROM DAT_PHONG WHERE trang_thai IN ('CHO_COC', 'DA_DAT', 'DANG_O'))
                    THEN RAISE(ABORT, 'Khong doi gio_tra_chuan khi dang co dat phong chua ket thuc') END;
    SELECT CASE WHEN OLD.ten_tham_so = 'thoi_gian_don_phut'
        AND (
                         EXISTS (SELECT 1 FROM DAT_PHONG WHERE trang_thai IN ('CHO_COC', 'DA_DAT', 'DANG_O')) OR
                         EXISTS (SELECT 1 FROM PHONG WHERE can_don_dep = 1)
                         )
                    THEN RAISE(ABORT, 'Khong doi thoi_gian_don_phut khi dang co dat phong hoac phong dang don') END;
END;


-- 7. Trigger (bộ kích hoạt) trong SQLite là 1 đoạn lệnh SQL được thiết kế để tự động thực hiện khi 1 thao tác nhất định xảy ra bên trong
-- ---------------------------
CREATE TRIGGER trg_dat_phong_initial_state
    BEFORE INSERT ON DAT_PHONG
    WHEN NOT (
        (NEW.trang_thai = 'CHO_COC' AND NEW.la_vang_lai = 0) OR
        (NEW.trang_thai = 'DANG_O' AND NEW.la_vang_lai = 1)
        )
BEGIN
    SELECT RAISE(ABORT, 'Dat truoc phai tao CHO_COC; khach vang lai moi duoc tao DANG_O');
END;

CREATE TRIGGER trg_dat_phong_phone_required
    BEFORE INSERT ON DAT_PHONG
    WHEN (SELECT so_dien_thoai FROM KHACH_HANG WHERE id = NEW.khach_hang_id) IS NULL
        OR length(trim((SELECT so_dien_thoai FROM KHACH_HANG WHERE id = NEW.khach_hang_id))) = 0
BEGIN
    SELECT RAISE(ABORT, 'Người đặt phòng phải có số điện thoại');
END;

CREATE TRIGGER trg_dat_phong_one_pending_phone_insert
    BEFORE INSERT ON DAT_PHONG
    WHEN NEW.trang_thai = 'CHO_COC'
        AND EXISTS (
            SELECT 1
            FROM DAT_PHONG d
                     JOIN KHACH_HANG old_k ON old_k.id = d.khach_hang_id
                     JOIN KHACH_HANG new_k ON new_k.id = NEW.khach_hang_id
            WHERE d.trang_thai = 'CHO_COC'
              AND old_k.so_dien_thoai = new_k.so_dien_thoai
        )
BEGIN
    SELECT RAISE(ABORT, 'Số điện thoại đã có đặt phòng CHO_COC');
END;

CREATE TRIGGER trg_dat_phong_one_pending_phone_update
    BEFORE UPDATE OF trang_thai, khach_hang_id ON DAT_PHONG
    WHEN NEW.trang_thai = 'CHO_COC'
        AND EXISTS (
            SELECT 1
            FROM DAT_PHONG d
                     JOIN KHACH_HANG old_k ON old_k.id = d.khach_hang_id
                     JOIN KHACH_HANG new_k ON new_k.id = NEW.khach_hang_id
            WHERE d.id <> NEW.id
              AND d.trang_thai = 'CHO_COC'
              AND old_k.so_dien_thoai = new_k.so_dien_thoai
        )
BEGIN
    SELECT RAISE(ABORT, 'Số điện thoại đã có đặt phòng CHO_COC');
END;

CREATE TRIGGER trg_dat_phong_state_transition
    BEFORE UPDATE OF trang_thai ON DAT_PHONG
    WHEN NEW.trang_thai <> OLD.trang_thai
BEGIN
    SELECT CASE
               WHEN NOT (
                   (OLD.trang_thai = 'CHO_COC' AND NEW.trang_thai IN ('DA_DAT', 'DA_HUY')) OR
                   (OLD.trang_thai = 'DA_DAT' AND NEW.trang_thai IN ('DANG_O', 'DA_HUY')) OR
                   (OLD.trang_thai = 'DANG_O' AND NEW.trang_thai = 'DA_TRA')
                   )
                   THEN RAISE(ABORT, 'Chuyen trang thai DAT_PHONG khong hop le')
               END;

    SELECT CASE WHEN OLD.trang_thai = 'CHO_COC' AND NEW.trang_thai = 'DA_DAT'
        AND datetime(NEW.thoi_diem_coc) >= datetime(OLD.han_thanh_toan_coc)
                    THEN RAISE(ABORT, 'Thanh toan coc phai truoc han thanh toan') END;

    SELECT CASE WHEN OLD.trang_thai = 'CHO_COC' AND NEW.trang_thai = 'DA_DAT'
        AND (
                         NOT EXISTS (SELECT 1 FROM CHI_TIET_DAT_PHONG c WHERE c.dat_phong_id = NEW.id) OR
                         NEW.tien_coc <> COALESCE((SELECT SUM(c.gia_moi_dem) FROM CHI_TIET_DAT_PHONG c WHERE c.dat_phong_id = NEW.id), 0) *
                                         COALESCE((SELECT CAST(gia_tri AS INTEGER) FROM CAU_HINH WHERE ten_tham_so = 'so_dem_coc'), 1)
                         )
                    THEN RAISE(ABORT, 'tien_coc khong khop CP-02 hoac dat phong chua co dong phong') END;

    SELECT CASE WHEN OLD.trang_thai = 'CHO_COC' AND NEW.trang_thai = 'DA_HUY'
        AND NEW.ly_do_huy = 'HET_HAN_COC'
        AND datetime(NEW.thoi_diem_huy) < datetime(OLD.han_thanh_toan_coc)
                    THEN RAISE(ABORT, 'Chua het han coc') END;

    SELECT CASE WHEN NEW.trang_thai = 'DA_HUY'
        AND NEW.tien_coc_hoan <> CASE
                                     WHEN NEW.thoi_diem_coc IS NULL THEN 0
                                     WHEN NEW.ly_do_huy IN ('KHACH_SAN_HUY', 'BAT_KHA_KHANG') THEN NEW.tien_coc
                                     WHEN NEW.ly_do_huy = 'NO_SHOW' OR NEW.ly_do_huy = 'HET_HAN_COC' THEN 0
                                     WHEN NEW.ly_do_huy = 'KHACH_HUY' THEN
                                         CASE
                                             WHEN (julianday(NEW.ngay_den) - julianday(NEW.thoi_diem_huy)) * 24 >= 48 THEN NEW.tien_coc
                                             WHEN (julianday(NEW.ngay_den) - julianday(NEW.thoi_diem_huy)) * 24 >= 24 THEN CAST(NEW.tien_coc * 50 / 100 AS INTEGER)
                                             ELSE 0
                                             END
                                     ELSE 0
            END
                    THEN RAISE(ABORT, 'tien_coc_hoan khong dung chinh sach huy CP-04/CP-07') END;

    SELECT CASE WHEN NEW.trang_thai = 'DANG_O'
        AND NOT EXISTS (
            SELECT 1 FROM CHI_TIET_DAT_PHONG c
            WHERE c.dat_phong_id = NEW.id AND c.checkin_thuc_te IS NOT NULL
        )
                    THEN RAISE(ABORT, 'Chi chuyen DANG_O khi co it nhat mot dong da check-in')
               END;

    SELECT CASE WHEN NEW.trang_thai = 'DA_HUY'
        AND EXISTS (
            SELECT 1 FROM CHI_TIET_DAT_PHONG c
            WHERE c.dat_phong_id = NEW.id AND c.checkin_thuc_te IS NOT NULL
        )
                    THEN RAISE(ABORT, 'Khong duoc huy toan bo dat phong da co check-in; dung no-show tung dong')
               END;

    SELECT CASE WHEN NEW.trang_thai = 'DA_TRA'
        AND (
                         NOT EXISTS (SELECT 1 FROM HOA_DON h WHERE h.dat_phong_id = NEW.id) OR
                         EXISTS (
                             SELECT 1 FROM CHI_TIET_DAT_PHONG c
                             WHERE c.dat_phong_id = NEW.id
                               AND NOT (
                                 (c.checkin_thuc_te IS NOT NULL AND c.checkout_thuc_te IS NOT NULL AND c.huy_luc IS NULL
                                     AND c.tien_phong IS NOT NULL AND c.tien_dich_vu IS NOT NULL) OR
                                 (c.checkin_thuc_te IS NULL AND c.checkout_thuc_te IS NULL AND c.huy_luc IS NOT NULL)
                                 )
                         )
                         )
                    THEN RAISE(ABORT, 'DAT_PHONG chi DA_TRA khi moi dong da ket thuc hop le va da co HOA_DON')
               END;
END;

CREATE TRIGGER trg_dat_phong_deposit_time_guard
    BEFORE UPDATE OF thoi_diem_coc, han_thanh_toan_coc ON DAT_PHONG
BEGIN
    SELECT CASE WHEN NEW.thoi_diem_coc IS NOT OLD.thoi_diem_coc AND NOT (
        OLD.trang_thai = 'CHO_COC' AND NEW.trang_thai = 'DA_DAT'
            AND OLD.thoi_diem_coc IS NULL AND NEW.thoi_diem_coc IS NOT NULL
        )
                    THEN RAISE(ABORT, 'thoi_diem_coc chi duoc ghi mot lan khi CHO_COC chuyen DA_DAT') END;
    SELECT CASE WHEN OLD.thoi_diem_coc IS NOT NULL AND NEW.thoi_diem_coc IS NOT OLD.thoi_diem_coc
                    THEN RAISE(ABORT, 'thoi_diem_coc da ghi thi khong duoc sua/xoa') END;
    SELECT CASE WHEN NEW.han_thanh_toan_coc IS NOT OLD.han_thanh_toan_coc AND NOT (
        OLD.trang_thai = 'CHO_COC' AND NEW.trang_thai IN ('DA_DAT', 'DA_HUY') AND NEW.han_thanh_toan_coc IS NULL
        )
                    THEN RAISE(ABORT, 'han_thanh_toan_coc chi duoc xoa khi thanh toan hoac huy') END;
END;

CREATE TRIGGER trg_dat_phong_refund_account_immutable
    BEFORE UPDATE OF tk_hoan_ten, tk_hoan_so, tk_hoan_ngan_hang ON DAT_PHONG
    WHEN NEW.tk_hoan_ten IS NOT OLD.tk_hoan_ten OR NEW.tk_hoan_so IS NOT OLD.tk_hoan_so OR NEW.tk_hoan_ngan_hang IS NOT OLD.tk_hoan_ngan_hang
BEGIN
    SELECT RAISE(ABORT, 'Thong tin tai khoan nhan hoan da chot khi tao dat phong');
END;

CREATE TRIGGER trg_dat_phong_immutable_identity
    BEFORE UPDATE OF ma_dat_phong, khach_hang_id, la_vang_lai, tien_coc ON DAT_PHONG
    WHEN NEW.ma_dat_phong <> OLD.ma_dat_phong
        OR NEW.khach_hang_id <> OLD.khach_hang_id
        OR NEW.la_vang_lai <> OLD.la_vang_lai
        OR NEW.tien_coc <> OLD.tien_coc
BEGIN
    SELECT RAISE(ABORT, 'Thong tin dinh danh va tien_coc da chot khong duoc sua');
END;

CREATE TRIGGER trg_dat_phong_date_extension_only
    BEFORE UPDATE OF ngay_den, ngay_di ON DAT_PHONG
    WHEN NEW.ngay_den <> OLD.ngay_den OR NEW.ngay_di <> OLD.ngay_di
BEGIN
    SELECT CASE WHEN NEW.ngay_den <> OLD.ngay_den
                    THEN RAISE(ABORT, 'Ngay den khong duoc sua trong pham vi hien tai') END;
    SELECT CASE WHEN OLD.trang_thai NOT IN ('DA_DAT', 'DANG_O') OR datetime(NEW.ngay_di) <= datetime(OLD.ngay_di)
                    THEN RAISE(ABORT, 'Chi duoc gia han ngay di cua dat phong DA_DAT/DANG_O') END;
END;

CREATE TRIGGER trg_dat_phong_extension_room_overlap
    BEFORE UPDATE OF ngay_di ON DAT_PHONG
    WHEN NEW.ngay_di > OLD.ngay_di
BEGIN
    SELECT CASE WHEN EXISTS (
        SELECT 1
        FROM CHI_TIET_DAT_PHONG own
                 JOIN CHI_TIET_DAT_PHONG other ON other.phong_id = own.phong_id AND other.id <> own.id
                 JOIN DAT_PHONG d2 ON d2.id = other.dat_phong_id
        WHERE own.dat_phong_id = NEW.id
          AND own.phong_id IS NOT NULL
          AND own.huy_luc IS NULL
          AND other.huy_luc IS NULL
          AND d2.trang_thai IN ('CHO_COC', 'DA_DAT', 'DANG_O')
          AND datetime(COALESCE(other.checkin_thuc_te, d2.ngay_den)) < datetime(NEW.ngay_di, '+' || COALESCE((SELECT CAST(gia_tri AS INTEGER) FROM CAU_HINH WHERE ten_tham_so='thoi_gian_don_phut'), 30) || ' minutes')
          AND datetime(NEW.ngay_den) < datetime(COALESCE(other.checkout_thuc_te, d2.ngay_di), '+' || COALESCE((SELECT CAST(gia_tri AS INTEGER) FROM CAU_HINH WHERE ten_tham_so='thoi_gian_don_phut'), 30) || ' minutes')
    )
                    THEN RAISE(ABORT, 'Gia han tao xung dot voi dat phong da gan cung phong') END;
END;

CREATE TRIGGER trg_dat_phong_delete_forbidden
    BEFORE DELETE ON DAT_PHONG
BEGIN
    SELECT RAISE(ABORT, 'Khong xoa vat ly DAT_PHONG; su dung trang thai huy');
END;

CREATE TRIGGER trg_dat_phong_refund_once
    BEFORE UPDATE OF tien_coc_hoan, ma_hoan_coc, thoi_diem_hoan_coc, noi_dung_hoan_coc ON DAT_PHONG
    WHEN NEW.tien_coc_hoan <> OLD.tien_coc_hoan
        OR NEW.ma_hoan_coc IS NOT OLD.ma_hoan_coc
        OR NEW.thoi_diem_hoan_coc IS NOT OLD.thoi_diem_hoan_coc
        OR NEW.noi_dung_hoan_coc IS NOT OLD.noi_dung_hoan_coc
BEGIN
    SELECT CASE WHEN OLD.ma_hoan_coc IS NOT NULL
                    THEN RAISE(ABORT, 'Dat phong da co lenh hoan; khong tao lenh hoan thu hai') END;

    SELECT CASE WHEN NEW.tien_coc_hoan <= OLD.tien_coc_hoan
                    THEN RAISE(ABORT, 'So tien hoan moi phai lon hon so da hoan') END;

    SELECT CASE WHEN NOT (
        (NEW.trang_thai = 'DA_HUY' AND NEW.ly_do_huy IN ('KHACH_HUY', 'KHACH_SAN_HUY', 'BAT_KHA_KHANG')) OR
        (NEW.trang_thai = 'DANG_O' AND NOT EXISTS (SELECT 1 FROM HOA_DON h WHERE h.dat_phong_id = NEW.id))
        )
                    THEN RAISE(ABORT, 'Lenh hoan chi hop le khi huy truoc check-in hoac chuan bi hoan coc du trong giao dich CO-04')
               END;

    SELECT CASE WHEN NEW.trang_thai = 'DANG_O'
        AND NEW.tien_coc_hoan > (
            (SELECT tien_coc FROM DAT_PHONG WHERE id = NEW.id) -
            COALESCE((SELECT SUM(c.coc_giu_lai) FROM CHI_TIET_DAT_PHONG c WHERE c.dat_phong_id = NEW.id), 0)
            )
                    THEN RAISE(ABORT, 'Tien hoan du khong duoc vuot coc con lai sau no-show')
               END;
END;

-- ---------------------------
-- 8. TRIGGERS: ROOM MASTER DATA
-- ---------------------------
CREATE TRIGGER trg_phong_cleaning_guard
    BEFORE UPDATE OF can_don_dep, don_xong_luc ON PHONG
    WHEN NEW.can_don_dep = 1
BEGIN
    SELECT CASE WHEN EXISTS (
        SELECT 1 FROM CHI_TIET_DAT_PHONG c
        WHERE c.phong_id = OLD.id AND c.checkin_thuc_te IS NOT NULL AND c.checkout_thuc_te IS NULL AND c.huy_luc IS NULL
    )
                    THEN RAISE(ABORT, 'Khong the danh dau dang don khi van co dong phong dang o') END;
    SELECT CASE WHEN NOT EXISTS (
        SELECT 1 FROM CHI_TIET_DAT_PHONG c
        WHERE c.phong_id = OLD.id AND c.checkout_thuc_te IS NOT NULL
          AND NEW.don_xong_luc = datetime(c.checkout_thuc_te, '+' || COALESCE((SELECT CAST(gia_tri AS INTEGER) FROM CAU_HINH WHERE ten_tham_so='thoi_gian_don_phut'), 30) || ' minutes')
    )
                    THEN RAISE(ABORT, 'don_xong_luc phai bang checkout_thuc_te cong thoi_gian_don_phut') END;
END;

CREATE TRIGGER trg_phong_no_change_type_after_history
    BEFORE UPDATE OF loai_phong_id, che_do, so_phong ON PHONG
    WHEN (NEW.loai_phong_id <> OLD.loai_phong_id OR NEW.che_do <> OLD.che_do OR NEW.so_phong <> OLD.so_phong)
        AND EXISTS (SELECT 1 FROM CHI_TIET_DAT_PHONG c WHERE c.phong_id = OLD.id)
BEGIN
    SELECT RAISE(ABORT, 'Khong doi so phong/loai/che do khi da co lich su dat phong');
END;

CREATE TRIGGER trg_loai_phong_name_no_change_after_history
    BEFORE UPDATE OF ten_loai ON LOAI_PHONG
    WHEN NEW.ten_loai <> OLD.ten_loai
        AND EXISTS (SELECT 1 FROM CHI_TIET_DAT_PHONG c WHERE c.loai_phong_id = OLD.id)
BEGIN
    SELECT RAISE(ABORT, 'Khong doi ten loai phong khi da co lich su giao dich');
END;

CREATE TRIGGER trg_khach_hang_identity_immutable_after_history
    BEFORE UPDATE OF ho_ten, so_dien_thoai, loai_giay_to, so_giay_to ON KHACH_HANG
    WHEN (NEW.ho_ten IS NOT OLD.ho_ten OR NEW.so_dien_thoai IS NOT OLD.so_dien_thoai OR NEW.loai_giay_to IS NOT OLD.loai_giay_to OR NEW.so_giay_to IS NOT OLD.so_giay_to)
        AND (EXISTS (SELECT 1 FROM DAT_PHONG d WHERE d.khach_hang_id = OLD.id)
            OR EXISTS (SELECT 1 FROM KHACH_O_PHONG k WHERE k.khach_hang_id = OLD.id))
BEGIN
    SELECT RAISE(ABORT, 'Khach hang da co lich su: khong sua thong tin dinh danh; can quy trinh dieu chinh co audit/snapshot');
END;

CREATE TRIGGER trg_khach_phone_no_pending_collision
    BEFORE UPDATE OF so_dien_thoai ON KHACH_HANG
    WHEN NEW.so_dien_thoai IS NOT NULL
        AND NEW.so_dien_thoai IS NOT OLD.so_dien_thoai
        AND EXISTS (
            SELECT 1 FROM DAT_PHONG d JOIN KHACH_HANG k ON k.id = d.khach_hang_id
            WHERE d.trang_thai = 'CHO_COC' AND k.id <> OLD.id AND k.so_dien_thoai = NEW.so_dien_thoai
        )
BEGIN
    SELECT RAISE(ABORT, 'So dien thoai moi dang co dat phong CHO_COC khac');
END;

CREATE TRIGGER trg_khach_phone_pending_guard
    BEFORE UPDATE OF so_dien_thoai ON KHACH_HANG
    WHEN NEW.so_dien_thoai IS NOT OLD.so_dien_thoai
        AND EXISTS (SELECT 1 FROM DAT_PHONG d WHERE d.khach_hang_id = OLD.id AND d.trang_thai = 'CHO_COC')
BEGIN
    SELECT RAISE(ABORT, 'Khong doi so dien thoai khi khach co dat phong CHO_COC');
END;

-- ---------------------------
-- 9. TRIGGERS: CHI TIẾT ĐẶT PHÒNG / PHÂN BỔ PHÒNG
-- ---------------------------
CREATE TRIGGER trg_ctdp_insert_guard
    BEFORE INSERT ON CHI_TIET_DAT_PHONG
BEGIN
    SELECT CASE WHEN (SELECT trang_thai FROM DAT_PHONG WHERE id = NEW.dat_phong_id) NOT IN ('CHO_COC', 'DANG_O')
                    THEN RAISE(ABORT, 'Chi them dong phong khi dat phong dang CHO_COC hoac tao khach vang lai DANG_O') END;
    SELECT CASE WHEN (SELECT la_vang_lai FROM DAT_PHONG WHERE id = NEW.dat_phong_id) = 1
        AND (NEW.phong_id IS NULL OR NEW.checkin_thuc_te IS NULL)
                    THEN RAISE(ABORT, 'Khach vang lai phai luu phong vat ly va check-in trong cung giao dich DP-04') END;
    SELECT CASE WHEN NEW.gia_moi_dem <> (SELECT gia_theo_ngay FROM LOAI_PHONG WHERE id = NEW.loai_phong_id)
        OR NEW.gia_moi_gio <> (SELECT gia_theo_gio FROM LOAI_PHONG WHERE id = NEW.loai_phong_id)
                    THEN RAISE(ABORT, 'Gia dong phong phai duoc chup tu bang gia loai phong tai thoi diem tao') END;
    SELECT CASE WHEN NEW.checkout_thuc_te IS NOT NULL OR NEW.huy_luc IS NOT NULL
                    THEN RAISE(ABORT, 'Dong phong moi khong duoc tao san o trang thai DA_TRA/NO_SHOW') END;

    SELECT CASE WHEN NEW.phong_id IS NOT NULL AND NOT EXISTS (
        SELECT 1 FROM PHONG p
        WHERE p.id = NEW.phong_id AND p.loai_phong_id = NEW.loai_phong_id
    )
                    THEN RAISE(ABORT, 'Phong duoc gan phai cung loai voi dong dat phong') END;

    SELECT CASE WHEN NEW.phong_id IS NOT NULL AND EXISTS (
        SELECT 1 FROM DAT_PHONG d JOIN PHONG p ON p.id = NEW.phong_id
        WHERE d.id = NEW.dat_phong_id AND d.la_vang_lai = 0 AND p.che_do <> 'THEO_NGAY'
    )
                    THEN RAISE(ABORT, 'Dat truoc chi duoc gan phong THEO_NGAY') END;

    SELECT CASE WHEN NEW.phong_id IS NOT NULL AND EXISTS (
        SELECT 1 FROM DAT_PHONG d JOIN PHONG p ON p.id = NEW.phong_id
        WHERE d.id = NEW.dat_phong_id AND (
            (p.che_do = 'THEO_GIO' AND date(d.ngay_den) <> date(d.ngay_di)) OR
            (p.che_do = 'THEO_NGAY' AND date(d.ngay_den) = date(d.ngay_di))
            )
    )
                    THEN RAISE(ABORT, 'Khoang ngay/di khong phu hop voi che_do phong') END;

    SELECT CASE WHEN NEW.phong_id IS NOT NULL AND EXISTS (
        SELECT 1 FROM DAT_PHONG d JOIN PHONG p ON p.id = NEW.phong_id
        WHERE d.id = NEW.dat_phong_id AND p.che_do = 'THEO_GIO' AND d.la_vang_lai <> 1
    )
                    THEN RAISE(ABORT, 'Phong THEO_GIO chi danh cho khach vang lai') END;

    SELECT CASE WHEN NEW.phong_id IS NOT NULL AND EXISTS (
        SELECT 1 FROM DAT_PHONG d JOIN PHONG p ON p.id = NEW.phong_id
        WHERE d.id = NEW.dat_phong_id AND (
            (p.che_do = 'THEO_GIO' AND date(d.ngay_den) <> date(d.ngay_di)) OR
            (p.che_do = 'THEO_NGAY' AND date(d.ngay_den) = date(d.ngay_di))
            )
    )
                    THEN RAISE(ABORT, 'Khoang ngay/di khong phu hop voi che_do phong') END;

    SELECT CASE WHEN NEW.phong_id IS NOT NULL AND EXISTS (
        SELECT 1 FROM DAT_PHONG d JOIN PHONG p ON p.id = NEW.phong_id
        WHERE d.id = NEW.dat_phong_id AND p.che_do = 'THEO_GIO'
          AND (SELECT COUNT(*) FROM CHI_TIET_DAT_PHONG c WHERE c.dat_phong_id = NEW.dat_phong_id) >= 1
    )
                    THEN RAISE(ABORT, 'Moi dat phong THEO_GIO chi co mot dong phong') END;

    SELECT CASE WHEN NEW.phong_id IS NOT NULL AND (SELECT trang_thai FROM DAT_PHONG WHERE id = NEW.dat_phong_id) = 'CHO_COC'
                    THEN RAISE(ABORT, 'Khong gan phong cu the khi dat phong dang CHO_COC') END;

    SELECT CASE WHEN NEW.phong_id IS NOT NULL AND EXISTS (
        SELECT 1 FROM PHONG p JOIN DAT_PHONG d ON d.id = NEW.dat_phong_id
        WHERE p.id = NEW.phong_id AND p.can_don_dep = 1 AND datetime(p.don_xong_luc) > datetime(d.ngay_den)
    )
                    THEN RAISE(ABORT, 'Phong chua don xong truoc thoi diem nhan du kien') END;

    SELECT CASE WHEN NEW.phong_id IS NOT NULL AND EXISTS (
        SELECT 1
        FROM CHI_TIET_DAT_PHONG c2
                 JOIN DAT_PHONG d2 ON d2.id = c2.dat_phong_id
                 JOIN DAT_PHONG dn ON dn.id = NEW.dat_phong_id
        WHERE c2.phong_id = NEW.phong_id
          AND c2.id <> NEW.id
          AND c2.huy_luc IS NULL
          AND d2.trang_thai IN ('CHO_COC', 'DA_DAT', 'DANG_O')
          AND datetime(COALESCE(c2.checkin_thuc_te, d2.ngay_den)) < datetime(dn.ngay_di, '+' || COALESCE((SELECT CAST(gia_tri AS INTEGER) FROM CAU_HINH WHERE ten_tham_so='thoi_gian_don_phut'), 30) || ' minutes')
          AND datetime(dn.ngay_den) < datetime(COALESCE(c2.checkout_thuc_te, d2.ngay_di), '+' || COALESCE((SELECT CAST(gia_tri AS INTEGER) FROM CAU_HINH WHERE ten_tham_so='thoi_gian_don_phut'), 30) || ' minutes')
    )
                    THEN RAISE(ABORT, 'Khoang thoi gian su dung phong bi chong lan hoac khong du thoi gian don') END;

    SELECT CASE WHEN NEW.huy_luc IS NOT NULL
                    THEN RAISE(ABORT, 'No-show phai duoc ghi bang DP-09 sau khi dong phong da duoc tao') END;

    SELECT CASE WHEN NEW.checkin_thuc_te IS NOT NULL AND (
        (SELECT trang_thai FROM DAT_PHONG WHERE id = NEW.dat_phong_id) <> 'DANG_O' OR
        NEW.phong_id IS NULL OR
        datetime(NEW.checkin_thuc_te) < datetime((SELECT ngay_den FROM DAT_PHONG WHERE id = NEW.dat_phong_id)) OR
        datetime(NEW.checkin_thuc_te) >= datetime((SELECT ngay_di FROM DAT_PHONG WHERE id = NEW.dat_phong_id)) OR
        EXISTS (SELECT 1 FROM PHONG p WHERE p.id = NEW.phong_id AND p.can_don_dep = 1)
        )
                    THEN RAISE(ABORT, 'Dong check-in moi phai thuoc DANG_O, dung phong va trong khoang hop le') END;
END;

CREATE TRIGGER trg_ctdp_update_room_guard
    BEFORE UPDATE OF phong_id ON CHI_TIET_DAT_PHONG
    WHEN NEW.phong_id IS NOT OLD.phong_id
BEGIN
    SELECT CASE WHEN OLD.checkin_thuc_te IS NOT NULL OR OLD.checkout_thuc_te IS NOT NULL OR OLD.huy_luc IS NOT NULL
                    THEN RAISE(ABORT, 'Khong doi phong sau check-in, check-out hoac no-show') END;

    SELECT CASE WHEN NEW.phong_id IS NOT NULL AND (SELECT trang_thai FROM DAT_PHONG WHERE id = NEW.dat_phong_id) NOT IN ('DA_DAT', 'DANG_O')
                    THEN RAISE(ABORT, 'Chi gan phong cho dong thuoc dat phong DA_DAT/DANG_O') END;

    SELECT CASE WHEN NEW.phong_id IS NOT NULL AND NOT EXISTS (
        SELECT 1 FROM PHONG p
        WHERE p.id = NEW.phong_id AND p.loai_phong_id = NEW.loai_phong_id
    )
                    THEN RAISE(ABORT, 'Phong duoc gan phai cung loai voi dong dat phong') END;

    SELECT CASE WHEN NEW.phong_id IS NOT NULL AND EXISTS (
        SELECT 1 FROM DAT_PHONG d JOIN PHONG p ON p.id = NEW.phong_id
        WHERE d.id = NEW.dat_phong_id AND d.la_vang_lai = 0 AND p.che_do <> 'THEO_NGAY'
    )
                    THEN RAISE(ABORT, 'Dat truoc chi duoc gan phong THEO_NGAY') END;

    SELECT CASE WHEN NEW.phong_id IS NOT NULL AND EXISTS (
        SELECT 1 FROM DAT_PHONG d JOIN PHONG p ON p.id = NEW.phong_id
        WHERE d.id = NEW.dat_phong_id AND (
            (p.che_do = 'THEO_GIO' AND date(d.ngay_den) <> date(d.ngay_di)) OR
            (p.che_do = 'THEO_NGAY' AND date(d.ngay_den) = date(d.ngay_di))
            )
    )
                    THEN RAISE(ABORT, 'Khoang ngay/di khong phu hop voi che_do phong') END;

    SELECT CASE WHEN NEW.phong_id IS NOT NULL AND EXISTS (
        SELECT 1 FROM DAT_PHONG d JOIN PHONG p ON p.id = NEW.phong_id
        WHERE d.id = NEW.dat_phong_id AND p.che_do = 'THEO_GIO'
          AND (SELECT COUNT(*) FROM CHI_TIET_DAT_PHONG c WHERE c.dat_phong_id = NEW.dat_phong_id) <> 1
    )
                    THEN RAISE(ABORT, 'Moi dat phong THEO_GIO chi co mot dong phong') END;

    SELECT CASE WHEN NEW.phong_id IS NOT NULL AND EXISTS (
        SELECT 1 FROM PHONG p JOIN DAT_PHONG d ON d.id = NEW.dat_phong_id
        WHERE p.id = NEW.phong_id AND p.can_don_dep = 1 AND datetime(p.don_xong_luc) > datetime(d.ngay_den)
    )
                    THEN RAISE(ABORT, 'Phong chua don xong truoc thoi diem nhan du kien') END;

    SELECT CASE WHEN NEW.phong_id IS NOT NULL AND EXISTS (
        SELECT 1
        FROM CHI_TIET_DAT_PHONG c2
                 JOIN DAT_PHONG d2 ON d2.id = c2.dat_phong_id
                 JOIN DAT_PHONG dn ON dn.id = NEW.dat_phong_id
        WHERE c2.phong_id = NEW.phong_id
          AND c2.id <> NEW.id
          AND c2.huy_luc IS NULL
          AND d2.trang_thai IN ('CHO_COC', 'DA_DAT', 'DANG_O')
          AND datetime(COALESCE(c2.checkin_thuc_te, d2.ngay_den)) < datetime(dn.ngay_di, '+' || COALESCE((SELECT CAST(gia_tri AS INTEGER) FROM CAU_HINH WHERE ten_tham_so='thoi_gian_don_phut'), 30) || ' minutes')
          AND datetime(dn.ngay_den) < datetime(COALESCE(c2.checkout_thuc_te, d2.ngay_di), '+' || COALESCE((SELECT CAST(gia_tri AS INTEGER) FROM CAU_HINH WHERE ten_tham_so='thoi_gian_don_phut'), 30) || ' minutes')
    )
                    THEN RAISE(ABORT, 'Khoang thoi gian su dung phong bi chong lan hoac khong du thoi gian don') END;
END;

CREATE TRIGGER trg_ctdp_checkin_guard
    BEFORE UPDATE OF checkin_thuc_te ON CHI_TIET_DAT_PHONG
    WHEN OLD.checkin_thuc_te IS NULL AND NEW.checkin_thuc_te IS NOT NULL
BEGIN
    SELECT CASE WHEN NEW.phong_id IS NULL
                    THEN RAISE(ABORT, 'Phai gan phong vat ly truoc khi check-in') END;
    SELECT CASE WHEN NEW.huy_luc IS NOT NULL OR NEW.checkout_thuc_te IS NOT NULL
                    THEN RAISE(ABORT, 'Dong phong da ket thuc khong the check-in') END;
    SELECT CASE WHEN (SELECT trang_thai FROM DAT_PHONG WHERE id = NEW.dat_phong_id) NOT IN ('DA_DAT', 'DANG_O')
                    THEN RAISE(ABORT, 'Chi check-in dat phong DA_DAT/DANG_O') END;
    SELECT CASE WHEN NOT EXISTS (
        SELECT 1 FROM PHONG p WHERE p.id = NEW.phong_id AND p.loai_phong_id = NEW.loai_phong_id AND p.can_don_dep = 0
    )
                    THEN RAISE(ABORT, 'Phong khong dung loai hoac dang don dep') END;
    SELECT CASE WHEN datetime(NEW.checkin_thuc_te) < datetime((SELECT ngay_den FROM DAT_PHONG WHERE id = NEW.dat_phong_id))
        OR datetime(NEW.checkin_thuc_te) >= datetime((SELECT ngay_di FROM DAT_PHONG WHERE id = NEW.dat_phong_id))
                    THEN RAISE(ABORT, 'Thoi diem check-in nam ngoai khoang DAT_PHONG') END;
END;

CREATE TRIGGER trg_ctdp_service_total_at_checkout
    BEFORE UPDATE OF checkout_thuc_te ON CHI_TIET_DAT_PHONG
    WHEN OLD.checkout_thuc_te IS NULL AND NEW.checkout_thuc_te IS NOT NULL
BEGIN
    SELECT CASE WHEN NEW.tien_dich_vu <> COALESCE((
                                                      SELECT SUM(s.so_luong * s.don_gia)
                                                      FROM CHI_TIET_DICH_VU s
                                                      WHERE s.chi_tiet_dat_phong_id = NEW.id AND s.da_xoa = 0
                                                  ), 0)
                    THEN RAISE(ABORT, 'tien_dich_vu phai bang tong dong dich vu chua xoa') END;
END;

CREATE TRIGGER trg_ctdp_after_checkout_start_cleaning
    AFTER UPDATE OF checkout_thuc_te ON CHI_TIET_DAT_PHONG
    WHEN OLD.checkout_thuc_te IS NULL AND NEW.checkout_thuc_te IS NOT NULL
BEGIN
    UPDATE PHONG
    SET can_don_dep = 1,
        don_xong_luc = datetime(NEW.checkout_thuc_te, '+' || COALESCE((SELECT CAST(gia_tri AS INTEGER) FROM CAU_HINH WHERE ten_tham_so='thoi_gian_don_phut'), 30) || ' minutes')
    WHERE id = NEW.phong_id;
END;

CREATE TRIGGER trg_ctdp_after_checkin_set_booking_state
    AFTER UPDATE OF checkin_thuc_te ON CHI_TIET_DAT_PHONG
    WHEN OLD.checkin_thuc_te IS NULL AND NEW.checkin_thuc_te IS NOT NULL
BEGIN
    UPDATE DAT_PHONG SET trang_thai = 'DANG_O'
    WHERE id = NEW.dat_phong_id AND trang_thai = 'DA_DAT';
END;

CREATE TRIGGER trg_ctdp_checkin_immutable
    BEFORE UPDATE OF checkin_thuc_te ON CHI_TIET_DAT_PHONG
    WHEN OLD.checkin_thuc_te IS NOT NULL AND NEW.checkin_thuc_te IS NOT OLD.checkin_thuc_te
BEGIN
    SELECT RAISE(ABORT, 'checkin_thuc_te da ghi thi khong duoc sua/xoa');
END;

CREATE TRIGGER trg_ctdp_checkout_once
    BEFORE UPDATE OF checkout_thuc_te ON CHI_TIET_DAT_PHONG
    WHEN OLD.checkout_thuc_te IS NOT NULL AND NEW.checkout_thuc_te IS NOT OLD.checkout_thuc_te
BEGIN
    SELECT RAISE(ABORT, 'checkout_thuc_te da chot thi khong duoc sua/xoa');
END;

CREATE TRIGGER trg_ctdp_checkout_insert_guard
    BEFORE INSERT ON CHI_TIET_DAT_PHONG
    WHEN NEW.checkout_thuc_te IS NOT NULL
BEGIN
    SELECT CASE WHEN NEW.checkin_thuc_te IS NULL
                    THEN RAISE(ABORT, 'Check-out chi hop le khi da check-in') END;
    SELECT CASE WHEN datetime(NEW.checkout_thuc_te) <= datetime(NEW.checkin_thuc_te)
                    THEN RAISE(ABORT, 'Thoi diem check-out phai lon hon check-in') END;
END;

CREATE TRIGGER trg_ctdp_checkout_guard
    BEFORE UPDATE OF checkout_thuc_te ON CHI_TIET_DAT_PHONG
    WHEN OLD.checkout_thuc_te IS NULL AND NEW.checkout_thuc_te IS NOT NULL
BEGIN
    SELECT CASE WHEN OLD.checkin_thuc_te IS NULL OR OLD.huy_luc IS NOT NULL
                    THEN RAISE(ABORT, 'Chi check-out dong da check-in va chua bi huy') END;
    SELECT CASE WHEN datetime(NEW.checkout_thuc_te) <= datetime(OLD.checkin_thuc_te)
                    THEN RAISE(ABORT, 'Thoi diem check-out phai lon hon check-in') END;
    SELECT CASE WHEN NEW.tien_phong IS NULL OR NEW.tien_dich_vu IS NULL
                    THEN RAISE(ABORT, 'Check-out phai chot ca tien_phong va tien_dich_vu') END;
END;

CREATE TRIGGER trg_ctdp_no_show_guard
    BEFORE UPDATE OF huy_luc ON CHI_TIET_DAT_PHONG
    WHEN OLD.huy_luc IS NULL AND NEW.huy_luc IS NOT NULL
BEGIN
    SELECT CASE WHEN (SELECT trang_thai FROM DAT_PHONG WHERE id = NEW.dat_phong_id) <> 'DANG_O'
                    THEN RAISE(ABORT, 'No-show tung dong chi ap dung khi DAT_PHONG DANG_O') END;
    SELECT CASE WHEN OLD.checkin_thuc_te IS NOT NULL OR OLD.checkout_thuc_te IS NOT NULL OR NEW.phong_id IS NOT NULL
                    THEN RAISE(ABORT, 'Chi dong chua check-in moi duoc no-show; dong no-show phai bo phong_id') END;
    SELECT CASE WHEN NEW.coc_giu_lai > (SELECT tien_coc FROM DAT_PHONG WHERE id = NEW.dat_phong_id)
                    THEN RAISE(ABORT, 'Coc giu lai vuot coc dat phong') END;
    SELECT CASE WHEN NEW.coc_giu_lai + COALESCE((
                                                    SELECT SUM(c.coc_giu_lai) FROM CHI_TIET_DAT_PHONG c
                                                    WHERE c.dat_phong_id = NEW.dat_phong_id AND c.id <> NEW.id
                                                ), 0) > (SELECT tien_coc FROM DAT_PHONG WHERE id = NEW.dat_phong_id)
                    THEN RAISE(ABORT, 'Tong coc giu cac dong vuot tien_coc') END;
    SELECT CASE WHEN NEW.coc_giu_lai <> (
        SELECT CASE
                   WHEN d.tien_coc = 0 OR COALESCE((SELECT SUM(x.gia_moi_dem) FROM CHI_TIET_DAT_PHONG x WHERE x.dat_phong_id = d.id), 0) = 0 THEN 0
                   WHEN NEW.id = (SELECT MAX(x.id) FROM CHI_TIET_DAT_PHONG x WHERE x.dat_phong_id = d.id) THEN
                       d.tien_coc - COALESCE((
                                                 SELECT SUM(CAST(d.tien_coc * x.gia_moi_dem / (SELECT SUM(y.gia_moi_dem) FROM CHI_TIET_DAT_PHONG y WHERE y.dat_phong_id = d.id) AS INTEGER))
                                                 FROM CHI_TIET_DAT_PHONG x
                                                 WHERE x.dat_phong_id = d.id AND x.id <> NEW.id
                                             ), 0)
                   ELSE CAST(d.tien_coc * NEW.gia_moi_dem / (SELECT SUM(y.gia_moi_dem) FROM CHI_TIET_DAT_PHONG y WHERE y.dat_phong_id = d.id) AS INTEGER)
                   END
        FROM DAT_PHONG d WHERE d.id = NEW.dat_phong_id
    )
                    THEN RAISE(ABORT, 'coc_giu_lai khong dung cach phan bo va lam tron CP-19') END;
    SELECT CASE WHEN (SELECT thoi_diem_coc FROM DAT_PHONG WHERE id = NEW.dat_phong_id) IS NULL AND NEW.coc_giu_lai <> 0
                    THEN RAISE(ABORT, 'Dat phong chua thu coc khong duoc giu coc') END;
    SELECT CASE WHEN datetime(NEW.huy_luc) < datetime(COALESCE(
            (SELECT gio_giu_phong_den FROM DAT_PHONG WHERE id = NEW.dat_phong_id),
            date((SELECT ngay_den FROM DAT_PHONG WHERE id = NEW.dat_phong_id)) || ' 23:59:59'
                                                      ))
                    THEN RAISE(ABORT, 'Chua den moc no-show') END;
END;

CREATE TRIGGER trg_ctdp_no_show_immutable
    BEFORE UPDATE OF huy_luc ON CHI_TIET_DAT_PHONG
    WHEN OLD.huy_luc IS NOT NULL AND NEW.huy_luc IS NOT OLD.huy_luc
BEGIN
    SELECT RAISE(ABORT, 'huy_luc da ghi thi khong duoc sua/xoa');
END;

CREATE TRIGGER trg_ctdp_price_identity_immutable
    BEFORE UPDATE OF dat_phong_id, loai_phong_id, gia_moi_dem, gia_moi_gio ON CHI_TIET_DAT_PHONG
    WHEN NEW.dat_phong_id <> OLD.dat_phong_id
        OR NEW.loai_phong_id <> OLD.loai_phong_id
        OR NEW.gia_moi_dem <> OLD.gia_moi_dem
        OR NEW.gia_moi_gio <> OLD.gia_moi_gio
BEGIN
    SELECT RAISE(ABORT, 'Thong tin dong dat phong va gia da chup khong duoc sua');
END;

CREATE TRIGGER trg_ctdp_money_immutable_after_checkout
    BEFORE UPDATE OF tien_phong, tien_dich_vu ON CHI_TIET_DAT_PHONG
    WHEN OLD.checkout_thuc_te IS NOT NULL
        AND (NEW.tien_phong <> OLD.tien_phong OR NEW.tien_dich_vu <> OLD.tien_dich_vu)
BEGIN
    SELECT RAISE(ABORT, 'Tien phong/dich vu da chot khong duoc sua');
END;

CREATE TRIGGER trg_ctdp_delete_forbidden
    BEFORE DELETE ON CHI_TIET_DAT_PHONG
BEGIN
    SELECT RAISE(ABORT, 'Khong xoa vat ly CHI_TIET_DAT_PHONG; giu lich su nghiep vu');
END;

CREATE TRIGGER trg_khop_insert_only_while_staying
    BEFORE INSERT ON KHACH_O_PHONG
    WHEN NOT EXISTS (
        SELECT 1 FROM CHI_TIET_DAT_PHONG c
        WHERE c.id = NEW.chi_tiet_dat_phong_id
          AND c.checkin_thuc_te IS NOT NULL AND c.checkout_thuc_te IS NULL AND c.huy_luc IS NULL
    )
BEGIN
    SELECT RAISE(ABORT, 'Chi gan khach vao dong phong dang o');
END;

CREATE TRIGGER trg_khop_update_forbidden
    BEFORE UPDATE ON KHACH_O_PHONG
BEGIN
    SELECT RAISE(ABORT, 'Khong sua lien ket lich su khach o phong');
END;

CREATE TRIGGER trg_khop_delete_forbidden
    BEFORE DELETE ON KHACH_O_PHONG
BEGIN
    SELECT RAISE(ABORT, 'Khong xoa vat ly lich su khach o phong');
END;

-- ---------------------------
-- 10. TRIGGERS: SERVICE DETAILS
-- ---------------------------
CREATE TRIGGER trg_dich_vu_identity_immutable_after_use
    BEFORE UPDATE OF ten, don_vi ON DICH_VU
    WHEN (NEW.ten IS NOT OLD.ten OR NEW.don_vi IS NOT OLD.don_vi)
        AND EXISTS (SELECT 1 FROM CHI_TIET_DICH_VU s WHERE s.dich_vu_id = OLD.id)
BEGIN
    SELECT RAISE(ABORT, 'Khong doi ten/don_vi dich vu khi da co lich su su dung');
END;

CREATE TRIGGER trg_ctdv_insert_guard
    BEFORE INSERT ON CHI_TIET_DICH_VU
BEGIN
    SELECT CASE WHEN NOT EXISTS (
        SELECT 1 FROM CHI_TIET_DAT_PHONG c
        WHERE c.id = NEW.chi_tiet_dat_phong_id
          AND c.checkin_thuc_te IS NOT NULL
          AND c.checkout_thuc_te IS NULL
          AND c.huy_luc IS NULL
    )
                    THEN RAISE(ABORT, 'Chi goi dich vu cho dong phong DANG_O') END;
    SELECT CASE WHEN NOT EXISTS (
        SELECT 1 FROM DICH_VU d WHERE d.id = NEW.dich_vu_id AND d.dang_ban = 1
    )
                    THEN RAISE(ABORT, 'Dich vu khong ton tai hoac da ngung ban') END;
    SELECT CASE WHEN datetime(NEW.thoi_diem) < (
        SELECT datetime(c.checkin_thuc_te) FROM CHI_TIET_DAT_PHONG c WHERE c.id = NEW.chi_tiet_dat_phong_id
    )
                    THEN RAISE(ABORT, 'Thoi diem goi dich vu khong duoc truoc check-in') END;
    SELECT CASE WHEN NEW.tai_khoan_id IS NULL OR NOT EXISTS (
        SELECT 1 FROM TAI_KHOAN t WHERE t.id = NEW.tai_khoan_id AND t.dang_hoat_dong = 1 AND t.vai_tro IN ('CHU', 'NHAN_VIEN')
    )
                    THEN RAISE(ABORT, 'Tao dich vu phai do tai khoan Nhan vien/Chu dang hoat dong thuc hien') END;
END;

CREATE TRIGGER trg_ctdv_update_only_soft_delete
    BEFORE UPDATE ON CHI_TIET_DICH_VU
BEGIN
    SELECT CASE WHEN OLD.da_xoa = 1
                    THEN RAISE(ABORT, 'Dong dich vu da xoa mem khong duoc sua tiep') END;
    SELECT CASE WHEN EXISTS (
        SELECT 1 FROM CHI_TIET_DAT_PHONG c
        WHERE c.id = OLD.chi_tiet_dat_phong_id AND c.checkout_thuc_te IS NOT NULL
    )
                    THEN RAISE(ABORT, 'Khong sua dich vu sau khi phong da checkout') END;
    SELECT CASE WHEN NEW.id <> OLD.id
        OR NEW.chi_tiet_dat_phong_id <> OLD.chi_tiet_dat_phong_id
        OR NEW.dich_vu_id <> OLD.dich_vu_id
        OR NEW.ten_dich_vu_luc_goi <> OLD.ten_dich_vu_luc_goi
        OR NEW.don_vi_luc_goi IS NOT OLD.don_vi_luc_goi
        OR NEW.so_luong <> OLD.so_luong
        OR NEW.don_gia <> OLD.don_gia
        OR NEW.thoi_diem <> OLD.thoi_diem
        OR NEW.tai_khoan_id IS NOT OLD.tai_khoan_id
        OR NEW.da_xoa <> 1
                    THEN RAISE(ABORT, 'Chi duoc xoa mem mot dong dich vu chua xoa; khong sua gia/so luong') END;
    SELECT CASE WHEN NOT EXISTS (
        SELECT 1 FROM TAI_KHOAN t WHERE t.id = NEW.tai_khoan_xoa_id AND t.dang_hoat_dong = 1 AND t.vai_tro IN ('CHU', 'NHAN_VIEN')
    )
                    THEN RAISE(ABORT, 'Tai khoan xoa dich vu phai la CHU/NHAN_VIEN dang hoat dong') END;
END;

CREATE TRIGGER trg_ctdv_delete_forbidden
    BEFORE DELETE ON CHI_TIET_DICH_VU
BEGIN
    SELECT RAISE(ABORT, 'Khong xoa vat ly CHI_TIET_DICH_VU; su dung xoa mem');
END;

-- ---------------------------
-- 11. TRIGGERS: INVOICE
-- ---------------------------
CREATE TRIGGER trg_hoa_don_insert_guard
    BEFORE INSERT ON HOA_DON
BEGIN
    SELECT CASE WHEN (SELECT trang_thai FROM DAT_PHONG WHERE id = NEW.dat_phong_id) <> 'DANG_O'
                    THEN RAISE(ABORT, 'Chi lap HOA_DON cho DAT_PHONG DANG_O') END;
    SELECT CASE WHEN NEW.so_tien_con_lai > 0
        AND (NEW.trang_thai <> 'CHO_THANH_TOAN' OR NEW.thoi_diem_thanh_toan IS NOT NULL)
                    THEN RAISE(ABORT, 'Hoa don co so du duong phai duoc tao CHO_THANH_TOAN; chi CO-05 moi duoc tat toan') END;
    SELECT CASE WHEN NOT EXISTS (
        SELECT 1 FROM CHI_TIET_DAT_PHONG c
        WHERE c.dat_phong_id = NEW.dat_phong_id AND c.checkin_thuc_te IS NOT NULL
    )
                    THEN RAISE(ABORT, 'Khong lap hoa don neu chua co dong nao check-in') END;
    SELECT CASE WHEN EXISTS (
        SELECT 1 FROM CHI_TIET_DAT_PHONG c
        WHERE c.dat_phong_id = NEW.dat_phong_id
          AND NOT (
            (c.checkin_thuc_te IS NOT NULL AND c.checkout_thuc_te IS NOT NULL AND c.huy_luc IS NULL
                AND c.tien_phong IS NOT NULL AND c.tien_dich_vu IS NOT NULL) OR
            (c.checkin_thuc_te IS NULL AND c.checkout_thuc_te IS NULL AND c.huy_luc IS NOT NULL)
            )
    )
                    THEN RAISE(ABORT, 'Con dong phong chua ket thuc hop le; chua du dieu kien lap hoa don') END;
    SELECT CASE WHEN NEW.tong_tien_phong <> COALESCE((
                                                         SELECT SUM(c.tien_phong) FROM CHI_TIET_DAT_PHONG c
                                                         WHERE c.dat_phong_id = NEW.dat_phong_id AND c.checkout_thuc_te IS NOT NULL
                                                     ), 0)
                    THEN RAISE(ABORT, 'tong_tien_phong khong khop tong tien_phong dong da checkout') END;
    SELECT CASE WHEN NEW.tong_tien_dich_vu <> COALESCE((
                                                           SELECT SUM(c.tien_dich_vu) FROM CHI_TIET_DAT_PHONG c
                                                           WHERE c.dat_phong_id = NEW.dat_phong_id AND c.checkout_thuc_te IS NOT NULL
                                                       ), 0)
                    THEN RAISE(ABORT, 'tong_tien_dich_vu khong khop tong tien_dich_vu dong da checkout') END;
    SELECT CASE WHEN EXISTS (
        SELECT 1 FROM CHI_TIET_DAT_PHONG c
        WHERE c.dat_phong_id = NEW.dat_phong_id
          AND c.checkout_thuc_te IS NOT NULL
          AND c.tien_dich_vu <> COALESCE((
                                             SELECT SUM(s.so_luong * s.don_gia)
                                             FROM CHI_TIET_DICH_VU s
                                             WHERE s.chi_tiet_dat_phong_id = c.id AND s.da_xoa = 0
                                         ), 0)
    )
                    THEN RAISE(ABORT, 'tien_dich_vu tren dong phong khong khop chi tiet dich vu') END;
    SELECT CASE WHEN NEW.tien_coc_con_lai <> (
        (SELECT tien_coc FROM DAT_PHONG WHERE id = NEW.dat_phong_id) -
        COALESCE((SELECT SUM(c.coc_giu_lai) FROM CHI_TIET_DAT_PHONG c WHERE c.dat_phong_id = NEW.dat_phong_id AND c.huy_luc IS NOT NULL), 0)
        )
                    THEN RAISE(ABORT, 'tien_coc_con_lai khong khop tien_coc tru coc_giu_lai') END;
    SELECT CASE WHEN NEW.so_tien_con_lai <> NEW.tong_tien_phong + NEW.tong_tien_dich_vu - NEW.tien_coc_con_lai
                    THEN RAISE(ABORT, 'so_tien_con_lai sai cong thuc') END;
    SELECT CASE WHEN NEW.so_tien_con_lai < 0 AND NOT EXISTS (
        SELECT 1 FROM DAT_PHONG d
        WHERE d.id = NEW.dat_phong_id
          AND d.tien_coc_hoan = -NEW.so_tien_con_lai
          AND d.ma_hoan_coc IS NOT NULL
          AND d.thoi_diem_hoan_coc IS NOT NULL
          AND d.noi_dung_hoan_coc IS NOT NULL
    )
                    THEN RAISE(ABORT, 'Hoa don am phai co lenh hoan coc du da duoc tao trong cung giao dich') END;
    SELECT CASE WHEN NEW.so_tien_con_lai >= 0 AND EXISTS (
        SELECT 1 FROM DAT_PHONG d WHERE d.id = NEW.dat_phong_id AND d.tien_coc_hoan > 0
    )
                    THEN RAISE(ABORT, 'Khong duoc ghi nhan hoan coc du khi so du hoa don khong am') END;
END;

-- Việc tạo hóa đơn sẽ tự động đóng đặt chỗ (booking). Ứng dụng phải thực hiện chèn hóa đơn
-- trong phạm vi giao dịch CO-04 của mình; trigger này sẽ thực hiện chuyển đổi trạng thái tương ứng.
CREATE TRIGGER trg_hoa_don_after_insert_close_booking
    AFTER INSERT ON HOA_DON
BEGIN
    UPDATE DAT_PHONG SET trang_thai = 'DA_TRA' WHERE id = NEW.dat_phong_id AND trang_thai = 'DANG_O';
END;

CREATE TRIGGER trg_hoa_don_update_guard
    BEFORE UPDATE ON HOA_DON
BEGIN
    SELECT CASE WHEN NEW.id <> OLD.id
        OR NEW.ma_hoa_don <> OLD.ma_hoa_don
        OR NEW.dat_phong_id <> OLD.dat_phong_id
        OR NEW.thoi_diem_lap <> OLD.thoi_diem_lap
        OR NEW.tong_tien_phong <> OLD.tong_tien_phong
        OR NEW.tong_tien_dich_vu <> OLD.tong_tien_dich_vu
        OR NEW.tien_coc_con_lai <> OLD.tien_coc_con_lai
        OR NEW.so_tien_con_lai <> OLD.so_tien_con_lai
                    THEN RAISE(ABORT, 'Du lieu snapshot HOA_DON khong duoc sua') END;
    SELECT CASE WHEN NOT (
        OLD.trang_thai = 'CHO_THANH_TOAN'
            AND NEW.trang_thai = 'DA_THANH_TOAN'
            AND OLD.so_tien_con_lai > 0
            AND OLD.thoi_diem_thanh_toan IS NULL
            AND NEW.thoi_diem_thanh_toan IS NOT NULL
        )
                    THEN RAISE(ABORT, 'Chi duoc chuyen hoa don CHO_THANH_TOAN sang DA_THANH_TOAN mot lan') END;
END;

CREATE TRIGGER trg_hoa_don_delete_forbidden
    BEFORE DELETE ON HOA_DON
BEGIN
    SELECT RAISE(ABORT, 'Khong xoa vat ly HOA_DON; giu lich su tai chinh');
END;

-- ---------------------------
-- 12. TRIGGERS: Kiểm soát khách không đến nhận phòng và đảm bảo tính toàn vẹn của khoản tiền cọc giữ lại
-- ---------------------------
CREATE TRIGGER trg_ctdp_hold_update_guard
    BEFORE UPDATE OF coc_giu_lai ON CHI_TIET_DAT_PHONG
    WHEN NEW.coc_giu_lai <> OLD.coc_giu_lai
        AND NOT (OLD.huy_luc IS NULL AND NEW.huy_luc IS NOT NULL)
BEGIN
    SELECT RAISE(ABORT, 'coc_giu_lai chi duoc ghi cung su kien no-show lan dau');
END;

CREATE TRIGGER trg_dat_phong_no_refund_after_invoice_payment
    BEFORE UPDATE OF tien_coc_hoan ON DAT_PHONG
    WHEN NEW.tien_coc_hoan <> OLD.tien_coc_hoan
        AND OLD.ma_hoan_coc IS NOT NULL
BEGIN
    SELECT RAISE(ABORT, 'Dat phong da co lenh hoan; khong ghi nhan hoan them');
END;

-- Ngăn chặn việc xóa các hàng chính/hàng định danh vốn là một phần của lịch sử kiểm tra (audit history).
CREATE TRIGGER trg_khach_hang_delete_forbidden
    BEFORE DELETE ON KHACH_HANG
    WHEN EXISTS (SELECT 1 FROM DAT_PHONG d WHERE d.khach_hang_id = OLD.id)
        OR EXISTS (SELECT 1 FROM KHACH_O_PHONG k WHERE k.khach_hang_id = OLD.id)
BEGIN
    SELECT RAISE(ABORT, 'Khong xoa khach hang da co lich su');
END;

CREATE TRIGGER trg_tai_khoan_username_immutable_after_audit
    BEFORE UPDATE OF ten_dang_nhap ON TAI_KHOAN
    WHEN NEW.ten_dang_nhap <> OLD.ten_dang_nhap
        AND EXISTS (SELECT 1 FROM CHI_TIET_DICH_VU s WHERE s.tai_khoan_id = OLD.id OR s.tai_khoan_xoa_id = OLD.id)
BEGIN
    SELECT RAISE(ABORT, 'Tai khoan da co lich su thao tac; khong doi ten dang nhap');
END;

CREATE TRIGGER trg_tai_khoan_delete_forbidden
    BEFORE DELETE ON TAI_KHOAN
    WHEN EXISTS (SELECT 1 FROM CHI_TIET_DICH_VU c WHERE c.tai_khoan_id = OLD.id OR c.tai_khoan_xoa_id = OLD.id)
BEGIN
    SELECT RAISE(ABORT, 'Khong xoa tai khoan co lich su; hay khoa tai khoan');
END;

INSERT OR IGNORE INTO CAU_HINH(ten_tham_so, gia_tri) VALUES
                                                         ('gio_nhan_chuan', '14:00'),
                                                         ('gio_tra_chuan', '12:00'),
                                                         ('so_dem_coc', '1'),
                                                         ('thoi_han_coc_phut', '10'),
                                                         ('thoi_gian_don_phut', '30');

INSERT OR IGNORE INTO CHINH_SACH_HUY(so_gio_toi_thieu, ty_le_hoan) VALUES
                                                                       (0, 0), (24, 50), (48, 100);

PRAGMA user_version = 3;
COMMIT;

