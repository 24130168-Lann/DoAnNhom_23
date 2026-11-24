
-- 1. dữ liệu loại phòng
INSERT OR IGNORE INTO LOAI_PHONG
    (ten_loai, gia_theo_gio, gia_theo_ngay)
VALUES
    ('Phòng thường', 50000, 300000),
    ('Phòng đôi', 70000, 450000),
    ('Phòng tầm trung', 110000, 700000),
    ('Phòng VIP', 300000, 2000000);

--2. dữ liệu dịch vụ
INSERT OR IGNORE INTO DICH_VU
(ten, don_gia, don_vi, dang_ban)
VALUES
    ('Nước suối', 10000, 'chai', 1),
    ('Nước ngọt', 15000, 'lon', 1),
    ('Bia', 25000, 'lon', 1),
    ('Cà phê', 30000, 'ly', 1),
    ('Mì ly', 20000, 'phần', 1),
    ('Ăn sáng', 50000, 'phần', 1),
    ('Giặt ủi', 30000, 'kg', 1),
    ('Thuê xe máy', 150000, 'ngày', 1);


-- 3. dữ liệu cấu hình
INSERT OR IGNORE INTO CAU_HINH
(ten_tham_so, gia_tri)
VALUES
    ('gio_nhan_chuan', '14:00'),
    ('gio_tra_chuan', '12:00'),
    ('so_dem_coc', '1'),
    ('thoi_han_coc_phut', '10'),
    ('thoi_gian_don_phut', '30');

-- 4. dữ liệu chính sách huỷ
INSERT OR IGNORE INTO CHINH_SACH_HUY
(so_gio_toi_thieu, ty_le_hoan)
VALUES
    (48, 100),
    (24, 50),
    (0, 0);


-- 5. Dữ liệu phòng : 40 PHONG MAU
-- Ca hai tang deu co phong theo ngay va theo gio
-- Moi loai: 5 phong THEO_NGAY + 5 phong THEO_GIO


INSERT OR IGNORE INTO PHONG
(so_phong, tang, loai_phong_id, che_do)
VALUES


-- PHONG THUONG
-- Theo ngay: 3 tang 1, 2 tang 2
('101', 1, (SELECT id FROM LOAI_PHONG WHERE ten_loai = 'Phòng thường'), 'THEO_NGAY'),
('102', 1, (SELECT id FROM LOAI_PHONG WHERE ten_loai = 'Phòng thường'), 'THEO_NGAY'),
('201', 2, (SELECT id FROM LOAI_PHONG WHERE ten_loai = 'Phòng thường'), 'THEO_NGAY'),
('202', 2, (SELECT id FROM LOAI_PHONG WHERE ten_loai = 'Phòng thường'), 'THEO_NGAY'),
('103', 1, (SELECT id FROM LOAI_PHONG WHERE ten_loai = 'Phòng thường'), 'THEO_NGAY');

-- Theo gio: 2 tang 1, 3 tang 2
INSERT OR IGNORE INTO PHONG
(so_phong, tang, loai_phong_id, che_do)
VALUES
    ('104', 1, (SELECT id FROM LOAI_PHONG WHERE ten_loai = 'Phòng thường'), 'THEO_GIO'),
    ('203', 2, (SELECT id FROM LOAI_PHONG WHERE ten_loai = 'Phòng thường'), 'THEO_GIO'),
    ('204', 2, (SELECT id FROM LOAI_PHONG WHERE ten_loai = 'Phòng thường'), 'THEO_GIO'),
    ('205', 2, (SELECT id FROM LOAI_PHONG WHERE ten_loai = 'Phòng thường'), 'THEO_GIO'),
    ('105', 1, (SELECT id FROM LOAI_PHONG WHERE ten_loai = 'Phòng thường'), 'THEO_GIO');


-- PHONG DOI


INSERT OR IGNORE INTO PHONG
(so_phong, tang, loai_phong_id, che_do)
VALUES
    ('106', 1, (SELECT id FROM LOAI_PHONG WHERE ten_loai = 'Phòng đôi'), 'THEO_NGAY'),
    ('107', 1, (SELECT id FROM LOAI_PHONG WHERE ten_loai = 'Phòng đôi'), 'THEO_NGAY'),
    ('206', 2, (SELECT id FROM LOAI_PHONG WHERE ten_loai = 'Phòng đôi'), 'THEO_NGAY'),
    ('207', 2, (SELECT id FROM LOAI_PHONG WHERE ten_loai = 'Phòng đôi'), 'THEO_NGAY'),
    ('108', 1, (SELECT id FROM LOAI_PHONG WHERE ten_loai = 'Phòng đôi'), 'THEO_NGAY');

INSERT OR IGNORE INTO PHONG
(so_phong, tang, loai_phong_id, che_do)
VALUES
    ('109', 1, (SELECT id FROM LOAI_PHONG WHERE ten_loai = 'Phòng đôi'), 'THEO_GIO'),
    ('208', 2, (SELECT id FROM LOAI_PHONG WHERE ten_loai = 'Phòng đôi'), 'THEO_GIO'),
    ('209', 2, (SELECT id FROM LOAI_PHONG WHERE ten_loai = 'Phòng đôi'), 'THEO_GIO'),
    ('210', 2, (SELECT id FROM LOAI_PHONG WHERE ten_loai = 'Phòng đôi'), 'THEO_GIO'),
    ('110', 1, (SELECT id FROM LOAI_PHONG WHERE ten_loai = 'Phòng đôi'), 'THEO_GIO');


-- PHONG TAM TRUNG
INSERT OR IGNORE INTO PHONG
(so_phong, tang, loai_phong_id, che_do)
VALUES
    ('111', 1, (SELECT id FROM LOAI_PHONG WHERE ten_loai = 'Phòng tầm trung'), 'THEO_NGAY'),
    ('112', 1, (SELECT id FROM LOAI_PHONG WHERE ten_loai = 'Phòng tầm trung'), 'THEO_NGAY'),
    ('211', 2, (SELECT id FROM LOAI_PHONG WHERE ten_loai = 'Phòng tầm trung'), 'THEO_NGAY'),
    ('212', 2, (SELECT id FROM LOAI_PHONG WHERE ten_loai = 'Phòng tầm trung'), 'THEO_NGAY'),
    ('113', 1, (SELECT id FROM LOAI_PHONG WHERE ten_loai = 'Phòng tầm trung'), 'THEO_NGAY');

INSERT OR IGNORE INTO PHONG
(so_phong, tang, loai_phong_id, che_do)
VALUES
    ('114', 1, (SELECT id FROM LOAI_PHONG WHERE ten_loai = 'Phòng tầm trung'), 'THEO_GIO'),
    ('213', 2, (SELECT id FROM LOAI_PHONG WHERE ten_loai = 'Phòng tầm trung'), 'THEO_GIO'),
    ('214', 2, (SELECT id FROM LOAI_PHONG WHERE ten_loai = 'Phòng tầm trung'), 'THEO_GIO'),
    ('215', 2, (SELECT id FROM LOAI_PHONG WHERE ten_loai = 'Phòng tầm trung'), 'THEO_GIO'),
    ('115', 1, (SELECT id FROM LOAI_PHONG WHERE ten_loai = 'Phòng tầm trung'), 'THEO_GIO');


-- PHONG VIP
INSERT OR IGNORE INTO PHONG
(so_phong, tang, loai_phong_id, che_do)
VALUES
    ('116', 1, (SELECT id FROM LOAI_PHONG WHERE ten_loai = 'Phòng VIP'), 'THEO_NGAY'),
    ('117', 1, (SELECT id FROM LOAI_PHONG WHERE ten_loai = 'Phòng VIP'), 'THEO_NGAY'),
    ('216', 2, (SELECT id FROM LOAI_PHONG WHERE ten_loai = 'Phòng VIP'), 'THEO_NGAY'),
    ('217', 2, (SELECT id FROM LOAI_PHONG WHERE ten_loai = 'Phòng VIP'), 'THEO_NGAY'),
    ('118', 1, (SELECT id FROM LOAI_PHONG WHERE ten_loai = 'Phòng VIP'), 'THEO_NGAY');

INSERT OR IGNORE INTO PHONG
(so_phong, tang, loai_phong_id, che_do)
VALUES
    ('119', 1, (SELECT id FROM LOAI_PHONG WHERE ten_loai = 'Phòng VIP'), 'THEO_GIO'),
    ('218', 2, (SELECT id FROM LOAI_PHONG WHERE ten_loai = 'Phòng VIP'), 'THEO_GIO'),
    ('219', 2, (SELECT id FROM LOAI_PHONG WHERE ten_loai = 'Phòng VIP'), 'THEO_GIO'),
    ('220', 2, (SELECT id FROM LOAI_PHONG WHERE ten_loai = 'Phòng VIP'), 'THEO_GIO'),
    ('120', 1, (SELECT id FROM LOAI_PHONG WHERE ten_loai = 'Phòng VIP'), 'THEO_GIO');

/*
  DU LIEU MAU NGHIEP VU - KHACH SAN
  Phu hop schema V3 duoc cung cap trong cuoc tro chuyen.

  Pham vi:
  - Lich su: 01/09/2026 den 31/10/2026 (30 dat phong, 35 chi tiet).
  - Van hanh demo: ngay 24/11/2026 (8 phong dang o, 4 phong da dat,
    1 phong dang don, 27 phong trong/san sang).

  Dieu kien:
  - Da chay schema.sql va data.sql (loai phong, dich vu, cau hinh, 40 phong,
    8 khach hang dau tien).
  - Cac bang DAT_PHONG, CHI_TIET_DAT_PHONG, KHACH_O_PHONG,
    CHI_TIET_DICH_VU, HOA_DON dang trong.
  - Chi chay mot lan tren database demo sach. Khong chay lai tren database
    da co giao dich, vi cac bang lich su co trigger cam xoa.

  Tai khoan nhan vien duoi day chi de gan tai_khoan_id cho du lieu dich vu.
  mat_khau_bam la chuoi danh dau, KHONG phai mat khau dang nhap hop le.
  Thong tin tai khoan hoan coc la du lieu gia lap: ten chu tai khoan lay theo khach dat,
  so tai khoan duoc tao rieng theo ID khach hang; khong phai thong tin ngan hang that.
*/
PRAGMA foreign_keys = ON;
PRAGMA recursive_triggers = ON;
BEGIN IMMEDIATE;

-- 0. Tao tai khoan thao tac dich vu.
-- Tai khoan chi phuc vu du lieu mau; khong dung de dang nhap.

INSERT INTO TAI_KHOAN (ten_dang_nhap, mat_khau_bam, vai_tro, dang_hoat_dong)
SELECT 'nhanvien_du_lieu_mau', 'SEED_ONLY_NOT_FOR_LOGIN', 'NHAN_VIEN', 1
WHERE NOT EXISTS (
    SELECT 1 FROM TAI_KHOAN WHERE ten_dang_nhap = 'nhanvien_du_lieu_mau'
);

-- Bo sung 16 khach hang, tong cong 24 khach hang mau.

INSERT OR IGNORE INTO KHACH_HANG (ho_ten, so_dien_thoai, loai_giay_to, so_giay_to) VALUES ('Nguyễn Đức Minh', '0900000009', 'CCCD', '079000000009');

INSERT OR IGNORE INTO KHACH_HANG (ho_ten, so_dien_thoai, loai_giay_to, so_giay_to) VALUES ('Trần Mỹ Duyên', '0900000010', 'CCCD', '079000000010');

INSERT OR IGNORE INTO KHACH_HANG (ho_ten, so_dien_thoai, loai_giay_to, so_giay_to) VALUES ('Lê Quốc Thịnh', '0900000011', 'CCCD', '079000000011');

INSERT OR IGNORE INTO KHACH_HANG (ho_ten, so_dien_thoai, loai_giay_to, so_giay_to) VALUES ('Phạm Ngọc Mai', '0900000012', 'CCCD', '079000000012');

INSERT OR IGNORE INTO KHACH_HANG (ho_ten, so_dien_thoai, loai_giay_to, so_giay_to) VALUES ('Võ Thanh Tùng', '0900000013', 'CCCD', '079000000013');

INSERT OR IGNORE INTO KHACH_HANG (ho_ten, so_dien_thoai, loai_giay_to, so_giay_to) VALUES ('Đặng Thảo Nhi', '0900000014', 'CCCD', '079000000014');

INSERT OR IGNORE INTO KHACH_HANG (ho_ten, so_dien_thoai, loai_giay_to, so_giay_to) VALUES ('Bùi Anh Khoa', '0900000015', 'CCCD', '079000000015');

INSERT OR IGNORE INTO KHACH_HANG (ho_ten, so_dien_thoai, loai_giay_to, so_giay_to) VALUES ('Đỗ Phương Thảo', '0900000016', 'CCCD', '079000000016');

INSERT OR IGNORE INTO KHACH_HANG (ho_ten, so_dien_thoai, loai_giay_to, so_giay_to) VALUES ('Hoàng Gia Bảo', '0900000017', 'CCCD', '079000000017');

INSERT OR IGNORE INTO KHACH_HANG (ho_ten, so_dien_thoai, loai_giay_to, so_giay_to) VALUES ('Nguyễn Khánh An', '0900000018', 'CCCD', '079000000018');

INSERT OR IGNORE INTO KHACH_HANG (ho_ten, so_dien_thoai, loai_giay_to, so_giay_to) VALUES ('Trương Minh Khang', '0900000019', 'CCCD', '079000000019');

INSERT OR IGNORE INTO KHACH_HANG (ho_ten, so_dien_thoai, loai_giay_to, so_giay_to) VALUES ('Phan Bảo Ngọc', '0900000020', 'CCCD', '079000000020');

INSERT OR IGNORE INTO KHACH_HANG (ho_ten, so_dien_thoai, loai_giay_to, so_giay_to) VALUES ('Đinh Tuấn Kiệt', '0900000021', 'CCCD', '079000000021');

INSERT OR IGNORE INTO KHACH_HANG (ho_ten, so_dien_thoai, loai_giay_to, so_giay_to) VALUES ('Lý Thanh Hằng', '0900000022', 'CCCD', '079000000022');

INSERT OR IGNORE INTO KHACH_HANG (ho_ten, so_dien_thoai, loai_giay_to, so_giay_to) VALUES ('Mai Nhật Nam', '0900000023', 'CCCD', '079000000023');

INSERT OR IGNORE INTO KHACH_HANG (ho_ten, so_dien_thoai, loai_giay_to, so_giay_to) VALUES ('Cao Thu Trang', '0900000024', 'CCCD', '079000000024');

