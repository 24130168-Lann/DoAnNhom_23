
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
    (48, 1.0),
    (24, 0.5),
    (0, 0.0);


-- 40 PHONG MAU
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



-- KHACH HANG MAU
-- 8 khach hang gia lap

INSERT OR IGNORE INTO KHACH_HANG
(ho_ten, so_dien_thoai, loai_giay_to, so_giay_to)
VALUES
    ('Nguyễn Minh Anh', '0900000001', 'CCCD', '079000000001'),
    ('Trần Quốc Bảo',  '0900000002', 'CCCD', '079000000002'),
    ('Lê Thu Hà',      '0900000003', 'CCCD', '079000000003'),
    ('Phạm Gia Huy',   '0900000004', 'CCCD', '079000000004'),
    ('Võ Ngọc Linh',   '0900000005', 'CCCD', '079000000005'),
    ('Đặng Hoàng Nam', '0900000006', 'CCCD', '079000000006'),
    ('Bùi Khánh Vy',   '0900000007', 'CCCD', '079000000007'),
    ('Đỗ Thành Đạt',   '0900000008', 'CCCD', '079000000008');




-- DAT PHONG MAU 001
-- Dat truoc 1 phong thuong

INSERT INTO DAT_PHONG (
    ma_dat_phong,
    khach_hang_id,
    la_vang_lai,
    ngay_den,
    ngay_di,
    tien_coc,
    han_thanh_toan_coc,
    trang_thai,
    tk_hoan_ten,
    tk_hoan_so,
    tk_hoan_ngan_hang
)
SELECT
    'DP-MAU-001',
    kh.id,
    0,
    date('now', 'localtime', '+1 day') || ' 14:00:00',
    date('now', 'localtime', '+3 days') || ' 12:00:00',
    300000,
    datetime('now', 'localtime', '+10 minutes'),
    'CHO_COC',
    'Nguyen Minh Anh',
    '0000000001',
    'NGAN_HANG_MO_PHONG'
FROM KHACH_HANG kh
WHERE kh.so_dien_thoai = '0900000001'
  AND NOT EXISTS (
    SELECT 1
    FROM DAT_PHONG
    WHERE ma_dat_phong = 'DP-MAU-001'
);