-- 1. Bang tam mo ta cac dat phong va chi tiet.
CREATE TEMP TABLE _seed_dp (
                               ma_dat_phong TEXT PRIMARY KEY,
                               so_dien_thoai TEXT NOT NULL,
                               loai_giao_dich TEXT NOT NULL CHECK (loai_giao_dich IN ('DAT_TRUOC','VANG_LAI')),
                               ngay_den TEXT NOT NULL,
                               ngay_di TEXT NOT NULL,
                               han_coc TEXT,
                               thoi_diem_coc TEXT,
                               gio_giu_phong_den TEXT,
                               ly_do_huy TEXT,
                               thoi_diem_huy TEXT,
                               tien_coc_hoan INTEGER NOT NULL DEFAULT 0,
                               ma_hoan_coc TEXT,
                               thoi_diem_hoan_coc TEXT,
                               noi_dung_hoan_coc TEXT,
                               ghi_chu_huy TEXT
);

CREATE TEMP TABLE _seed_ct (
                               ma_dat_phong TEXT NOT NULL,
                               stt INTEGER NOT NULL,
                               ten_loai_phong TEXT NOT NULL,
                               so_phong TEXT,
                               checkin_thuc_te TEXT,
                               checkout_thuc_te TEXT,
                               PRIMARY KEY (ma_dat_phong, stt)
);

CREATE TEMP TABLE _seed_map (
                                ma_dat_phong TEXT NOT NULL,
                                stt INTEGER NOT NULL,
                                chi_tiet_id INTEGER NOT NULL,
                                so_phong TEXT,
                                checkin_thuc_te TEXT,
                                checkout_thuc_te TEXT,
                                PRIMARY KEY (ma_dat_phong, stt),
                                UNIQUE (chi_tiet_id)
);

-- Dat phong truoc: lich su + 10 dat phong thang 11.
INSERT INTO _seed_dp VALUES
                         ('DP-H09-001', '0900000001', 'DAT_TRUOC', '2026-09-02 14:00:00', '2026-09-04 12:00:00', '2026-08-31 10:00:00', '2026-08-31 09:00:00', NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL),
                         ('DP-H09-002', '0900000002', 'DAT_TRUOC', '2026-09-05 14:00:00', '2026-09-07 12:00:00', '2026-09-02 10:00:00', '2026-09-02 09:00:00', NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL),
                         ('DP-H09-003', '0900000003', 'DAT_TRUOC', '2026-09-08 14:00:00', '2026-09-10 12:00:00', '2026-09-05 10:00:00', '2026-09-05 09:00:00', NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL),
                         ('DP-H09-004', '0900000004', 'DAT_TRUOC', '2026-09-12 14:00:00', '2026-09-14 12:00:00', '2026-09-09 10:00:00', '2026-09-09 09:00:00', NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL),
                         ('DP-H09-005', '0900000005', 'DAT_TRUOC', '2026-09-15 14:00:00', '2026-09-17 12:00:00', '2026-09-12 10:00:00', '2026-09-12 09:00:00', NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL),
                         ('DP-H09-006', '0900000006', 'DAT_TRUOC', '2026-09-18 14:00:00', '2026-09-20 12:00:00', '2026-09-15 10:00:00', '2026-09-15 09:00:00', NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL),
                         ('DP-H09-007', '0900000007', 'DAT_TRUOC', '2026-09-21 14:00:00', '2026-09-23 12:00:00', '2026-09-18 10:00:00', '2026-09-18 09:00:00', NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL),
                         ('DP-H09-008', '0900000008', 'DAT_TRUOC', '2026-09-24 14:00:00', '2026-09-26 12:00:00', '2026-09-21 10:00:00', '2026-09-21 09:00:00', NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL),
                         ('DP-H09-009', '0900000009', 'DAT_TRUOC', '2026-09-27 14:00:00', '2026-09-29 12:00:00', '2026-09-24 10:00:00', '2026-09-24 09:00:00', NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL),
                         ('DP-H09-010', '0900000010', 'DAT_TRUOC', '2026-09-29 14:00:00', '2026-10-02 12:00:00', '2026-09-26 10:00:00', '2026-09-26 09:00:00', NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL),
                         ('DP-H10-001', '0900000011', 'DAT_TRUOC', '2026-10-03 14:00:00', '2026-10-05 12:00:00', '2026-09-30 10:00:00', '2026-09-30 09:00:00', NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL),
                         ('DP-H10-002', '0900000012', 'DAT_TRUOC', '2026-10-06 14:00:00', '2026-10-08 12:00:00', '2026-10-03 10:00:00', '2026-10-03 09:00:00', NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL),
                         ('DP-H10-003', '0900000013', 'DAT_TRUOC', '2026-10-09 14:00:00', '2026-10-11 12:00:00', '2026-10-06 10:00:00', '2026-10-06 09:00:00', NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL),
                         ('DP-H10-004', '0900000014', 'DAT_TRUOC', '2026-10-24 14:00:00', '2026-10-26 12:00:00', '2026-10-21 10:00:00', '2026-10-21 09:00:00', '2026-10-24 23:59:59', NULL, NULL, 0, NULL, NULL, NULL, NULL),
                         ('DP-H09-011', '0900000015', 'DAT_TRUOC', '2026-09-20 14:00:00', '2026-09-22 12:00:00', '2026-09-16 10:00:00', '2026-09-16 09:00:00', NULL, 'KHACH_HUY', '2026-09-17 10:00:00', 300000, 'REF-H09-011', '2026-09-17 10:05:00', 'Hoan coc dat phong DP-H09-011', NULL),
                         ('DP-H09-012', '0900000016', 'DAT_TRUOC', '2026-09-25 14:00:00', '2026-09-27 12:00:00', '2026-09-22 10:00:00', '2026-09-22 09:00:00', NULL, 'KHACH_HUY', '2026-09-24 02:00:00', 225000, 'REF-H09-012', '2026-09-24 02:05:00', 'Hoan 50 phan tram coc DP-H09-012', NULL),
                         ('DP-H10-005', '0900000017', 'DAT_TRUOC', '2026-10-15 14:00:00', '2026-10-17 12:00:00', '2026-10-12 10:00:00', '2026-10-12 09:00:00', NULL, 'KHACH_HUY', '2026-10-14 18:00:00', 0, NULL, NULL, NULL, NULL),
                         ('DP-H10-006', '0900000018', 'DAT_TRUOC', '2026-10-18 14:00:00', '2026-10-20 12:00:00', '2026-10-15 10:00:00', '2026-10-15 09:00:00', NULL, 'KHACH_SAN_HUY', '2026-10-15 11:00:00', 2000000, 'REF-H10-006', '2026-10-15 11:05:00', 'Hoan coc do khach san huy DP-H10-006', 'Khach san huy do phong khong dap ung yeu cau'),
                         ('DP-H10-007', '0900000019', 'DAT_TRUOC', '2026-10-22 14:00:00', '2026-10-24 12:00:00', '2026-10-19 10:00:00', '2026-10-19 09:00:00', NULL, 'BAT_KHA_KHANG', '2026-10-20 10:00:00', 300000, 'REF-H10-007', '2026-10-20 10:05:00', 'Hoan coc bat kha khang DP-H10-007', 'Khach khong the di chuyen do su kien bat kha khang'),
                         ('DP-H10-008', '0900000020', 'DAT_TRUOC', '2026-10-28 14:00:00', '2026-10-30 12:00:00', '2026-10-25 10:00:00', '2026-10-25 09:00:00', NULL, 'NO_SHOW', '2026-10-28 23:59:59', 0, NULL, NULL, NULL, NULL),
                         ('DP-TH11-001', '0900000001', 'DAT_TRUOC', '2026-11-23 14:00:00', '2026-11-25 12:00:00', '2026-11-22 10:00:00', '2026-11-22 09:00:00', NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL),
                         ('DP-TH11-002', '0900000002', 'DAT_TRUOC', '2026-11-23 14:00:00', '2026-11-25 12:00:00', '2026-11-22 10:05:00', '2026-11-22 09:05:00', NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL),
                         ('DP-TH11-003', '0900000003', 'DAT_TRUOC', '2026-11-23 14:00:00', '2026-11-25 12:00:00', '2026-11-22 10:10:00', '2026-11-22 09:10:00', NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL),
                         ('DP-TH11-004', '0900000004', 'DAT_TRUOC', '2026-11-23 14:00:00', '2026-11-25 12:00:00', '2026-11-22 10:15:00', '2026-11-22 09:15:00', NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL),
                         ('DP-TH11-005', '0900000005', 'DAT_TRUOC', '2026-11-23 14:00:00', '2026-11-25 12:00:00', '2026-11-22 10:20:00', '2026-11-22 09:20:00', NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL),
                         ('DP-TH11-006', '0900000006', 'DAT_TRUOC', '2026-11-23 14:00:00', '2026-11-25 12:00:00', '2026-11-22 10:25:00', '2026-11-22 09:25:00', NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL),
                         ('DP-TH11-007', '0900000007', 'DAT_TRUOC', '2026-11-25 14:00:00', '2026-11-27 12:00:00', '2026-11-24 10:00:00', '2026-11-24 09:00:00', NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL),
                         ('DP-TH11-008', '0900000008', 'DAT_TRUOC', '2026-11-25 14:00:00', '2026-11-27 12:00:00', '2026-11-24 10:05:00', '2026-11-24 09:05:00', NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL),
                         ('DP-TH11-009', '0900000009', 'DAT_TRUOC', '2026-11-25 14:00:00', '2026-11-27 12:00:00', '2026-11-24 10:10:00', '2026-11-24 09:10:00', NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL),
                         ('DP-TH11-010', '0900000010', 'DAT_TRUOC', '2026-11-25 14:00:00', '2026-11-27 12:00:00', '2026-11-24 10:15:00', '2026-11-24 09:15:00', NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL);

-- Chi tiet dat phong: 35 dong lich su + 10 dong dat truoc thang 11 + 3 dong vang lai thang 11.
INSERT INTO _seed_ct VALUES
                         ('DP-H09-001', 1, 'Phòng thường', '101', '2026-09-02 14:00:00', '2026-09-04 11:00:00'),
                         ('DP-H09-001', 2, 'Phòng thường', '102', '2026-09-02 14:05:00', '2026-09-04 11:05:00'),
                         ('DP-H09-002', 1, 'Phòng đôi', '106', '2026-09-05 14:00:00', '2026-09-07 11:00:00'),
                         ('DP-H09-002', 2, 'Phòng đôi', '107', '2026-09-05 14:05:00', '2026-09-07 11:05:00'),
                         ('DP-H09-003', 1, 'Phòng tầm trung', '111', '2026-09-08 14:00:00', '2026-09-10 11:00:00'),
                         ('DP-H09-003', 2, 'Phòng tầm trung', '112', '2026-09-08 14:05:00', '2026-09-10 11:05:00'),
                         ('DP-H09-004', 1, 'Phòng VIP', '116', '2026-09-12 14:00:00', '2026-09-14 11:00:00'),
                         ('DP-H09-004', 2, 'Phòng VIP', '117', '2026-09-12 14:05:00', '2026-09-14 11:05:00'),
                         ('DP-H09-005', 1, 'Phòng thường', '103', '2026-09-15 14:00:00', '2026-09-17 11:00:00'),
                         ('DP-H09-006', 1, 'Phòng đôi', '108', '2026-09-18 14:00:00', '2026-09-20 11:00:00'),
                         ('DP-H09-007', 1, 'Phòng tầm trung', '113', '2026-09-21 14:00:00', '2026-09-23 11:00:00'),
                         ('DP-H09-008', 1, 'Phòng VIP', '118', '2026-09-24 14:00:00', '2026-09-26 11:00:00'),
                         ('DP-H09-009', 1, 'Phòng thường', '201', '2026-09-27 14:00:00', '2026-09-29 11:00:00'),
                         ('DP-H09-010', 1, 'Phòng thường', '202', '2026-09-29 14:00:00', '2026-10-02 11:00:00'),
                         ('DP-H10-001', 1, 'Phòng đôi', '207', '2026-10-03 14:00:00', '2026-10-05 11:00:00'),
                         ('DP-H10-002', 1, 'Phòng tầm trung', '212', '2026-10-06 14:00:00', '2026-10-08 11:00:00'),
                         ('DP-H10-003', 1, 'Phòng VIP', '217', '2026-10-09 14:00:00', '2026-10-11 11:00:00'),
                         ('DP-H10-004', 1, 'Phòng đôi', '206', '2026-10-24 14:00:00', '2026-10-26 11:00:00'),
                         ('DP-H10-004', 2, 'Phòng đôi', NULL, NULL, NULL),
                         ('DP-H09-011', 1, 'Phòng thường', NULL, NULL, NULL),
                         ('DP-H09-012', 1, 'Phòng đôi', NULL, NULL, NULL),
                         ('DP-H10-005', 1, 'Phòng tầm trung', NULL, NULL, NULL),
                         ('DP-H10-006', 1, 'Phòng VIP', NULL, NULL, NULL),
                         ('DP-H10-007', 1, 'Phòng thường', NULL, NULL, NULL),
                         ('DP-H10-008', 1, 'Phòng thường', NULL, NULL, NULL),
                         ('WI-H09-001', 1, 'Phòng thường', '104', '2026-09-03 15:05:00', '2026-09-03 18:00:00'),
                         ('WI-H09-002', 1, 'Phòng đôi', '109', '2026-09-06 15:05:00', '2026-09-06 18:00:00'),
                         ('WI-H09-003', 1, 'Phòng tầm trung', '114', '2026-09-09 15:05:00', '2026-09-09 18:00:00'),
                         ('WI-H09-004', 1, 'Phòng VIP', '119', '2026-09-13 15:05:00', '2026-09-13 18:00:00'),
                         ('WI-H10-001', 1, 'Phòng thường', '105', '2026-10-04 14:05:00', '2026-10-04 17:00:00'),
                         ('WI-H10-002', 1, 'Phòng đôi', '110', '2026-10-07 15:05:00', '2026-10-07 18:00:00'),
                         ('WI-H10-003', 1, 'Phòng tầm trung', '115', '2026-10-12 16:05:00', '2026-10-12 19:00:00'),
                         ('WI-H10-004', 1, 'Phòng VIP', '120', '2026-10-20 14:05:00', '2026-10-20 17:00:00'),
                         ('WI-H09-005', 1, 'Phòng tầm trung', '211', '2026-09-16 14:05:00', '2026-09-17 12:00:00'),
                         ('WI-H10-005', 1, 'Phòng VIP', '216', '2026-10-13 14:05:00', '2026-10-14 12:00:00'),
                         ('DP-TH11-001', 1, 'Phòng thường', '101', '2026-11-23 14:10:00', NULL),
                         ('DP-TH11-002', 1, 'Phòng đôi', '106', '2026-11-23 14:15:00', NULL),
                         ('DP-TH11-003', 1, 'Phòng tầm trung', '111', '2026-11-23 14:20:00', NULL),
                         ('DP-TH11-004', 1, 'Phòng VIP', '116', '2026-11-23 14:25:00', NULL),
                         ('DP-TH11-005', 1, 'Phòng thường', '201', '2026-11-23 14:30:00', NULL),
                         ('DP-TH11-006', 1, 'Phòng đôi', '206', '2026-11-23 14:35:00', NULL),
                         ('DP-TH11-007', 1, 'Phòng thường', '102', NULL, NULL),
                         ('DP-TH11-008', 1, 'Phòng đôi', '107', NULL, NULL),
                         ('DP-TH11-009', 1, 'Phòng tầm trung', '112', NULL, NULL),
                         ('DP-TH11-010', 1, 'Phòng VIP', '117', NULL, NULL),
                         ('WI-TH11-001', 1, 'Phòng thường', '104', '2026-11-24 15:05:00', NULL),
                         ('WI-TH11-002', 1, 'Phòng đôi', '109', '2026-11-24 15:35:00', NULL),
                         ('WI-TH11-003', 1, 'Phòng thường', '105', '2026-11-24 14:35:00', '2026-11-24 17:30:00');
-- 2. Tao tung dat truoc lich su va xac nhan coc ngay sau khi tao.
-- Lam lan luot de khong vi pham trigger mot so dien thoai chi co mot CHO_COC.

-- DP-H09-001
INSERT INTO DAT_PHONG (
    ma_dat_phong, khach_hang_id, la_vang_lai, ngay_den, ngay_di,
    tien_coc, han_thanh_toan_coc, thoi_diem_coc, gio_giu_phong_den,
    trang_thai, tk_hoan_ten, tk_hoan_so, tk_hoan_ngan_hang
)
SELECT
    b.ma_dat_phong, kh.id, 0, b.ngay_den, b.ngay_di,
    (SELECT SUM(lp.gia_theo_ngay)
     FROM _seed_ct s JOIN LOAI_PHONG lp ON lp.ten_loai = s.ten_loai_phong
     WHERE s.ma_dat_phong = b.ma_dat_phong),
    b.han_coc, NULL, b.gio_giu_phong_den,
    'CHO_COC', kh.ho_ten, printf('900000%06d', kh.id), 'Vietcombank'
FROM _seed_dp b
         JOIN KHACH_HANG kh ON kh.so_dien_thoai = b.so_dien_thoai
WHERE b.ma_dat_phong = 'DP-H09-001' AND b.loai_giao_dich = 'DAT_TRUOC';

INSERT INTO CHI_TIET_DAT_PHONG (
    dat_phong_id, loai_phong_id, phong_id, gia_moi_dem, gia_moi_gio
)
SELECT dp.id, lp.id, NULL, lp.gia_theo_ngay, lp.gia_theo_gio
FROM _seed_ct s
         JOIN DAT_PHONG dp ON dp.ma_dat_phong = s.ma_dat_phong
         JOIN LOAI_PHONG lp ON lp.ten_loai = s.ten_loai_phong
WHERE s.ma_dat_phong = 'DP-H09-001'
ORDER BY s.stt;

UPDATE DAT_PHONG
SET trang_thai = 'DA_DAT',
    thoi_diem_coc = (SELECT b.thoi_diem_coc FROM _seed_dp b WHERE b.ma_dat_phong = DAT_PHONG.ma_dat_phong),
    han_thanh_toan_coc = NULL
WHERE ma_dat_phong = 'DP-H09-001';

-- DP-H09-002
INSERT INTO DAT_PHONG (
    ma_dat_phong, khach_hang_id, la_vang_lai, ngay_den, ngay_di,
    tien_coc, han_thanh_toan_coc, thoi_diem_coc, gio_giu_phong_den,
    trang_thai, tk_hoan_ten, tk_hoan_so, tk_hoan_ngan_hang
)
SELECT
    b.ma_dat_phong, kh.id, 0, b.ngay_den, b.ngay_di,
    (SELECT SUM(lp.gia_theo_ngay)
     FROM _seed_ct s JOIN LOAI_PHONG lp ON lp.ten_loai = s.ten_loai_phong
     WHERE s.ma_dat_phong = b.ma_dat_phong),
    b.han_coc, NULL, b.gio_giu_phong_den,
    'CHO_COC', kh.ho_ten, printf('900000%06d', kh.id), 'Vietcombank'
FROM _seed_dp b
         JOIN KHACH_HANG kh ON kh.so_dien_thoai = b.so_dien_thoai
WHERE b.ma_dat_phong = 'DP-H09-002' AND b.loai_giao_dich = 'DAT_TRUOC';

INSERT INTO CHI_TIET_DAT_PHONG (
    dat_phong_id, loai_phong_id, phong_id, gia_moi_dem, gia_moi_gio
)
SELECT dp.id, lp.id, NULL, lp.gia_theo_ngay, lp.gia_theo_gio
FROM _seed_ct s
         JOIN DAT_PHONG dp ON dp.ma_dat_phong = s.ma_dat_phong
         JOIN LOAI_PHONG lp ON lp.ten_loai = s.ten_loai_phong
WHERE s.ma_dat_phong = 'DP-H09-002'
ORDER BY s.stt;

UPDATE DAT_PHONG
SET trang_thai = 'DA_DAT',
    thoi_diem_coc = (SELECT b.thoi_diem_coc FROM _seed_dp b WHERE b.ma_dat_phong = DAT_PHONG.ma_dat_phong),
    han_thanh_toan_coc = NULL
WHERE ma_dat_phong = 'DP-H09-002';

-- DP-H09-003
INSERT INTO DAT_PHONG (
    ma_dat_phong, khach_hang_id, la_vang_lai, ngay_den, ngay_di,
    tien_coc, han_thanh_toan_coc, thoi_diem_coc, gio_giu_phong_den,
    trang_thai, tk_hoan_ten, tk_hoan_so, tk_hoan_ngan_hang
)
SELECT
    b.ma_dat_phong, kh.id, 0, b.ngay_den, b.ngay_di,
    (SELECT SUM(lp.gia_theo_ngay)
     FROM _seed_ct s JOIN LOAI_PHONG lp ON lp.ten_loai = s.ten_loai_phong
     WHERE s.ma_dat_phong = b.ma_dat_phong),
    b.han_coc, NULL, b.gio_giu_phong_den,
    'CHO_COC', kh.ho_ten, printf('900000%06d', kh.id), 'Vietcombank'
FROM _seed_dp b
         JOIN KHACH_HANG kh ON kh.so_dien_thoai = b.so_dien_thoai
WHERE b.ma_dat_phong = 'DP-H09-003' AND b.loai_giao_dich = 'DAT_TRUOC';

INSERT INTO CHI_TIET_DAT_PHONG (
    dat_phong_id, loai_phong_id, phong_id, gia_moi_dem, gia_moi_gio
)
SELECT dp.id, lp.id, NULL, lp.gia_theo_ngay, lp.gia_theo_gio
FROM _seed_ct s
         JOIN DAT_PHONG dp ON dp.ma_dat_phong = s.ma_dat_phong
         JOIN LOAI_PHONG lp ON lp.ten_loai = s.ten_loai_phong
WHERE s.ma_dat_phong = 'DP-H09-003'
ORDER BY s.stt;

UPDATE DAT_PHONG
SET trang_thai = 'DA_DAT',
    thoi_diem_coc = (SELECT b.thoi_diem_coc FROM _seed_dp b WHERE b.ma_dat_phong = DAT_PHONG.ma_dat_phong),
    han_thanh_toan_coc = NULL
WHERE ma_dat_phong = 'DP-H09-003';

-- DP-H09-004
INSERT INTO DAT_PHONG (
    ma_dat_phong, khach_hang_id, la_vang_lai, ngay_den, ngay_di,
    tien_coc, han_thanh_toan_coc, thoi_diem_coc, gio_giu_phong_den,
    trang_thai, tk_hoan_ten, tk_hoan_so, tk_hoan_ngan_hang
)
SELECT
    b.ma_dat_phong, kh.id, 0, b.ngay_den, b.ngay_di,
    (SELECT SUM(lp.gia_theo_ngay)
     FROM _seed_ct s JOIN LOAI_PHONG lp ON lp.ten_loai = s.ten_loai_phong
     WHERE s.ma_dat_phong = b.ma_dat_phong),
    b.han_coc, NULL, b.gio_giu_phong_den,
    'CHO_COC', kh.ho_ten, printf('900000%06d', kh.id), 'Vietcombank'
FROM _seed_dp b
         JOIN KHACH_HANG kh ON kh.so_dien_thoai = b.so_dien_thoai
WHERE b.ma_dat_phong = 'DP-H09-004' AND b.loai_giao_dich = 'DAT_TRUOC';

INSERT INTO CHI_TIET_DAT_PHONG (
    dat_phong_id, loai_phong_id, phong_id, gia_moi_dem, gia_moi_gio
)
SELECT dp.id, lp.id, NULL, lp.gia_theo_ngay, lp.gia_theo_gio
FROM _seed_ct s
         JOIN DAT_PHONG dp ON dp.ma_dat_phong = s.ma_dat_phong
         JOIN LOAI_PHONG lp ON lp.ten_loai = s.ten_loai_phong
WHERE s.ma_dat_phong = 'DP-H09-004'
ORDER BY s.stt;

UPDATE DAT_PHONG
SET trang_thai = 'DA_DAT',
    thoi_diem_coc = (SELECT b.thoi_diem_coc FROM _seed_dp b WHERE b.ma_dat_phong = DAT_PHONG.ma_dat_phong),
    han_thanh_toan_coc = NULL
WHERE ma_dat_phong = 'DP-H09-004';
-- DP-H09-005
INSERT INTO DAT_PHONG (
    ma_dat_phong, khach_hang_id, la_vang_lai, ngay_den, ngay_di,
    tien_coc, han_thanh_toan_coc, thoi_diem_coc, gio_giu_phong_den,
    trang_thai, tk_hoan_ten, tk_hoan_so, tk_hoan_ngan_hang
)
SELECT
    b.ma_dat_phong, kh.id, 0, b.ngay_den, b.ngay_di,
    (SELECT SUM(lp.gia_theo_ngay)
     FROM _seed_ct s JOIN LOAI_PHONG lp ON lp.ten_loai = s.ten_loai_phong
     WHERE s.ma_dat_phong = b.ma_dat_phong),
    b.han_coc, NULL, b.gio_giu_phong_den,
    'CHO_COC', kh.ho_ten, printf('900000%06d', kh.id), 'Vietcombank'
FROM _seed_dp b
         JOIN KHACH_HANG kh ON kh.so_dien_thoai = b.so_dien_thoai
WHERE b.ma_dat_phong = 'DP-H09-005' AND b.loai_giao_dich = 'DAT_TRUOC';

INSERT INTO CHI_TIET_DAT_PHONG (
    dat_phong_id, loai_phong_id, phong_id, gia_moi_dem, gia_moi_gio
)
SELECT dp.id, lp.id, NULL, lp.gia_theo_ngay, lp.gia_theo_gio
FROM _seed_ct s
         JOIN DAT_PHONG dp ON dp.ma_dat_phong = s.ma_dat_phong
         JOIN LOAI_PHONG lp ON lp.ten_loai = s.ten_loai_phong
WHERE s.ma_dat_phong = 'DP-H09-005'
ORDER BY s.stt;

UPDATE DAT_PHONG
SET trang_thai = 'DA_DAT',
    thoi_diem_coc = (SELECT b.thoi_diem_coc FROM _seed_dp b WHERE b.ma_dat_phong = DAT_PHONG.ma_dat_phong),
    han_thanh_toan_coc = NULL
WHERE ma_dat_phong = 'DP-H09-005';

-- DP-H09-006
INSERT INTO DAT_PHONG (
    ma_dat_phong, khach_hang_id, la_vang_lai, ngay_den, ngay_di,
    tien_coc, han_thanh_toan_coc, thoi_diem_coc, gio_giu_phong_den,
    trang_thai, tk_hoan_ten, tk_hoan_so, tk_hoan_ngan_hang
)
SELECT
    b.ma_dat_phong, kh.id, 0, b.ngay_den, b.ngay_di,
    (SELECT SUM(lp.gia_theo_ngay)
     FROM _seed_ct s JOIN LOAI_PHONG lp ON lp.ten_loai = s.ten_loai_phong
     WHERE s.ma_dat_phong = b.ma_dat_phong),
    b.han_coc, NULL, b.gio_giu_phong_den,
    'CHO_COC', kh.ho_ten, printf('900000%06d', kh.id), 'Vietcombank'
FROM _seed_dp b
         JOIN KHACH_HANG kh ON kh.so_dien_thoai = b.so_dien_thoai
WHERE b.ma_dat_phong = 'DP-H09-006' AND b.loai_giao_dich = 'DAT_TRUOC';

INSERT INTO CHI_TIET_DAT_PHONG (
    dat_phong_id, loai_phong_id, phong_id, gia_moi_dem, gia_moi_gio
)
SELECT dp.id, lp.id, NULL, lp.gia_theo_ngay, lp.gia_theo_gio
FROM _seed_ct s
         JOIN DAT_PHONG dp ON dp.ma_dat_phong = s.ma_dat_phong
         JOIN LOAI_PHONG lp ON lp.ten_loai = s.ten_loai_phong
WHERE s.ma_dat_phong = 'DP-H09-006'
ORDER BY s.stt;

UPDATE DAT_PHONG
SET trang_thai = 'DA_DAT',
    thoi_diem_coc = (SELECT b.thoi_diem_coc FROM _seed_dp b WHERE b.ma_dat_phong = DAT_PHONG.ma_dat_phong),
    han_thanh_toan_coc = NULL
WHERE ma_dat_phong = 'DP-H09-006';
-- DP-H09-007
INSERT INTO DAT_PHONG (
    ma_dat_phong, khach_hang_id, la_vang_lai, ngay_den, ngay_di,
    tien_coc, han_thanh_toan_coc, thoi_diem_coc, gio_giu_phong_den,
    trang_thai, tk_hoan_ten, tk_hoan_so, tk_hoan_ngan_hang
)
SELECT
    b.ma_dat_phong, kh.id, 0, b.ngay_den, b.ngay_di,
    (SELECT SUM(lp.gia_theo_ngay)
     FROM _seed_ct s JOIN LOAI_PHONG lp ON lp.ten_loai = s.ten_loai_phong
     WHERE s.ma_dat_phong = b.ma_dat_phong),
    b.han_coc, NULL, b.gio_giu_phong_den,
    'CHO_COC', kh.ho_ten, printf('900000%06d', kh.id), 'Vietcombank'
FROM _seed_dp b
         JOIN KHACH_HANG kh ON kh.so_dien_thoai = b.so_dien_thoai
WHERE b.ma_dat_phong = 'DP-H09-007' AND b.loai_giao_dich = 'DAT_TRUOC';

INSERT INTO CHI_TIET_DAT_PHONG (
    dat_phong_id, loai_phong_id, phong_id, gia_moi_dem, gia_moi_gio
)
SELECT dp.id, lp.id, NULL, lp.gia_theo_ngay, lp.gia_theo_gio
FROM _seed_ct s
         JOIN DAT_PHONG dp ON dp.ma_dat_phong = s.ma_dat_phong
         JOIN LOAI_PHONG lp ON lp.ten_loai = s.ten_loai_phong
WHERE s.ma_dat_phong = 'DP-H09-007'
ORDER BY s.stt;

UPDATE DAT_PHONG
SET trang_thai = 'DA_DAT',
    thoi_diem_coc = (SELECT b.thoi_diem_coc FROM _seed_dp b WHERE b.ma_dat_phong = DAT_PHONG.ma_dat_phong),
    han_thanh_toan_coc = NULL
WHERE ma_dat_phong = 'DP-H09-007';

-- DP-H09-008
INSERT INTO DAT_PHONG (
    ma_dat_phong, khach_hang_id, la_vang_lai, ngay_den, ngay_di,
    tien_coc, han_thanh_toan_coc, thoi_diem_coc, gio_giu_phong_den,
    trang_thai, tk_hoan_ten, tk_hoan_so, tk_hoan_ngan_hang
)
SELECT
    b.ma_dat_phong, kh.id, 0, b.ngay_den, b.ngay_di,
    (SELECT SUM(lp.gia_theo_ngay)
     FROM _seed_ct s JOIN LOAI_PHONG lp ON lp.ten_loai = s.ten_loai_phong
     WHERE s.ma_dat_phong = b.ma_dat_phong),
    b.han_coc, NULL, b.gio_giu_phong_den,
    'CHO_COC', kh.ho_ten, printf('900000%06d', kh.id), 'Vietcombank'
FROM _seed_dp b
         JOIN KHACH_HANG kh ON kh.so_dien_thoai = b.so_dien_thoai
WHERE b.ma_dat_phong = 'DP-H09-008' AND b.loai_giao_dich = 'DAT_TRUOC';

INSERT INTO CHI_TIET_DAT_PHONG (
    dat_phong_id, loai_phong_id, phong_id, gia_moi_dem, gia_moi_gio
)
SELECT dp.id, lp.id, NULL, lp.gia_theo_ngay, lp.gia_theo_gio
FROM _seed_ct s
         JOIN DAT_PHONG dp ON dp.ma_dat_phong = s.ma_dat_phong
         JOIN LOAI_PHONG lp ON lp.ten_loai = s.ten_loai_phong
WHERE s.ma_dat_phong = 'DP-H09-008'
ORDER BY s.stt;

UPDATE DAT_PHONG
SET trang_thai = 'DA_DAT',
    thoi_diem_coc = (SELECT b.thoi_diem_coc FROM _seed_dp b WHERE b.ma_dat_phong = DAT_PHONG.ma_dat_phong),
    han_thanh_toan_coc = NULL
WHERE ma_dat_phong = 'DP-H09-008';
-- DP-H09-009
INSERT INTO DAT_PHONG (
    ma_dat_phong, khach_hang_id, la_vang_lai, ngay_den, ngay_di,
    tien_coc, han_thanh_toan_coc, thoi_diem_coc, gio_giu_phong_den,
    trang_thai, tk_hoan_ten, tk_hoan_so, tk_hoan_ngan_hang
)
SELECT
    b.ma_dat_phong, kh.id, 0, b.ngay_den, b.ngay_di,
    (SELECT SUM(lp.gia_theo_ngay)
     FROM _seed_ct s JOIN LOAI_PHONG lp ON lp.ten_loai = s.ten_loai_phong
     WHERE s.ma_dat_phong = b.ma_dat_phong),
    b.han_coc, NULL, b.gio_giu_phong_den,
    'CHO_COC', kh.ho_ten, printf('900000%06d', kh.id), 'Vietcombank'
FROM _seed_dp b
         JOIN KHACH_HANG kh ON kh.so_dien_thoai = b.so_dien_thoai
WHERE b.ma_dat_phong = 'DP-H09-009' AND b.loai_giao_dich = 'DAT_TRUOC';

INSERT INTO CHI_TIET_DAT_PHONG (
    dat_phong_id, loai_phong_id, phong_id, gia_moi_dem, gia_moi_gio
)
SELECT dp.id, lp.id, NULL, lp.gia_theo_ngay, lp.gia_theo_gio
FROM _seed_ct s
         JOIN DAT_PHONG dp ON dp.ma_dat_phong = s.ma_dat_phong
         JOIN LOAI_PHONG lp ON lp.ten_loai = s.ten_loai_phong
WHERE s.ma_dat_phong = 'DP-H09-009'
ORDER BY s.stt;

UPDATE DAT_PHONG
SET trang_thai = 'DA_DAT',
    thoi_diem_coc = (SELECT b.thoi_diem_coc FROM _seed_dp b WHERE b.ma_dat_phong = DAT_PHONG.ma_dat_phong),
    han_thanh_toan_coc = NULL
WHERE ma_dat_phong = 'DP-H09-009';

-- DP-H09-010
INSERT INTO DAT_PHONG (
    ma_dat_phong, khach_hang_id, la_vang_lai, ngay_den, ngay_di,
    tien_coc, han_thanh_toan_coc, thoi_diem_coc, gio_giu_phong_den,
    trang_thai, tk_hoan_ten, tk_hoan_so, tk_hoan_ngan_hang
)
SELECT
    b.ma_dat_phong, kh.id, 0, b.ngay_den, b.ngay_di,
    (SELECT SUM(lp.gia_theo_ngay)
     FROM _seed_ct s JOIN LOAI_PHONG lp ON lp.ten_loai = s.ten_loai_phong
     WHERE s.ma_dat_phong = b.ma_dat_phong),
    b.han_coc, NULL, b.gio_giu_phong_den,
    'CHO_COC', kh.ho_ten, printf('900000%06d', kh.id), 'Vietcombank'
FROM _seed_dp b
         JOIN KHACH_HANG kh ON kh.so_dien_thoai = b.so_dien_thoai
WHERE b.ma_dat_phong = 'DP-H09-010' AND b.loai_giao_dich = 'DAT_TRUOC';

INSERT INTO CHI_TIET_DAT_PHONG (
    dat_phong_id, loai_phong_id, phong_id, gia_moi_dem, gia_moi_gio
)
SELECT dp.id, lp.id, NULL, lp.gia_theo_ngay, lp.gia_theo_gio
FROM _seed_ct s
         JOIN DAT_PHONG dp ON dp.ma_dat_phong = s.ma_dat_phong
         JOIN LOAI_PHONG lp ON lp.ten_loai = s.ten_loai_phong
WHERE s.ma_dat_phong = 'DP-H09-010'
ORDER BY s.stt;

UPDATE DAT_PHONG
SET trang_thai = 'DA_DAT',
    thoi_diem_coc = (SELECT b.thoi_diem_coc FROM _seed_dp b WHERE b.ma_dat_phong = DAT_PHONG.ma_dat_phong),
    han_thanh_toan_coc = NULL
WHERE ma_dat_phong = 'DP-H09-010';
-- DP-H10-001
INSERT INTO DAT_PHONG (
    ma_dat_phong, khach_hang_id, la_vang_lai, ngay_den, ngay_di,
    tien_coc, han_thanh_toan_coc, thoi_diem_coc, gio_giu_phong_den,
    trang_thai, tk_hoan_ten, tk_hoan_so, tk_hoan_ngan_hang
)
SELECT
    b.ma_dat_phong, kh.id, 0, b.ngay_den, b.ngay_di,
    (SELECT SUM(lp.gia_theo_ngay)
     FROM _seed_ct s JOIN LOAI_PHONG lp ON lp.ten_loai = s.ten_loai_phong
     WHERE s.ma_dat_phong = b.ma_dat_phong),
    b.han_coc, NULL, b.gio_giu_phong_den,
    'CHO_COC', kh.ho_ten, printf('900000%06d', kh.id), 'Vietcombank'
FROM _seed_dp b
         JOIN KHACH_HANG kh ON kh.so_dien_thoai = b.so_dien_thoai
WHERE b.ma_dat_phong = 'DP-H10-001' AND b.loai_giao_dich = 'DAT_TRUOC';

INSERT INTO CHI_TIET_DAT_PHONG (
    dat_phong_id, loai_phong_id, phong_id, gia_moi_dem, gia_moi_gio
)
SELECT dp.id, lp.id, NULL, lp.gia_theo_ngay, lp.gia_theo_gio
FROM _seed_ct s
         JOIN DAT_PHONG dp ON dp.ma_dat_phong = s.ma_dat_phong
         JOIN LOAI_PHONG lp ON lp.ten_loai = s.ten_loai_phong
WHERE s.ma_dat_phong = 'DP-H10-001'
ORDER BY s.stt;

UPDATE DAT_PHONG
SET trang_thai = 'DA_DAT',
    thoi_diem_coc = (SELECT b.thoi_diem_coc FROM _seed_dp b WHERE b.ma_dat_phong = DAT_PHONG.ma_dat_phong),
    han_thanh_toan_coc = NULL
WHERE ma_dat_phong = 'DP-H10-001';

-- DP-H10-002
INSERT INTO DAT_PHONG (
    ma_dat_phong, khach_hang_id, la_vang_lai, ngay_den, ngay_di,
    tien_coc, han_thanh_toan_coc, thoi_diem_coc, gio_giu_phong_den,
    trang_thai, tk_hoan_ten, tk_hoan_so, tk_hoan_ngan_hang
)
SELECT
    b.ma_dat_phong, kh.id, 0, b.ngay_den, b.ngay_di,
    (SELECT SUM(lp.gia_theo_ngay)
     FROM _seed_ct s JOIN LOAI_PHONG lp ON lp.ten_loai = s.ten_loai_phong
     WHERE s.ma_dat_phong = b.ma_dat_phong),
    b.han_coc, NULL, b.gio_giu_phong_den,
    'CHO_COC', kh.ho_ten, printf('900000%06d', kh.id), 'Vietcombank'
FROM _seed_dp b
JOIN KHACH_HANG kh ON kh.so_dien_thoai = b.so_dien_thoai
WHERE b.ma_dat_phong = 'DP-H10-002' AND b.loai_giao_dich = 'DAT_TRUOC';

INSERT INTO CHI_TIET_DAT_PHONG (
    dat_phong_id, loai_phong_id, phong_id, gia_moi_dem, gia_moi_gio
)
SELECT dp.id, lp.id, NULL, lp.gia_theo_ngay, lp.gia_theo_gio
FROM _seed_ct s
         JOIN DAT_PHONG dp ON dp.ma_dat_phong = s.ma_dat_phong
         JOIN LOAI_PHONG lp ON lp.ten_loai = s.ten_loai_phong
WHERE s.ma_dat_phong = 'DP-H10-002'
ORDER BY s.stt;

UPDATE DAT_PHONG
SET trang_thai = 'DA_DAT',
    thoi_diem_coc = (SELECT b.thoi_diem_coc FROM _seed_dp b WHERE b.ma_dat_phong = DAT_PHONG.ma_dat_phong),
    han_thanh_toan_coc = NULL
WHERE ma_dat_phong = 'DP-H10-002';

-- DP-H10-003
INSERT INTO DAT_PHONG (
    ma_dat_phong, khach_hang_id, la_vang_lai, ngay_den, ngay_di,
    tien_coc, han_thanh_toan_coc, thoi_diem_coc, gio_giu_phong_den,
    trang_thai, tk_hoan_ten, tk_hoan_so, tk_hoan_ngan_hang
)
SELECT
    b.ma_dat_phong, kh.id, 0, b.ngay_den, b.ngay_di,
    (SELECT SUM(lp.gia_theo_ngay)
     FROM _seed_ct s JOIN LOAI_PHONG lp ON lp.ten_loai = s.ten_loai_phong
     WHERE s.ma_dat_phong = b.ma_dat_phong),
    b.han_coc, NULL, b.gio_giu_phong_den,
    'CHO_COC', kh.ho_ten, printf('900000%06d', kh.id), 'Vietcombank'
FROM _seed_dp b
         JOIN KHACH_HANG kh ON kh.so_dien_thoai = b.so_dien_thoai
WHERE b.ma_dat_phong = 'DP-H10-003' AND b.loai_giao_dich = 'DAT_TRUOC';

INSERT INTO CHI_TIET_DAT_PHONG (
    dat_phong_id, loai_phong_id, phong_id, gia_moi_dem, gia_moi_gio
)
SELECT dp.id, lp.id, NULL, lp.gia_theo_ngay, lp.gia_theo_gio
FROM _seed_ct s
         JOIN DAT_PHONG dp ON dp.ma_dat_phong = s.ma_dat_phong
         JOIN LOAI_PHONG lp ON lp.ten_loai = s.ten_loai_phong
WHERE s.ma_dat_phong = 'DP-H10-003'
ORDER BY s.stt;

UPDATE DAT_PHONG
SET trang_thai = 'DA_DAT',
    thoi_diem_coc = (SELECT b.thoi_diem_coc FROM _seed_dp b WHERE b.ma_dat_phong = DAT_PHONG.ma_dat_phong),
    han_thanh_toan_coc = NULL
WHERE ma_dat_phong = 'DP-H10-003';

-- DP-H10-004
INSERT INTO DAT_PHONG (
    ma_dat_phong, khach_hang_id, la_vang_lai, ngay_den, ngay_di,
    tien_coc, han_thanh_toan_coc, thoi_diem_coc, gio_giu_phong_den,
    trang_thai, tk_hoan_ten, tk_hoan_so, tk_hoan_ngan_hang
)
SELECT
    b.ma_dat_phong, kh.id, 0, b.ngay_den, b.ngay_di,
    (SELECT SUM(lp.gia_theo_ngay)
     FROM _seed_ct s JOIN LOAI_PHONG lp ON lp.ten_loai = s.ten_loai_phong
     WHERE s.ma_dat_phong = b.ma_dat_phong),
    b.han_coc, NULL, b.gio_giu_phong_den,
    'CHO_COC', kh.ho_ten, printf('900000%06d', kh.id), 'Vietcombank'
FROM _seed_dp b
         JOIN KHACH_HANG kh ON kh.so_dien_thoai = b.so_dien_thoai
WHERE b.ma_dat_phong = 'DP-H10-004' AND b.loai_giao_dich = 'DAT_TRUOC';

INSERT INTO CHI_TIET_DAT_PHONG (
    dat_phong_id, loai_phong_id, phong_id, gia_moi_dem, gia_moi_gio
)
SELECT dp.id, lp.id, NULL, lp.gia_theo_ngay, lp.gia_theo_gio
FROM _seed_ct s
         JOIN DAT_PHONG dp ON dp.ma_dat_phong = s.ma_dat_phong
         JOIN LOAI_PHONG lp ON lp.ten_loai = s.ten_loai_phong
WHERE s.ma_dat_phong = 'DP-H10-004'
ORDER BY s.stt;

UPDATE DAT_PHONG
SET trang_thai = 'DA_DAT',
    thoi_diem_coc = (SELECT b.thoi_diem_coc FROM _seed_dp b WHERE b.ma_dat_phong = DAT_PHONG.ma_dat_phong),
    han_thanh_toan_coc = NULL
WHERE ma_dat_phong = 'DP-H10-004';

-- DP-H09-011
INSERT INTO DAT_PHONG (
    ma_dat_phong, khach_hang_id, la_vang_lai, ngay_den, ngay_di,
    tien_coc, han_thanh_toan_coc, thoi_diem_coc, gio_giu_phong_den,
    trang_thai, tk_hoan_ten, tk_hoan_so, tk_hoan_ngan_hang
)
SELECT
    b.ma_dat_phong, kh.id, 0, b.ngay_den, b.ngay_di,
    (SELECT SUM(lp.gia_theo_ngay)
     FROM _seed_ct s JOIN LOAI_PHONG lp ON lp.ten_loai = s.ten_loai_phong
     WHERE s.ma_dat_phong = b.ma_dat_phong),
    b.han_coc, NULL, b.gio_giu_phong_den,
    'CHO_COC', kh.ho_ten, printf('900000%06d', kh.id), 'Vietcombank'
FROM _seed_dp b
         JOIN KHACH_HANG kh ON kh.so_dien_thoai = b.so_dien_thoai
WHERE b.ma_dat_phong = 'DP-H09-011' AND b.loai_giao_dich = 'DAT_TRUOC';

INSERT INTO CHI_TIET_DAT_PHONG (
    dat_phong_id, loai_phong_id, phong_id, gia_moi_dem, gia_moi_gio
)
SELECT dp.id, lp.id, NULL, lp.gia_theo_ngay, lp.gia_theo_gio
FROM _seed_ct s
         JOIN DAT_PHONG dp ON dp.ma_dat_phong = s.ma_dat_phong
         JOIN LOAI_PHONG lp ON lp.ten_loai = s.ten_loai_phong
WHERE s.ma_dat_phong = 'DP-H09-011'
ORDER BY s.stt;

UPDATE DAT_PHONG
SET trang_thai = 'DA_DAT',
    thoi_diem_coc = (SELECT b.thoi_diem_coc FROM _seed_dp b WHERE b.ma_dat_phong = DAT_PHONG.ma_dat_phong),
    han_thanh_toan_coc = NULL
WHERE ma_dat_phong = 'DP-H09-011';

-- DP-H09-012
INSERT INTO DAT_PHONG (
    ma_dat_phong, khach_hang_id, la_vang_lai, ngay_den, ngay_di,
    tien_coc, han_thanh_toan_coc, thoi_diem_coc, gio_giu_phong_den,
    trang_thai, tk_hoan_ten, tk_hoan_so, tk_hoan_ngan_hang
)
SELECT
    b.ma_dat_phong, kh.id, 0, b.ngay_den, b.ngay_di,
    (SELECT SUM(lp.gia_theo_ngay)
     FROM _seed_ct s JOIN LOAI_PHONG lp ON lp.ten_loai = s.ten_loai_phong
     WHERE s.ma_dat_phong = b.ma_dat_phong),
    b.han_coc, NULL, b.gio_giu_phong_den,
    'CHO_COC', kh.ho_ten, printf('900000%06d', kh.id), 'Vietcombank'
FROM _seed_dp b
         JOIN KHACH_HANG kh ON kh.so_dien_thoai = b.so_dien_thoai
WHERE b.ma_dat_phong = 'DP-H09-012' AND b.loai_giao_dich = 'DAT_TRUOC';

INSERT INTO CHI_TIET_DAT_PHONG (
    dat_phong_id, loai_phong_id, phong_id, gia_moi_dem, gia_moi_gio
)
SELECT dp.id, lp.id, NULL, lp.gia_theo_ngay, lp.gia_theo_gio
FROM _seed_ct s
         JOIN DAT_PHONG dp ON dp.ma_dat_phong = s.ma_dat_phong
         JOIN LOAI_PHONG lp ON lp.ten_loai = s.ten_loai_phong
WHERE s.ma_dat_phong = 'DP-H09-012'
ORDER BY s.stt;

UPDATE DAT_PHONG
SET trang_thai = 'DA_DAT',
    thoi_diem_coc = (SELECT b.thoi_diem_coc FROM _seed_dp b WHERE b.ma_dat_phong = DAT_PHONG.ma_dat_phong),
    han_thanh_toan_coc = NULL
WHERE ma_dat_phong = 'DP-H09-012';
-- DP-H10-005
INSERT INTO DAT_PHONG (
    ma_dat_phong, khach_hang_id, la_vang_lai, ngay_den, ngay_di,
    tien_coc, han_thanh_toan_coc, thoi_diem_coc, gio_giu_phong_den,
    trang_thai, tk_hoan_ten, tk_hoan_so, tk_hoan_ngan_hang
)
SELECT
    b.ma_dat_phong, kh.id, 0, b.ngay_den, b.ngay_di,
(SELECT SUM(lp.gia_theo_ngay)
 FROM _seed_ct s JOIN LOAI_PHONG lp ON lp.ten_loai = s.ten_loai_phong
 WHERE s.ma_dat_phong = b.ma_dat_phong),
    b.han_coc, NULL, b.gio_giu_phong_den,
    'CHO_COC', kh.ho_ten, printf('900000%06d', kh.id), 'Vietcombank'
 FROM _seed_dp b
     JOIN KHACH_HANG kh ON kh.so_dien_thoai = b.so_dien_thoai
 WHERE b.ma_dat_phong = 'DP-H10-005' AND b.loai_giao_dich = 'DAT_TRUOC';

INSERT INTO CHI_TIET_DAT_PHONG (
    dat_phong_id, loai_phong_id, phong_id, gia_moi_dem, gia_moi_gio
)
SELECT dp.id, lp.id, NULL, lp.gia_theo_ngay, lp.gia_theo_gio
FROM _seed_ct s
         JOIN DAT_PHONG dp ON dp.ma_dat_phong = s.ma_dat_phong
         JOIN LOAI_PHONG lp ON lp.ten_loai = s.ten_loai_phong
WHERE s.ma_dat_phong = 'DP-H10-005'
ORDER BY s.stt;

UPDATE DAT_PHONG
SET trang_thai = 'DA_DAT',
    thoi_diem_coc = (SELECT b.thoi_diem_coc FROM _seed_dp b WHERE b.ma_dat_phong = DAT_PHONG.ma_dat_phong),
    han_thanh_toan_coc = NULL
WHERE ma_dat_phong = 'DP-H10-005';

-- DP-H10-006
INSERT INTO DAT_PHONG (
    ma_dat_phong, khach_hang_id, la_vang_lai, ngay_den, ngay_di,
    tien_coc, han_thanh_toan_coc, thoi_diem_coc, gio_giu_phong_den,
    trang_thai, tk_hoan_ten, tk_hoan_so, tk_hoan_ngan_hang
)
SELECT
    b.ma_dat_phong, kh.id, 0, b.ngay_den, b.ngay_di,
    (SELECT SUM(lp.gia_theo_ngay)
     FROM _seed_ct s JOIN LOAI_PHONG lp ON lp.ten_loai = s.ten_loai_phong
     WHERE s.ma_dat_phong = b.ma_dat_phong),
    b.han_coc, NULL, b.gio_giu_phong_den,
    'CHO_COC', kh.ho_ten, printf('900000%06d', kh.id), 'Vietcombank'
FROM _seed_dp b
         JOIN KHACH_HANG kh ON kh.so_dien_thoai = b.so_dien_thoai
WHERE b.ma_dat_phong = 'DP-H10-006' AND b.loai_giao_dich = 'DAT_TRUOC';

INSERT INTO CHI_TIET_DAT_PHONG (
    dat_phong_id, loai_phong_id, phong_id, gia_moi_dem, gia_moi_gio
)
SELECT dp.id, lp.id, NULL, lp.gia_theo_ngay, lp.gia_theo_gio
FROM _seed_ct s
         JOIN DAT_PHONG dp ON dp.ma_dat_phong = s.ma_dat_phong
         JOIN LOAI_PHONG lp ON lp.ten_loai = s.ten_loai_phong
WHERE s.ma_dat_phong = 'DP-H10-006'
ORDER BY s.stt;

UPDATE DAT_PHONG
SET trang_thai = 'DA_DAT',
    thoi_diem_coc = (SELECT b.thoi_diem_coc FROM _seed_dp b WHERE b.ma_dat_phong = DAT_PHONG.ma_dat_phong),
    han_thanh_toan_coc = NULL
WHERE ma_dat_phong = 'DP-H10-006';
-- DP-H10-007
INSERT INTO DAT_PHONG (
    ma_dat_phong, khach_hang_id, la_vang_lai, ngay_den, ngay_di,
    tien_coc, han_thanh_toan_coc, thoi_diem_coc, gio_giu_phong_den,
    trang_thai, tk_hoan_ten, tk_hoan_so, tk_hoan_ngan_hang
)
SELECT
    b.ma_dat_phong, kh.id, 0, b.ngay_den, b.ngay_di,
    (SELECT SUM(lp.gia_theo_ngay)
     FROM _seed_ct s JOIN LOAI_PHONG lp ON lp.ten_loai = s.ten_loai_phong
     WHERE s.ma_dat_phong = b.ma_dat_phong),
    b.han_coc, NULL, b.gio_giu_phong_den,
    'CHO_COC', kh.ho_ten, printf('900000%06d', kh.id), 'Vietcombank'
FROM _seed_dp b
         JOIN KHACH_HANG kh ON kh.so_dien_thoai = b.so_dien_thoai
WHERE b.ma_dat_phong = 'DP-H10-007' AND b.loai_giao_dich = 'DAT_TRUOC';

INSERT INTO CHI_TIET_DAT_PHONG (
    dat_phong_id, loai_phong_id, phong_id, gia_moi_dem, gia_moi_gio
)
SELECT dp.id, lp.id, NULL, lp.gia_theo_ngay, lp.gia_theo_gio
FROM _seed_ct s
         JOIN DAT_PHONG dp ON dp.ma_dat_phong = s.ma_dat_phong
         JOIN LOAI_PHONG lp ON lp.ten_loai = s.ten_loai_phong
WHERE s.ma_dat_phong = 'DP-H10-007'
ORDER BY s.stt;

UPDATE DAT_PHONG
SET trang_thai = 'DA_DAT',
    thoi_diem_coc = (SELECT b.thoi_diem_coc FROM _seed_dp b WHERE b.ma_dat_phong = DAT_PHONG.ma_dat_phong),
    han_thanh_toan_coc = NULL
WHERE ma_dat_phong = 'DP-H10-007';

-- DP-H10-008
INSERT INTO DAT_PHONG (
    ma_dat_phong, khach_hang_id, la_vang_lai, ngay_den, ngay_di,
    tien_coc, han_thanh_toan_coc, thoi_diem_coc, gio_giu_phong_den,
    trang_thai, tk_hoan_ten, tk_hoan_so, tk_hoan_ngan_hang
)
SELECT
    b.ma_dat_phong, kh.id, 0, b.ngay_den, b.ngay_di,
    (SELECT SUM(lp.gia_theo_ngay)
     FROM _seed_ct s JOIN LOAI_PHONG lp ON lp.ten_loai = s.ten_loai_phong
     WHERE s.ma_dat_phong = b.ma_dat_phong),
    b.han_coc, NULL, b.gio_giu_phong_den,
    'CHO_COC', kh.ho_ten, printf('900000%06d', kh.id), 'Vietcombank'
FROM _seed_dp b
         JOIN KHACH_HANG kh ON kh.so_dien_thoai = b.so_dien_thoai
WHERE b.ma_dat_phong = 'DP-H10-008' AND b.loai_giao_dich = 'DAT_TRUOC';

INSERT INTO CHI_TIET_DAT_PHONG (
    dat_phong_id, loai_phong_id, phong_id, gia_moi_dem, gia_moi_gio
)
SELECT dp.id, lp.id, NULL, lp.gia_theo_ngay, lp.gia_theo_gio
FROM _seed_ct s
         JOIN DAT_PHONG dp ON dp.ma_dat_phong = s.ma_dat_phong
         JOIN LOAI_PHONG lp ON lp.ten_loai = s.ten_loai_phong
WHERE s.ma_dat_phong = 'DP-H10-008'
ORDER BY s.stt;

UPDATE DAT_PHONG
SET trang_thai = 'DA_DAT',
    thoi_diem_coc = (SELECT b.thoi_diem_coc FROM _seed_dp b WHERE b.ma_dat_phong = DAT_PHONG.ma_dat_phong),
    han_thanh_toan_coc = NULL
WHERE ma_dat_phong = 'DP-H10-008';
-- Tao anh xa tung dong chi tiet voi dong staging theo thu tu stt.
INSERT INTO _seed_map (ma_dat_phong, stt, chi_tiet_id, so_phong, checkin_thuc_te, checkout_thuc_te)
WITH s AS (
    SELECT ma_dat_phong, stt, so_phong, checkin_thuc_te, checkout_thuc_te,
           ROW_NUMBER() OVER (PARTITION BY ma_dat_phong ORDER BY stt) AS rn
    FROM _seed_ct
    WHERE ma_dat_phong LIKE 'DP-H%'
), c AS (
    SELECT dp.ma_dat_phong, ct.id,
           ROW_NUMBER() OVER (PARTITION BY dp.id ORDER BY ct.id) AS rn
    FROM CHI_TIET_DAT_PHONG ct
             JOIN DAT_PHONG dp ON dp.id = ct.dat_phong_id
    WHERE dp.ma_dat_phong LIKE 'DP-H%'
)
SELECT s.ma_dat_phong, s.stt, c.id, s.so_phong, s.checkin_thuc_te, s.checkout_thuc_te
FROM s JOIN c ON c.ma_dat_phong = s.ma_dat_phong AND c.rn = s.rn;

-- 3. Gan phong vat ly cho cac dong dat truoc lich su co phong cu the.
UPDATE CHI_TIET_DAT_PHONG
SET phong_id = (
    SELECT p.id
    FROM _seed_map m JOIN PHONG p ON p.so_phong = m.so_phong
    WHERE m.chi_tiet_id = CHI_TIET_DAT_PHONG.id
)
WHERE id IN (SELECT chi_tiet_id FROM _seed_map WHERE so_phong IS NOT NULL);

-- 4. Tao khach vang lai lich su: 8 luot theo gio + 2 luot theo ngay.

INSERT INTO DAT_PHONG (
    ma_dat_phong, khach_hang_id, la_vang_lai, ngay_den, ngay_di,
    tien_coc, trang_thai
)
SELECT b.ma_dat_phong, kh.id, 1, b.ngay_den, b.ngay_di, 0, 'DANG_O'
FROM (

         SELECT 'WI-H09-001' ma_dat_phong, '0900000021' so_dien_thoai, '2026-09-03 15:00:00' ngay_den, '2026-09-03 18:00:00' ngay_di
         UNION ALL
         SELECT 'WI-H09-002' ma_dat_phong, '0900000022' so_dien_thoai, '2026-09-06 15:00:00' ngay_den, '2026-09-06 18:00:00' ngay_di
         UNION ALL
         SELECT 'WI-H09-003' ma_dat_phong, '0900000023' so_dien_thoai, '2026-09-09 15:00:00' ngay_den, '2026-09-09 18:00:00' ngay_di
         UNION ALL
         SELECT 'WI-H09-004' ma_dat_phong, '0900000024' so_dien_thoai, '2026-09-13 15:00:00' ngay_den, '2026-09-13 18:00:00' ngay_di
         UNION ALL
         SELECT 'WI-H10-001' ma_dat_phong, '0900000021' so_dien_thoai, '2026-10-04 14:00:00' ngay_den, '2026-10-04 17:00:00' ngay_di
         UNION ALL
         SELECT 'WI-H10-002' ma_dat_phong, '0900000022' so_dien_thoai, '2026-10-07 15:00:00' ngay_den, '2026-10-07 18:00:00' ngay_di
         UNION ALL
         SELECT 'WI-H10-003' ma_dat_phong, '0900000023' so_dien_thoai, '2026-10-12 16:00:00' ngay_den, '2026-10-12 19:00:00' ngay_di
         UNION ALL
         SELECT 'WI-H10-004' ma_dat_phong, '0900000024' so_dien_thoai, '2026-10-20 14:00:00' ngay_den, '2026-10-20 17:00:00' ngay_di
         UNION ALL
         SELECT 'WI-H09-005' ma_dat_phong, '0900000021' so_dien_thoai, '2026-09-16 14:00:00' ngay_den, '2026-09-17 12:00:00' ngay_di
         UNION ALL
         SELECT 'WI-H10-005' ma_dat_phong, '0900000022' so_dien_thoai, '2026-10-13 14:00:00' ngay_den, '2026-10-14 12:00:00' ngay_di

     ) b JOIN KHACH_HANG kh ON kh.so_dien_thoai = b.so_dien_thoai;
-- Them dong phong walk-in: phong va check-in bat buoc co ngay luc INSERT.
INSERT INTO CHI_TIET_DAT_PHONG (
    dat_phong_id, loai_phong_id, phong_id, gia_moi_dem, gia_moi_gio,
    checkin_thuc_te
)
SELECT dp.id, lp.id, p.id, lp.gia_theo_ngay, lp.gia_theo_gio, s.checkin_thuc_te
FROM _seed_ct s
         JOIN DAT_PHONG dp ON dp.ma_dat_phong = s.ma_dat_phong AND dp.la_vang_lai = 1
         JOIN LOAI_PHONG lp ON lp.ten_loai = s.ten_loai_phong
         JOIN PHONG p ON p.so_phong = s.so_phong
WHERE s.ma_dat_phong LIKE 'WI-H%';

-- Bo sung anh xa cho cac dong walk-in lich su.
INSERT INTO _seed_map (ma_dat_phong, stt, chi_tiet_id, so_phong, checkin_thuc_te, checkout_thuc_te)
WITH s AS (
    SELECT ma_dat_phong, stt, so_phong, checkin_thuc_te, checkout_thuc_te,
           ROW_NUMBER() OVER (PARTITION BY ma_dat_phong ORDER BY stt) AS rn
    FROM _seed_ct WHERE ma_dat_phong LIKE 'WI-H%'
), c AS (
    SELECT dp.ma_dat_phong, ct.id,
           ROW_NUMBER() OVER (PARTITION BY dp.id ORDER BY ct.id) AS rn
    FROM CHI_TIET_DAT_PHONG ct JOIN DAT_PHONG dp ON dp.id = ct.dat_phong_id
    WHERE dp.ma_dat_phong LIKE 'WI-H%'
)
SELECT s.ma_dat_phong, s.stt, c.id, s.so_phong, s.checkin_thuc_te, s.checkout_thuc_te
FROM s JOIN c ON c.ma_dat_phong=s.ma_dat_phong AND c.rn=s.rn;

-- 5. Check-in cac dong dat truoc lich su.
UPDATE CHI_TIET_DAT_PHONG
SET checkin_thuc_te = (
    SELECT m.checkin_thuc_te FROM _seed_map m
    WHERE m.chi_tiet_id = CHI_TIET_DAT_PHONG.id
)
WHERE id IN (
    SELECT chi_tiet_id FROM _seed_map
    WHERE ma_dat_phong LIKE 'DP-H%' AND checkin_thuc_te IS NOT NULL
);
-- Ghi nhan khach o phong trong luc dong phong dang o.
INSERT INTO KHACH_O_PHONG (chi_tiet_dat_phong_id, khach_hang_id)
SELECT m.chi_tiet_id, dp.khach_hang_id
FROM _seed_map m JOIN DAT_PHONG dp ON dp.ma_dat_phong=m.ma_dat_phong
                 JOIN CHI_TIET_DAT_PHONG ct ON ct.id=m.chi_tiet_id
WHERE m.ma_dat_phong LIKE 'DP-H%' AND ct.checkin_thuc_te IS NOT NULL;
INSERT INTO KHACH_O_PHONG (chi_tiet_dat_phong_id, khach_hang_id)
SELECT ct.id, dp.khach_hang_id
FROM DAT_PHONG dp JOIN CHI_TIET_DAT_PHONG ct ON ct.dat_phong_id=dp.id
WHERE dp.ma_dat_phong LIKE 'WI-H%';

-- 6. Huy 5 dat phong theo chinh sach; 1 dat phong no-show toan bo.

UPDATE DAT_PHONG SET trang_thai = 'DA_HUY', ly_do_huy = 'KHACH_HUY', thoi_diem_huy = '2026-09-17 10:00:00', tien_coc_hoan = 300000, ma_hoan_coc = 'REF-H09-011', thoi_diem_hoan_coc = '2026-09-17 10:05:00', noi_dung_hoan_coc = 'Hoan coc dat phong DP-H09-011' WHERE ma_dat_phong = 'DP-H09-011';

UPDATE DAT_PHONG SET trang_thai = 'DA_HUY', ly_do_huy = 'KHACH_HUY', thoi_diem_huy = '2026-09-24 02:00:00', tien_coc_hoan = 225000, ma_hoan_coc = 'REF-H09-012', thoi_diem_hoan_coc = '2026-09-24 02:05:00', noi_dung_hoan_coc = 'Hoan 50 phan tram coc DP-H09-012' WHERE ma_dat_phong = 'DP-H09-012';

UPDATE DAT_PHONG SET trang_thai = 'DA_HUY', ly_do_huy = 'KHACH_HUY', thoi_diem_huy = '2026-10-14 18:00:00' WHERE ma_dat_phong = 'DP-H10-005';

UPDATE DAT_PHONG SET trang_thai = 'DA_HUY', ly_do_huy = 'KHACH_SAN_HUY', thoi_diem_huy = '2026-10-15 11:00:00', ghi_chu_huy = 'Khach san huy do phong khong dap ung yeu cau', tien_coc_hoan = 2000000, ma_hoan_coc = 'REF-H10-006', thoi_diem_hoan_coc = '2026-10-15 11:05:00', noi_dung_hoan_coc = 'Hoan coc do khach san huy DP-H10-006' WHERE ma_dat_phong = 'DP-H10-006';

UPDATE DAT_PHONG SET trang_thai = 'DA_HUY', ly_do_huy = 'BAT_KHA_KHANG', thoi_diem_huy = '2026-10-20 10:00:00', ghi_chu_huy = 'Khach khong the di chuyen do su kien bat kha khang', tien_coc_hoan = 300000, ma_hoan_coc = 'REF-H10-007', thoi_diem_hoan_coc = '2026-10-20 10:05:00', noi_dung_hoan_coc = 'Hoan coc bat kha khang DP-H10-007' WHERE ma_dat_phong = 'DP-H10-007';

UPDATE DAT_PHONG SET trang_thai = 'DA_HUY', ly_do_huy = 'NO_SHOW', thoi_diem_huy = '2026-10-28 23:59:59' WHERE ma_dat_phong = 'DP-H10-008';
-- 7. Tao tai khoan nhan vien ky thuat de ghi nhan nguoi them/xoa mem dich vu.
-- Chuoi mat_khau_bam nay khong duoc dung de dang nhap.

-- Them 15 dong dich vu lich su, trong do 1 dong se duoc xoa mem.
INSERT INTO CHI_TIET_DICH_VU (
    chi_tiet_dat_phong_id, dich_vu_id, ten_dich_vu_luc_goi, don_vi_luc_goi,
    so_luong, don_gia, thoi_diem, tai_khoan_id
)
SELECT m.chi_tiet_id, dv.id, dv.ten, dv.don_vi, s.so_luong, dv.don_gia,
       s.thoi_diem, tk.id
FROM (

         SELECT 'DP-H09-001' ma_dat_phong, 1 stt, 'Nước suối' ten_dich_vu, 2 so_luong, '2026-09-03 09:00:00' thoi_diem
         UNION ALL
         SELECT 'DP-H09-002' ma_dat_phong, 2 stt, 'Cà phê' ten_dich_vu, 2 so_luong, '2026-09-06 09:00:00' thoi_diem
         UNION ALL
         SELECT 'DP-H09-003' ma_dat_phong, 1 stt, 'Mì ly' ten_dich_vu, 2 so_luong, '2026-09-08 19:00:00' thoi_diem
         UNION ALL
         SELECT 'DP-H09-003' ma_dat_phong, 2 stt, 'Nước ngọt' ten_dich_vu, 1 so_luong, '2026-09-09 09:00:00' thoi_diem
         UNION ALL
         SELECT 'DP-H09-005' ma_dat_phong, 1 stt, 'Giặt ủi' ten_dich_vu, 1 so_luong, '2026-09-16 10:00:00' thoi_diem
         UNION ALL
         SELECT 'DP-H09-006' ma_dat_phong, 1 stt, 'Ăn sáng' ten_dich_vu, 2 so_luong, '2026-09-19 08:00:00' thoi_diem
         UNION ALL
         SELECT 'DP-H09-007' ma_dat_phong, 1 stt, 'Nước suối' ten_dich_vu, 3 so_luong, '2026-09-22 09:00:00' thoi_diem
         UNION ALL
         SELECT 'DP-H09-010' ma_dat_phong, 1 stt, 'Nước ngọt' ten_dich_vu, 2 so_luong, '2026-10-01 09:00:00' thoi_diem
         UNION ALL
         SELECT 'WI-H09-001' ma_dat_phong, 1 stt, 'Mì ly' ten_dich_vu, 1 so_luong, '2026-09-03 16:00:00' thoi_diem
         UNION ALL
         SELECT 'WI-H09-005' ma_dat_phong, 1 stt, 'Cà phê' ten_dich_vu, 1 so_luong, '2026-09-16 18:00:00' thoi_diem
         UNION ALL
         SELECT 'DP-H10-001' ma_dat_phong, 1 stt, 'Nước suối' ten_dich_vu, 2 so_luong, '2026-10-04 09:00:00' thoi_diem
         UNION ALL
         SELECT 'DP-H10-002' ma_dat_phong, 1 stt, 'Ăn sáng' ten_dich_vu, 1 so_luong, '2026-10-07 09:00:00' thoi_diem
         UNION ALL
         SELECT 'DP-H10-003' ma_dat_phong, 1 stt, 'Giặt ủi' ten_dich_vu, 1 so_luong, '2026-10-10 09:00:00' thoi_diem
         UNION ALL
         SELECT 'WI-H10-002' ma_dat_phong, 1 stt, 'Nước ngọt' ten_dich_vu, 2 so_luong, '2026-10-07 16:00:00' thoi_diem
         UNION ALL
         SELECT 'DP-H10-004' ma_dat_phong, 1 stt, 'Bia' ten_dich_vu, 1 so_luong, '2026-10-25 09:00:00' thoi_diem

     ) s
         JOIN _seed_map m ON m.ma_dat_phong=s.ma_dat_phong AND m.stt=s.stt
         JOIN DICH_VU dv ON dv.ten=s.ten_dich_vu
         JOIN TAI_KHOAN tk ON tk.ten_dang_nhap='nhanvien_du_lieu_mau' AND tk.dang_hoat_dong=1;

-- Xoa mem mot dong dich vu da goi, giu nguyen lich su va gia.
UPDATE CHI_TIET_DICH_VU
SET da_xoa=1,
    ghi_chu_xoa='Nhap nham so luong, da xoa mem trong du lieu demo',
    tai_khoan_xoa_id=(SELECT id FROM TAI_KHOAN WHERE ten_dang_nhap='nhanvien_du_lieu_mau'),
    thoi_diem_xoa='2026-10-10 10:00:00'
WHERE chi_tiet_dat_phong_id=(
    SELECT chi_tiet_id FROM _seed_map WHERE ma_dat_phong='DP-H10-003' AND stt=1
)
  AND dich_vu_id=(SELECT id FROM DICH_VU WHERE ten='Giặt ủi');

-- No-show mot phan: phong thu nhat da check-in; phong thu hai khong den.
-- Hai dong cung loai Phong doi, moi dong co tien coc 450.000 VND.
UPDATE CHI_TIET_DAT_PHONG
SET huy_luc='2026-10-25 00:05:00', coc_giu_lai=450000
WHERE id=(SELECT chi_tiet_id FROM _seed_map WHERE ma_dat_phong='DP-H10-004' AND stt=2);
-- Check-out cac dong phong lich su da thuc su luu tru.
-- Tien phong = don gia ngay x so dem, hoac don gia gio x so gio.
UPDATE CHI_TIET_DAT_PHONG
SET checkout_thuc_te = (
    SELECT m.checkout_thuc_te FROM _seed_map m
    WHERE m.chi_tiet_id=CHI_TIET_DAT_PHONG.id
),
    tien_phong = CASE
                     WHEN (SELECT p.che_do FROM PHONG p WHERE p.id=CHI_TIET_DAT_PHONG.phong_id)='THEO_GIO'
                         THEN gia_moi_gio * CAST((
                         SELECT (julianday(dp.ngay_di)-julianday(dp.ngay_den))*24
                         FROM DAT_PHONG dp WHERE dp.id=CHI_TIET_DAT_PHONG.dat_phong_id
                     ) AS INTEGER)
                     ELSE gia_moi_dem * CAST((
                         SELECT julianday(date(dp.ngay_di))-julianday(date(dp.ngay_den))
                         FROM DAT_PHONG dp WHERE dp.id=CHI_TIET_DAT_PHONG.dat_phong_id
                     ) AS INTEGER)
        END,
    tien_dich_vu = COALESCE((
                                SELECT SUM(s.so_luong*s.don_gia)
                                FROM CHI_TIET_DICH_VU s
                                WHERE s.chi_tiet_dat_phong_id=CHI_TIET_DAT_PHONG.id AND s.da_xoa=0
                            ),0)
WHERE id IN (
    SELECT chi_tiet_id FROM _seed_map
    WHERE ma_dat_phong LIKE 'DP-H%' OR ma_dat_phong LIKE 'WI-H%'
)
  AND (SELECT checkout_thuc_te FROM _seed_map m WHERE m.chi_tiet_id=CHI_TIET_DAT_PHONG.id) IS NOT NULL;

-- Lap hoa don cho cac giao dich lich su da ket thuc day du.
INSERT INTO HOA_DON (
    ma_hoa_don, dat_phong_id, thoi_diem_lap,
    tong_tien_phong, tong_tien_dich_vu, tien_coc_con_lai,
    so_tien_con_lai, trang_thai, thoi_diem_thanh_toan
)
SELECT
    'HD-'||dp.ma_dat_phong,
    dp.id,
    datetime(MAX(ct.checkout_thuc_te), '+1 minute'),
    COALESCE(SUM(ct.tien_phong),0),
    COALESCE(SUM(ct.tien_dich_vu),0),
    dp.tien_coc-COALESCE(SUM(ct.coc_giu_lai),0),
    COALESCE(SUM(ct.tien_phong),0)+COALESCE(SUM(ct.tien_dich_vu),0)
        -(dp.tien_coc-COALESCE(SUM(ct.coc_giu_lai),0)),
    'CHO_THANH_TOAN', NULL
FROM DAT_PHONG dp
         JOIN CHI_TIET_DAT_PHONG ct ON ct.dat_phong_id=dp.id
WHERE (dp.ma_dat_phong LIKE 'DP-H%' OR dp.ma_dat_phong LIKE 'WI-H%')
  AND dp.trang_thai='DANG_O'
GROUP BY dp.id
HAVING COUNT(*) = SUM(CASE
                          WHEN ct.checkin_thuc_te IS NOT NULL AND ct.checkout_thuc_te IS NOT NULL
                              AND ct.huy_luc IS NULL AND ct.tien_phong IS NOT NULL AND ct.tien_dich_vu IS NOT NULL THEN 1
                          WHEN ct.checkin_thuc_te IS NULL AND ct.checkout_thuc_te IS NULL AND ct.huy_luc IS NOT NULL THEN 1
                          ELSE 0 END);
-- Thanh toan cac hoa don lich su (gia lap thanh toan sau khi lap hoa don).
UPDATE HOA_DON
SET trang_thai='DA_THANH_TOAN',
    thoi_diem_thanh_toan=datetime(thoi_diem_lap, '+10 minutes')
WHERE trang_thai='CHO_THANH_TOAN';

-- Ket thuc don dep cua cac phong da checkout trong lich su.
-- Tat ca phong lich su duoc dua ve san sang truoc khi tao snapshot thang 11.
UPDATE PHONG SET can_don_dep=0, don_xong_luc=NULL WHERE can_don_dep=1;
-- 8. Tao 10 dat truoc trong thang 11: 6 da check-in, 4 da dat cho ngay 25/11.

-- DP-TH11-001
INSERT INTO DAT_PHONG (
    ma_dat_phong, khach_hang_id, la_vang_lai, ngay_den, ngay_di,
    tien_coc, han_thanh_toan_coc, thoi_diem_coc, trang_thai,
    tk_hoan_ten, tk_hoan_so, tk_hoan_ngan_hang
)
SELECT b.ma_dat_phong, kh.id, 0, b.ngay_den, b.ngay_di,
       (SELECT SUM(lp.gia_theo_ngay) FROM _seed_ct s JOIN LOAI_PHONG lp ON lp.ten_loai=s.ten_loai_phong WHERE s.ma_dat_phong=b.ma_dat_phong),
       b.han_coc, NULL, 'CHO_COC', kh.ho_ten, printf('900000%06d', kh.id), 'Vietcombank'
FROM _seed_dp b JOIN KHACH_HANG kh ON kh.so_dien_thoai=b.so_dien_thoai
WHERE b.ma_dat_phong='DP-TH11-001';

INSERT INTO CHI_TIET_DAT_PHONG (dat_phong_id, loai_phong_id, phong_id, gia_moi_dem, gia_moi_gio)
SELECT dp.id, lp.id, NULL, lp.gia_theo_ngay, lp.gia_theo_gio
FROM _seed_ct s JOIN DAT_PHONG dp ON dp.ma_dat_phong=s.ma_dat_phong
                JOIN LOAI_PHONG lp ON lp.ten_loai=s.ten_loai_phong
WHERE s.ma_dat_phong='DP-TH11-001' ORDER BY s.stt;

UPDATE DAT_PHONG
SET trang_thai='DA_DAT',
    thoi_diem_coc=(SELECT thoi_diem_coc FROM _seed_dp WHERE ma_dat_phong='DP-TH11-001'),
    han_thanh_toan_coc=NULL
WHERE ma_dat_phong='DP-TH11-001';

-- DP-TH11-002
INSERT INTO DAT_PHONG (
    ma_dat_phong, khach_hang_id, la_vang_lai, ngay_den, ngay_di,
    tien_coc, han_thanh_toan_coc, thoi_diem_coc, trang_thai,
    tk_hoan_ten, tk_hoan_so, tk_hoan_ngan_hang
)
SELECT b.ma_dat_phong, kh.id, 0, b.ngay_den, b.ngay_di,
       (SELECT SUM(lp.gia_theo_ngay) FROM _seed_ct s JOIN LOAI_PHONG lp ON lp.ten_loai=s.ten_loai_phong WHERE s.ma_dat_phong=b.ma_dat_phong),
       b.han_coc, NULL, 'CHO_COC', kh.ho_ten, printf('900000%06d', kh.id), 'Vietcombank'
FROM _seed_dp b JOIN KHACH_HANG kh ON kh.so_dien_thoai=b.so_dien_thoai
WHERE b.ma_dat_phong='DP-TH11-002';

INSERT INTO CHI_TIET_DAT_PHONG (dat_phong_id, loai_phong_id, phong_id, gia_moi_dem, gia_moi_gio)
SELECT dp.id, lp.id, NULL, lp.gia_theo_ngay, lp.gia_theo_gio
FROM _seed_ct s JOIN DAT_PHONG dp ON dp.ma_dat_phong=s.ma_dat_phong
                JOIN LOAI_PHONG lp ON lp.ten_loai=s.ten_loai_phong
WHERE s.ma_dat_phong='DP-TH11-002' ORDER BY s.stt;

UPDATE DAT_PHONG
SET trang_thai='DA_DAT',
    thoi_diem_coc=(SELECT thoi_diem_coc FROM _seed_dp WHERE ma_dat_phong='DP-TH11-002'),
    han_thanh_toan_coc=NULL
WHERE ma_dat_phong='DP-TH11-002';
-- DP-TH11-003
INSERT INTO DAT_PHONG (
    ma_dat_phong, khach_hang_id, la_vang_lai, ngay_den, ngay_di,
    tien_coc, han_thanh_toan_coc, thoi_diem_coc, trang_thai,
    tk_hoan_ten, tk_hoan_so, tk_hoan_ngan_hang
)
SELECT b.ma_dat_phong, kh.id, 0, b.ngay_den, b.ngay_di,
       (SELECT SUM(lp.gia_theo_ngay) FROM _seed_ct s JOIN LOAI_PHONG lp ON lp.ten_loai=s.ten_loai_phong WHERE s.ma_dat_phong=b.ma_dat_phong),
       b.han_coc, NULL, 'CHO_COC', kh.ho_ten, printf('900000%06d', kh.id), 'Vietcombank'
FROM _seed_dp b JOIN KHACH_HANG kh ON kh.so_dien_thoai=b.so_dien_thoai
WHERE b.ma_dat_phong='DP-TH11-003';

INSERT INTO CHI_TIET_DAT_PHONG (dat_phong_id, loai_phong_id, phong_id, gia_moi_dem, gia_moi_gio)
SELECT dp.id, lp.id, NULL, lp.gia_theo_ngay, lp.gia_theo_gio
FROM _seed_ct s JOIN DAT_PHONG dp ON dp.ma_dat_phong=s.ma_dat_phong
                JOIN LOAI_PHONG lp ON lp.ten_loai=s.ten_loai_phong
WHERE s.ma_dat_phong='DP-TH11-003' ORDER BY s.stt;

UPDATE DAT_PHONG
SET trang_thai='DA_DAT',
    thoi_diem_coc=(SELECT thoi_diem_coc FROM _seed_dp WHERE ma_dat_phong='DP-TH11-003'),
    han_thanh_toan_coc=NULL
WHERE ma_dat_phong='DP-TH11-003';
-- DP-TH11-004
INSERT INTO DAT_PHONG (
    ma_dat_phong, khach_hang_id, la_vang_lai, ngay_den, ngay_di,
    tien_coc, han_thanh_toan_coc, thoi_diem_coc, trang_thai,
    tk_hoan_ten, tk_hoan_so, tk_hoan_ngan_hang
)
SELECT b.ma_dat_phong, kh.id, 0, b.ngay_den, b.ngay_di,
       (SELECT SUM(lp.gia_theo_ngay) FROM _seed_ct s JOIN LOAI_PHONG lp ON lp.ten_loai=s.ten_loai_phong WHERE s.ma_dat_phong=b.ma_dat_phong),
       b.han_coc, NULL, 'CHO_COC', kh.ho_ten, printf('900000%06d', kh.id), 'Vietcombank'
FROM _seed_dp b JOIN KHACH_HANG kh ON kh.so_dien_thoai=b.so_dien_thoai
WHERE b.ma_dat_phong='DP-TH11-004';

INSERT INTO CHI_TIET_DAT_PHONG (dat_phong_id, loai_phong_id, phong_id, gia_moi_dem, gia_moi_gio)
SELECT dp.id, lp.id, NULL, lp.gia_theo_ngay, lp.gia_theo_gio
FROM _seed_ct s JOIN DAT_PHONG dp ON dp.ma_dat_phong=s.ma_dat_phong
                JOIN LOAI_PHONG lp ON lp.ten_loai=s.ten_loai_phong
WHERE s.ma_dat_phong='DP-TH11-004' ORDER BY s.stt;

UPDATE DAT_PHONG
SET trang_thai='DA_DAT',
    thoi_diem_coc=(SELECT thoi_diem_coc FROM _seed_dp WHERE ma_dat_phong='DP-TH11-004'),
    han_thanh_toan_coc=NULL
WHERE ma_dat_phong='DP-TH11-004';

-- DP-TH11-005
INSERT INTO DAT_PHONG (
    ma_dat_phong, khach_hang_id, la_vang_lai, ngay_den, ngay_di,
    tien_coc, han_thanh_toan_coc, thoi_diem_coc, trang_thai,
    tk_hoan_ten, tk_hoan_so, tk_hoan_ngan_hang
)
SELECT b.ma_dat_phong, kh.id, 0, b.ngay_den, b.ngay_di,
       (SELECT SUM(lp.gia_theo_ngay) FROM _seed_ct s JOIN LOAI_PHONG lp ON lp.ten_loai=s.ten_loai_phong WHERE s.ma_dat_phong=b.ma_dat_phong),
       b.han_coc, NULL, 'CHO_COC', kh.ho_ten, printf('900000%06d', kh.id), 'Vietcombank'
FROM _seed_dp b JOIN KHACH_HANG kh ON kh.so_dien_thoai=b.so_dien_thoai
WHERE b.ma_dat_phong='DP-TH11-005';

INSERT INTO CHI_TIET_DAT_PHONG (dat_phong_id, loai_phong_id, phong_id, gia_moi_dem, gia_moi_gio)
SELECT dp.id, lp.id, NULL, lp.gia_theo_ngay, lp.gia_theo_gio
FROM _seed_ct s JOIN DAT_PHONG dp ON dp.ma_dat_phong=s.ma_dat_phong
                JOIN LOAI_PHONG lp ON lp.ten_loai=s.ten_loai_phong
WHERE s.ma_dat_phong='DP-TH11-005' ORDER BY s.stt;

UPDATE DAT_PHONG
SET trang_thai='DA_DAT',
    thoi_diem_coc=(SELECT thoi_diem_coc FROM _seed_dp WHERE ma_dat_phong='DP-TH11-005'),
    han_thanh_toan_coc=NULL
WHERE ma_dat_phong='DP-TH11-005';
-- DP-TH11-006
INSERT INTO DAT_PHONG (
    ma_dat_phong, khach_hang_id, la_vang_lai, ngay_den, ngay_di,
    tien_coc, han_thanh_toan_coc, thoi_diem_coc, trang_thai,
    tk_hoan_ten, tk_hoan_so, tk_hoan_ngan_hang
)
SELECT b.ma_dat_phong, kh.id, 0, b.ngay_den, b.ngay_di,
       (SELECT SUM(lp.gia_theo_ngay) FROM _seed_ct s JOIN LOAI_PHONG lp ON lp.ten_loai=s.ten_loai_phong WHERE s.ma_dat_phong=b.ma_dat_phong),
       b.han_coc, NULL, 'CHO_COC', kh.ho_ten, printf('900000%06d', kh.id), 'Vietcombank'
FROM _seed_dp b JOIN KHACH_HANG kh ON kh.so_dien_thoai=b.so_dien_thoai
WHERE b.ma_dat_phong='DP-TH11-006';

INSERT INTO CHI_TIET_DAT_PHONG (dat_phong_id, loai_phong_id, phong_id, gia_moi_dem, gia_moi_gio)
SELECT dp.id, lp.id, NULL, lp.gia_theo_ngay, lp.gia_theo_gio
FROM _seed_ct s JOIN DAT_PHONG dp ON dp.ma_dat_phong=s.ma_dat_phong
                JOIN LOAI_PHONG lp ON lp.ten_loai=s.ten_loai_phong
WHERE s.ma_dat_phong='DP-TH11-006' ORDER BY s.stt;

UPDATE DAT_PHONG
SET trang_thai='DA_DAT',
    thoi_diem_coc=(SELECT thoi_diem_coc FROM _seed_dp WHERE ma_dat_phong='DP-TH11-006'),
    han_thanh_toan_coc=NULL
WHERE ma_dat_phong='DP-TH11-006';

-- DP-TH11-007
INSERT INTO DAT_PHONG (
    ma_dat_phong, khach_hang_id, la_vang_lai, ngay_den, ngay_di,
    tien_coc, han_thanh_toan_coc, thoi_diem_coc, trang_thai,
    tk_hoan_ten, tk_hoan_so, tk_hoan_ngan_hang
)
SELECT b.ma_dat_phong, kh.id, 0, b.ngay_den, b.ngay_di,
       (SELECT SUM(lp.gia_theo_ngay) FROM _seed_ct s JOIN LOAI_PHONG lp ON lp.ten_loai=s.ten_loai_phong WHERE s.ma_dat_phong=b.ma_dat_phong),
       b.han_coc, NULL, 'CHO_COC', kh.ho_ten, printf('900000%06d', kh.id), 'Vietcombank'
FROM _seed_dp b JOIN KHACH_HANG kh ON kh.so_dien_thoai=b.so_dien_thoai
WHERE b.ma_dat_phong='DP-TH11-007';

INSERT INTO CHI_TIET_DAT_PHONG (dat_phong_id, loai_phong_id, phong_id, gia_moi_dem, gia_moi_gio)
SELECT dp.id, lp.id, NULL, lp.gia_theo_ngay, lp.gia_theo_gio
FROM _seed_ct s JOIN DAT_PHONG dp ON dp.ma_dat_phong=s.ma_dat_phong
                JOIN LOAI_PHONG lp ON lp.ten_loai=s.ten_loai_phong
WHERE s.ma_dat_phong='DP-TH11-007' ORDER BY s.stt;

UPDATE DAT_PHONG
SET trang_thai='DA_DAT',
    thoi_diem_coc=(SELECT thoi_diem_coc FROM _seed_dp WHERE ma_dat_phong='DP-TH11-007'),
    han_thanh_toan_coc=NULL
WHERE ma_dat_phong='DP-TH11-007';
-- DP-TH11-008
INSERT INTO DAT_PHONG (
    ma_dat_phong, khach_hang_id, la_vang_lai, ngay_den, ngay_di,
    tien_coc, han_thanh_toan_coc, thoi_diem_coc, trang_thai,
    tk_hoan_ten, tk_hoan_so, tk_hoan_ngan_hang
)
SELECT b.ma_dat_phong, kh.id, 0, b.ngay_den, b.ngay_di,
       (SELECT SUM(lp.gia_theo_ngay) FROM _seed_ct s JOIN LOAI_PHONG lp ON lp.ten_loai=s.ten_loai_phong WHERE s.ma_dat_phong=b.ma_dat_phong),
       b.han_coc, NULL, 'CHO_COC', kh.ho_ten, printf('900000%06d', kh.id), 'Vietcombank'
FROM _seed_dp b JOIN KHACH_HANG kh ON kh.so_dien_thoai=b.so_dien_thoai
WHERE b.ma_dat_phong='DP-TH11-008';

INSERT INTO CHI_TIET_DAT_PHONG (dat_phong_id, loai_phong_id, phong_id, gia_moi_dem, gia_moi_gio)
SELECT dp.id, lp.id, NULL, lp.gia_theo_ngay, lp.gia_theo_gio
FROM _seed_ct s JOIN DAT_PHONG dp ON dp.ma_dat_phong=s.ma_dat_phong
                JOIN LOAI_PHONG lp ON lp.ten_loai=s.ten_loai_phong
WHERE s.ma_dat_phong='DP-TH11-008' ORDER BY s.stt;

UPDATE DAT_PHONG
SET trang_thai='DA_DAT',
    thoi_diem_coc=(SELECT thoi_diem_coc FROM _seed_dp WHERE ma_dat_phong='DP-TH11-008'),
    han_thanh_toan_coc=NULL
WHERE ma_dat_phong='DP-TH11-008';

-- DP-TH11-009
INSERT INTO DAT_PHONG (
    ma_dat_phong, khach_hang_id, la_vang_lai, ngay_den, ngay_di,
    tien_coc, han_thanh_toan_coc, thoi_diem_coc, trang_thai,
    tk_hoan_ten, tk_hoan_so, tk_hoan_ngan_hang
)
SELECT b.ma_dat_phong, kh.id, 0, b.ngay_den, b.ngay_di,
       (SELECT SUM(lp.gia_theo_ngay) FROM _seed_ct s JOIN LOAI_PHONG lp ON lp.ten_loai=s.ten_loai_phong WHERE s.ma_dat_phong=b.ma_dat_phong),
       b.han_coc, NULL, 'CHO_COC', kh.ho_ten, printf('900000%06d', kh.id), 'Vietcombank'
FROM _seed_dp b JOIN KHACH_HANG kh ON kh.so_dien_thoai=b.so_dien_thoai
WHERE b.ma_dat_phong='DP-TH11-009';

INSERT INTO CHI_TIET_DAT_PHONG (dat_phong_id, loai_phong_id, phong_id, gia_moi_dem, gia_moi_gio)
SELECT dp.id, lp.id, NULL, lp.gia_theo_ngay, lp.gia_theo_gio
FROM _seed_ct s JOIN DAT_PHONG dp ON dp.ma_dat_phong=s.ma_dat_phong
                JOIN LOAI_PHONG lp ON lp.ten_loai=s.ten_loai_phong
WHERE s.ma_dat_phong='DP-TH11-009' ORDER BY s.stt;

UPDATE DAT_PHONG
SET trang_thai='DA_DAT',
    thoi_diem_coc=(SELECT thoi_diem_coc FROM _seed_dp WHERE ma_dat_phong='DP-TH11-009'),
    han_thanh_toan_coc=NULL
WHERE ma_dat_phong='DP-TH11-009';
-- DP-TH11-010
INSERT INTO DAT_PHONG (
    ma_dat_phong, khach_hang_id, la_vang_lai, ngay_den, ngay_di,
    tien_coc, han_thanh_toan_coc, thoi_diem_coc, trang_thai,
    tk_hoan_ten, tk_hoan_so, tk_hoan_ngan_hang
)
SELECT b.ma_dat_phong, kh.id, 0, b.ngay_den, b.ngay_di,
       (SELECT SUM(lp.gia_theo_ngay) FROM _seed_ct s JOIN LOAI_PHONG lp ON lp.ten_loai=s.ten_loai_phong WHERE s.ma_dat_phong=b.ma_dat_phong),
       b.han_coc, NULL, 'CHO_COC', kh.ho_ten, printf('900000%06d', kh.id), 'Vietcombank'
FROM _seed_dp b JOIN KHACH_HANG kh ON kh.so_dien_thoai=b.so_dien_thoai
WHERE b.ma_dat_phong='DP-TH11-010';

INSERT INTO CHI_TIET_DAT_PHONG (dat_phong_id, loai_phong_id, phong_id, gia_moi_dem, gia_moi_gio)
SELECT dp.id, lp.id, NULL, lp.gia_theo_ngay, lp.gia_theo_gio
FROM _seed_ct s JOIN DAT_PHONG dp ON dp.ma_dat_phong=s.ma_dat_phong
                JOIN LOAI_PHONG lp ON lp.ten_loai=s.ten_loai_phong
WHERE s.ma_dat_phong='DP-TH11-010' ORDER BY s.stt;

UPDATE DAT_PHONG
SET trang_thai='DA_DAT',
    thoi_diem_coc=(SELECT thoi_diem_coc FROM _seed_dp WHERE ma_dat_phong='DP-TH11-010'),
    han_thanh_toan_coc=NULL
WHERE ma_dat_phong='DP-TH11-010';


INSERT INTO _seed_map (ma_dat_phong, stt, chi_tiet_id, so_phong, checkin_thuc_te, checkout_thuc_te)
WITH s AS (
    SELECT ma_dat_phong, stt, so_phong, checkin_thuc_te, checkout_thuc_te,
           ROW_NUMBER() OVER (PARTITION BY ma_dat_phong ORDER BY stt) AS rn
    FROM _seed_ct WHERE ma_dat_phong LIKE 'DP-TH11-%'
), c AS (
    SELECT dp.ma_dat_phong, ct.id,
           ROW_NUMBER() OVER (PARTITION BY dp.id ORDER BY ct.id) AS rn
    FROM CHI_TIET_DAT_PHONG ct JOIN DAT_PHONG dp ON dp.id=ct.dat_phong_id
    WHERE dp.ma_dat_phong LIKE 'DP-TH11-%'
)
SELECT s.ma_dat_phong,s.stt,c.id,s.so_phong,s.checkin_thuc_te,s.checkout_thuc_te
FROM s JOIN c ON c.ma_dat_phong=s.ma_dat_phong AND c.rn=s.rn;
-- Gan 10 phong vat ly cho 10 dat truoc thang 11.
UPDATE CHI_TIET_DAT_PHONG
SET phong_id=(SELECT p.id FROM _seed_map m JOIN PHONG p ON p.so_phong=m.so_phong WHERE m.chi_tiet_id=CHI_TIET_DAT_PHONG.id)
WHERE id IN (SELECT chi_tiet_id FROM _seed_map WHERE ma_dat_phong LIKE 'DP-TH11-%' AND so_phong IS NOT NULL);

-- Check-in 6 dat phong co khach dang o; 4 dat phong con lai giu DA_DAT.
UPDATE CHI_TIET_DAT_PHONG
SET checkin_thuc_te=(SELECT m.checkin_thuc_te FROM _seed_map m WHERE m.chi_tiet_id=CHI_TIET_DAT_PHONG.id)
WHERE id IN (SELECT chi_tiet_id FROM _seed_map WHERE ma_dat_phong LIKE 'DP-TH11-%' AND checkin_thuc_te IS NOT NULL);

-- 9. Tao hai khach vang lai dang thue theo gio trong thang 11.
INSERT INTO DAT_PHONG (ma_dat_phong, khach_hang_id, la_vang_lai, ngay_den, ngay_di, tien_coc, trang_thai)
SELECT b.ma_dat_phong, kh.id, 1, b.ngay_den, b.ngay_di, 0, 'DANG_O'
FROM (
         SELECT 'WI-TH11-001' ma_dat_phong, '0900000011' so_dien_thoai, '2026-11-24 15:00:00' ngay_den, '2026-11-24 19:00:00' ngay_di
         UNION ALL
         SELECT 'WI-TH11-002','0900000012','2026-11-24 15:30:00','2026-11-24 19:30:00'
         UNION ALL
         SELECT 'WI-TH11-003','0900000013','2026-11-24 14:30:00','2026-11-24 17:30:00'
     ) b JOIN KHACH_HANG kh ON kh.so_dien_thoai=b.so_dien_thoai;

INSERT INTO CHI_TIET_DAT_PHONG (
    dat_phong_id, loai_phong_id, phong_id, gia_moi_dem, gia_moi_gio, checkin_thuc_te
)
SELECT dp.id, lp.id, p.id, lp.gia_theo_ngay, lp.gia_theo_gio, s.checkin_thuc_te
FROM _seed_ct s JOIN DAT_PHONG dp ON dp.ma_dat_phong=s.ma_dat_phong AND dp.la_vang_lai=1
                JOIN LOAI_PHONG lp ON lp.ten_loai=s.ten_loai_phong
                JOIN PHONG p ON p.so_phong=s.so_phong
WHERE s.ma_dat_phong LIKE 'WI-TH11-%';
-- Anh xa 3 dong walk-in thang 11.
INSERT INTO _seed_map (ma_dat_phong, stt, chi_tiet_id, so_phong, checkin_thuc_te, checkout_thuc_te)
WITH s AS (
    SELECT ma_dat_phong, stt, so_phong, checkin_thuc_te, checkout_thuc_te,
           ROW_NUMBER() OVER (PARTITION BY ma_dat_phong ORDER BY stt) AS rn
    FROM _seed_ct WHERE ma_dat_phong LIKE 'WI-TH11-%'
), c AS (
    SELECT dp.ma_dat_phong, ct.id,
           ROW_NUMBER() OVER (PARTITION BY dp.id ORDER BY ct.id) AS rn
    FROM CHI_TIET_DAT_PHONG ct JOIN DAT_PHONG dp ON dp.id=ct.dat_phong_id
    WHERE dp.ma_dat_phong LIKE 'WI-TH11-%'
)
SELECT s.ma_dat_phong,s.stt,c.id,s.so_phong,s.checkin_thuc_te,s.checkout_thuc_te
FROM s JOIN c ON c.ma_dat_phong=s.ma_dat_phong AND c.rn=s.rn;

-- Ghi nhan khach o phong cho 6 dat truoc dang o va 2 khach vang lai dang o.
INSERT INTO KHACH_O_PHONG (chi_tiet_dat_phong_id, khach_hang_id)
SELECT m.chi_tiet_id, dp.khach_hang_id
FROM _seed_map m JOIN DAT_PHONG dp ON dp.ma_dat_phong=m.ma_dat_phong
                 JOIN CHI_TIET_DAT_PHONG ct ON ct.id=m.chi_tiet_id
WHERE m.ma_dat_phong LIKE 'DP-TH11-%' AND ct.checkin_thuc_te IS NOT NULL;
INSERT INTO KHACH_O_PHONG (chi_tiet_dat_phong_id, khach_hang_id)
SELECT ct.id, dp.khach_hang_id
FROM DAT_PHONG dp JOIN CHI_TIET_DAT_PHONG ct ON ct.dat_phong_id=dp.id
WHERE dp.ma_dat_phong IN ('WI-TH11-001','WI-TH11-002');
-- Ghi nhan khach o phong 105 truoc khi checkout.
INSERT INTO KHACH_O_PHONG (chi_tiet_dat_phong_id, khach_hang_id)
SELECT ct.id, dp.khach_hang_id
FROM DAT_PHONG dp JOIN CHI_TIET_DAT_PHONG ct ON ct.dat_phong_id=dp.id
WHERE dp.ma_dat_phong='WI-TH11-003';

-- Check-out rieng mot khach vang lai luc 17:30; phong 105 se hien dang don den 18:00.
UPDATE CHI_TIET_DAT_PHONG
SET checkout_thuc_te='2026-11-24 17:30:00', tien_phong=150000, tien_dich_vu=0
WHERE id=(SELECT chi_tiet_id FROM _seed_map WHERE ma_dat_phong='WI-TH11-003' AND stt=1);

INSERT INTO HOA_DON (
    ma_hoa_don, dat_phong_id, thoi_diem_lap, tong_tien_phong,
    tong_tien_dich_vu, tien_coc_con_lai, so_tien_con_lai,
    trang_thai, thoi_diem_thanh_toan
)
SELECT 'HD-WI-TH11-003', dp.id, '2026-11-24 17:31:00', 150000, 0, 0, 150000,
       'CHO_THANH_TOAN', NULL
FROM DAT_PHONG dp WHERE dp.ma_dat_phong='WI-TH11-003';
UPDATE HOA_DON
SET trang_thai='DA_THANH_TOAN', thoi_diem_thanh_toan='2026-11-24 17:40:00'
WHERE ma_hoa_don='HD-WI-TH11-003';
-- Don dep cac bang tam chi dung de nap du lieu mau.
-- Khong xoa du lieu trong 12 bang nghiep vu.
DROP TABLE IF EXISTS _seed_map;
DROP TABLE IF EXISTS _seed_ct;
DROP TABLE IF EXISTS _seed_dp;

COMMIT;
