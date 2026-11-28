using System.Drawing;
using System.Windows.Forms;
using khachSan.Data;

namespace khachSan.GiaoDien;

/// <summary>
/// Giao diện "Báo cáo quản trị" dành cho Chủ khách sạn (BC-01 công suất phòng, BC-02 doanh thu).
/// Số liệu lấy từ SQLite qua DichVuBaoCao; đổi năm ở ô chọn năm thì báo cáo được tính lại.
/// Toàn bộ UI dựng bằng code nên không cần Designer.
/// </summary>
public sealed class BaoCaoKhachSanForm : Form
{
    private readonly NguonDuLieuSqlite _nguon;
    private readonly DichVuBaoCao _dichVu;
    private readonly VaiTro _vaiTro;
    private readonly List<int> _cacNam = new();
    private readonly ToolTip _tip = new() { AutoPopDelay = 15000 };

    // Các control cần cập nhật khi có số liệu
    private ComboBox _cboNam = null!;
    private Label _lblThoiDiem = null!;
    private Panel _host = null!;
    private SectionPanel _secCongSuat = null!, _secDoanhThu = null!;
    private KpiCard _kpiThangNay = null!, _kpiThangTruoc = null!, _kpiThamChieu = null!;
    private KpiCard _kpiDoanhThu = null!, _kpiCoc = null!, _kpiGhiNhan = null!;
    private AlertBanner _banner = null!;
    private InfoNote _noteCongSuat = null!;
    private OccupancyChart _chartCongSuat = null!;
    private RevenueChart _chartDoanhThu = null!;
    private DataGridView _gridCongSuat = null!, _gridDoanhThu = null!;
    private List<CanhBao> _canhBaos = new();

    private const string GhiChuThamChieu = "Tham chiếu theo giờ nhận/trả chuẩn, không phải giới hạn tối đa";

    public BaoCaoKhachSanForm(NguonDuLieuSqlite nguon, VaiTro vaiTro = VaiTro.Chu)
    {
        _nguon = nguon ?? throw new ArgumentNullException(nameof(nguon));
        _vaiTro = vaiTro;
        _dichVu = new DichVuBaoCao(nguon);

        SuspendLayout();

        Text = "Canaria Boutique Hotel – Báo cáo quản trị";
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(1440, 900);
        MinimumSize = new Size(1180, 700);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Theme.Cream;
        Font = Theme.Body;

        // Khung 2x2: [logo | header] / [menu | nội dung]
        var frame = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 2,
            Margin = Padding.Empty,
            BackColor = Theme.Cream,
        };
        frame.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 187));
        frame.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        frame.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));
        frame.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        frame.Controls.Add(new LogoPanel { Dock = DockStyle.Fill, Margin = Padding.Empty }, 0, 0);
        frame.Controls.Add(BuildHeader(), 1, 0);
        frame.Controls.Add(BuildSidebar(), 0, 1);
        frame.Controls.Add(BuildContent(), 1, 1);

        Controls.Add(frame);
        ResumeLayout(false);

        // Tải số liệu sau khi form đã hiện để không làm đứng lúc mở
        Shown += (_, _) => TaiBaoCao();
    }

    // ───────────────────────── Nạp dữ liệu và đổ lên giao diện ─────────────────────────
    private void TaiBaoCao()
    {
        if (_cboNam.SelectedIndex < 0 || _cboNam.SelectedIndex >= _cacNam.Count) return;
        int nam = _cacNam[_cboNam.SelectedIndex];

        try
        {
            UseWaitCursor = true;

            var congSuat = _dichVu.BaoCaoCongSuat(_vaiTro, nam);
            KetQuaCongSuat? namTruoc = nam - 1 >= KiemTraDauVao.NamToiThieu
                ? _dichVu.BaoCaoCongSuat(_vaiTro, nam - 1)
                : null;
            var doanhThu = _dichVu.BaoCaoDoanhThu(_vaiTro, nam);

            HienCongSuat(congSuat, namTruoc, nam);
            HienDoanhThu(doanhThu);
            HienCanhBao(congSuat, doanhThu);
            _lblThoiDiem.Text = congSuat.ThoiDiemBaoCao.ToString("dd/MM/yyyy HH:mm");
        }
        catch (UnauthorizedAccessException ex)
        {
            MessageBox.Show(this, ex.Message, "Không có quyền xem báo cáo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, "Không tải được báo cáo:\n\n" + ex.Message, "Lỗi",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            UseWaitCursor = false;
        }
    }

    private void HienCongSuat(KetQuaCongSuat cs, KetQuaCongSuat? namTruoc, int nam)
    {
        var thang = cs.CacThang;

        // Tháng mới nhất đã có công suất
        int idx = thang.FindLastIndex(t => t.CongSuatHienThi.HasValue);
        double? thangNay = idx >= 0 ? thang[idx].CongSuatHienThi : null;
        double? thangTruoc = idx > 0
            ? thang[idx - 1].CongSuatHienThi
            : idx == 0 && namTruoc is not null && namTruoc.CacThang.Count == 12
                ? namTruoc.CacThang[11].CongSuatHienThi
                : null;

        bool laThangHienTai = nam == DateTime.Now.Year && idx == DateTime.Now.Month - 1;
        _kpiThangNay.Caption = idx < 0 ? "Công suất tháng này"
            : laThangHienTai ? "Công suất tháng này" : $"Công suất tháng {idx + 1}";
        _kpiThangNay.Value = Phan(thangNay);
        _kpiThangTruoc.Value = Phan(thangTruoc);
        _kpiThamChieu.Value = Phan(cs.MucThamChieu);

        _noteCongSuat.Text = cs.KhongCoPhong
            ? "Danh mục phòng đang trống nên chưa tính được công suất"
            : GhiChuThamChieu;
        _noteCongSuat.Invalidate();

        // Biểu đồ: năm đang xem và năm trước (null = tháng chưa có số liệu)
        _chartCongSuat.Current = thang.Select(t => t.CongSuatHienThi).ToArray();
        _chartCongSuat.Previous = namTruoc is null
            ? new double?[12]
            : namTruoc.CacThang.Select(t => t.CongSuatHienThi).ToArray();
        _chartCongSuat.Invalidate();

        // Bảng chi tiết
        _gridCongSuat.Rows.Clear();
        foreach (var t in thang)
        {
            if (t.TrangThai == TrangThaiThang.DaTinh)
            {
                _gridCongSuat.Rows.Add($"Tháng {t.Thang}", So(t.GioPhongCoKhach), So(t.GioPhongDanhNghia), Phan(t.CongSuatHienThi));
            }
            else
            {
                _gridCongSuat.Rows.Add($"Tháng {t.Thang}", "—", "—", "—");
            }
        }
        ReportGrid.AddTotal(_gridCongSuat, "Tổng năm", So(cs.TongGioPhongCoKhach), So(cs.TongGioPhongDanhNghia),
            Phan(cs.CongSuatNamHienThi));
        ReportGrid.FitHeight(_gridCongSuat);
    }

    private void HienDoanhThu(KetQuaDoanhThu dt)
    {
        var tong = dt.TongNam;
        _kpiDoanhThu.Value = TienGon(tong.TongDoanhThu);
        _kpiCoc.Value = TienGon(tong.CocGiuLai);
        _kpiGhiNhan.Value = TienGon(tong.TongGiaTriGhiNhan);

        // Chọn đơn vị trục (tỷ hoặc triệu) theo giá trị lớn nhất để cột không quá bé
        long lonNhat = dt.CacThang.Where(t => t.DaBatDau)
            .Select(t => Math.Max(t.DoanhThuPhong, Math.Max(t.DoanhThuDichVu, t.CocGiuLai)))
            .DefaultIfEmpty(0L).Max();
        double heSo = lonNhat >= 1_000_000_000L ? 1e9 : 1e6;
        ChonTruc(lonNhat / heSo, out double tran, out double buoc);

        double?[] Cot(Func<KetQuaDoanhThuThang, long> lay) =>
            dt.CacThang.Select(t => t.DaBatDau ? (double?)(lay(t) / heSo) : null).ToArray();

        _chartDoanhThu.Room = Cot(t => t.DoanhThuPhong);
        _chartDoanhThu.Service = Cot(t => t.DoanhThuDichVu);
        _chartDoanhThu.Deposit = Cot(t => t.CocGiuLai);
        _chartDoanhThu.MaxValue = tran;
        _chartDoanhThu.Step = buoc;
        _chartDoanhThu.Unit = heSo >= 1e9 ? "tỷ" : "triệu";
        _chartDoanhThu.Invalidate();

        _gridDoanhThu.Rows.Clear();
        foreach (var t in dt.CacThang)
        {
            if (t.DaBatDau)
            {
                _gridDoanhThu.Rows.Add($"Tháng {t.Thang}", Vnd(t.DoanhThuPhong), Vnd(t.DoanhThuDichVu),
                    Vnd(t.TongDoanhThu), Vnd(t.CocGiuLai), Vnd(t.TongGiaTriGhiNhan));
            }
            else
            {
                _gridDoanhThu.Rows.Add($"Tháng {t.Thang}", "—", "—", "—", "—", "—");
            }
        }
        ReportGrid.AddTotal(_gridDoanhThu, "Tổng năm", Vnd(tong.DoanhThuPhong), Vnd(tong.DoanhThuDichVu),
            Vnd(tong.TongDoanhThu), Vnd(tong.CocGiuLai), Vnd(tong.TongGiaTriGhiNhan));
        ReportGrid.FitHeight(_gridDoanhThu);
    }

    private void HienCanhBao(KetQuaCongSuat cs, KetQuaDoanhThu dt)
    {
        _canhBaos = cs.CanhBaos.Concat(dt.CanhBaos).Distinct().ToList();

        _banner.Visible = _canhBaos.Count > 0;
        if (_canhBaos.Count == 0) return;

        _banner.Text = $"Có dữ liệu cần kiểm tra ({_canhBaos.Count})";
        _banner.Invalidate();
        _tip.SetToolTip(_banner, "Bấm để xem chi tiết");
    }

    private void HienChiTietCanhBao()
    {
        if (_canhBaos.Count == 0) return;

        const int toiDa = 20;
        var dong = _canhBaos.Take(toiDa).Select(c => $"• [{c.Loai}] {c.NoiDung}");
        string them = _canhBaos.Count > toiDa ? $"\n… và {_canhBaos.Count - toiDa} cảnh báo khác." : "";
        MessageBox.Show(this, string.Join("\n", dong) + them, "Dữ liệu cần kiểm tra",
            MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    // ───────────────────────── Định dạng số ─────────────────────────
    private static string Phan(double? v) => v is double x ? x.ToString("0.0", Theme.Vi) + "%" : "—";
    private static string So(double v) => Math.Round(v).ToString("N0", Theme.Vi);
    private static string Vnd(long v) => v.ToString("N0", Theme.Vi);

    private static string TienGon(long v)
    {
        double a = Math.Abs((double)v);
        if (a >= 1e9) return (v / 1e9).ToString("0.###", Theme.Vi) + " tỷ đ";
        if (a >= 1e6) return (v / 1e6).ToString("0.##", Theme.Vi) + " triệu đ";
        return v.ToString("N0", Theme.Vi) + " đ";
    }

    /// <summary>Chọn trần và bước trục sao cho có không quá 5 vạch và số tròn.</summary>
    private static void ChonTruc(double max, out double tran, out double buoc)
    {
        if (max <= 0) { tran = 1; buoc = 1; return; }

        double bac = Math.Pow(10, Math.Floor(Math.Log10(max)));
        buoc = bac * 10;
        foreach (double h in new[] { 1d, 2d, 2.5d, 5d, 10d })
        {
            if (max / (bac * h) <= 5) { buoc = bac * h; break; }
        }
        tran = Math.Ceiling(max / buoc) * buoc;
    }

    // ───────────────────────── Header ─────────────────────────
    private Control BuildHeader()
    {
        var header = new Panel { Dock = DockStyle.Fill, BackColor = Theme.Cream, Margin = Padding.Empty };

        var title = new Label
        {
            Text = "BÁO CÁO QUẢN TRỊ",
            Font = Theme.Title,
            ForeColor = Theme.Navy,
            AutoSize = true,
            Location = new Point(46, 12),
        };
        var subtitle = new Label
        {
            Text = "Dữ liệu tổng hợp theo tháng  •  Chỉ dành cho Chủ khách sạn",
            Font = Theme.Subtitle,
            ForeColor = Theme.TextDark,
            AutoSize = true,
            Location = new Point(48, 62),
        };

        // Cụm bên phải: chọn năm + thời điểm cập nhật
        var right = new Panel { Dock = DockStyle.Right, Width = 500, BackColor = Theme.Cream };

        var yearBox = new RoundedPanel
        {
            Size = new Size(150, 42),
            Location = new Point(40, 30),
            CornerRadius = 10,
            FillColor = Theme.Paper,
        };
        yearBox.Controls.Add(new Label
        {
            Text = "\U0001F4C5",
            Font = Theme.Icon,
            ForeColor = Theme.Navy,
            AutoSize = false,
            Size = new Size(34, 42),
            Location = new Point(6, 0),
            TextAlign = ContentAlignment.MiddleCenter,
        });

        _cboNam = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            FlatStyle = FlatStyle.Flat,
            Font = Theme.Combo,
            BackColor = Theme.Paper,
            Location = new Point(42, 8),
            Width = 102,
        };
        // Danh sách năm lấy từ dữ liệu: từ năm có đặt phòng sớm nhất đến năm hiện tại
        try { _cacNam.AddRange(_nguon.CacNamCoDuLieu(DateTime.Now)); }
        catch { /* DB chưa sẵn sàng: dùng năm hiện tại, lỗi sẽ được báo khi tải báo cáo */ }
        if (_cacNam.Count == 0) _cacNam.Add(DateTime.Now.Year);

        foreach (int n in _cacNam) _cboNam.Items.Add($"Năm {n}");
        _cboNam.SelectedIndex = _cacNam.Count - 1;
        _cboNam.SelectedIndexChanged += (_, _) => TaiBaoCao();
        yearBox.Controls.Add(_cboNam);

        right.Controls.Add(yearBox);
        right.Controls.Add(new Label
        {
            Text = "\u23F1",
            Font = Theme.Icon,
            ForeColor = Theme.Navy,
            AutoSize = false,
            Size = new Size(32, 32),
            Location = new Point(226, 28),
            TextAlign = ContentAlignment.MiddleCenter,
        });
        right.Controls.Add(new Label
        {
            Text = "Cập nhật tại thời điểm xem",
            Font = Theme.Body,
            ForeColor = Theme.TextDark,
            AutoSize = true,
            Location = new Point(262, 26),
        });
        _lblThoiDiem = new Label
        {
            Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
            Font = Theme.Subtitle,
            ForeColor = Theme.TextMuted,
            AutoSize = true,
            Location = new Point(262, 46),
        };
        right.Controls.Add(_lblThoiDiem);

        header.Controls.Add(title);
        header.Controls.Add(subtitle);
        header.Controls.Add(right);
        return header;
    }

    // ───────────────────────── Menu trái ─────────────────────────
    private Control BuildSidebar()
    {
        var sidebar = new Panel { Dock = DockStyle.Fill, BackColor = Theme.Navy, Margin = Padding.Empty };

        var items = new (string Icon, string Text, bool Selected)[]
        {
            ("\U0001F3E0", "Tổng quan", false),
            ("\U0001F6CF", "Công suất phòng", true),
            ("\U0001F4CA", "Doanh thu", false),
        };

        var buttons = new List<SidebarButton>();
        for (int i = 0; i < items.Length; i++)
        {
            var btn = new SidebarButton
            {
                Icon = items[i].Icon,
                Text = items[i].Text,
                Selected = items[i].Selected,
                Location = new Point(0, 44 + i * 54),
                Size = new Size(181, 50),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Cursor = Cursors.Hand,
            };
            int index = i;
            btn.Click += (_, _) =>
            {
                foreach (var b in buttons) b.Selected = ReferenceEquals(b, btn);
                // Tổng quan: về đầu trang; hai mục còn lại: cuộn tới phần tương ứng
                if (index == 0) _host.AutoScrollPosition = new Point(0, 0);
                else _host.ScrollControlIntoView(index == 1 ? _secCongSuat : _secDoanhThu);
            };
            buttons.Add(btn);
            sidebar.Controls.Add(btn);
        }
        return sidebar;
    }

    // ───────────────────────── Nội dung (cuộn dọc) ─────────────────────────
    private Control BuildContent()
    {
        _host = new Panel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            BackColor = Theme.Cream,
            Margin = Padding.Empty,
            Padding = new Padding(16, 12, 14, 8),
        };

        var footer = new Panel { Dock = DockStyle.Top, Height = 40, BackColor = Theme.Cream };
        footer.Controls.Add(new Label
        {
            Text = "\U0001F464  Chủ khách sạn – Báo cáo  →",
            Font = Theme.Body,
            ForeColor = Theme.TextDark,
            Dock = DockStyle.Right,
            Width = 300,
            TextAlign = ContentAlignment.MiddleRight,
        });

        _secCongSuat = BuildOccupancySection();
        _secDoanhThu = BuildRevenueSection();

        // Dock = Top: control add SAU nằm TRÊN, nên add theo thứ tự ngược với hiển thị.
        _host.Controls.Add(footer);
        _host.Controls.Add(_secDoanhThu);
        _host.Controls.Add(new Panel { Dock = DockStyle.Top, Height = 14, BackColor = Theme.Cream });
        _host.Controls.Add(_secCongSuat);
        return _host;
    }

    // ───────────────────────── BC-01 · Công suất phòng ─────────────────────────
    private SectionPanel BuildOccupancySection()
    {
        var section = new SectionPanel { TabText = "BC-01 • CÔNG SUẤT PHÒNG", Dock = DockStyle.Top, Height = 476 };

        // Hàng thẻ chỉ số (giá trị thật được điền trong HienCongSuat)
        _kpiThangNay = NewKpi("\U0001F6CF", "Công suất tháng này", "—");
        _kpiThangTruoc = NewKpi("\U0001F4C5", "Tháng trước", "—");
        _kpiThamChieu = NewKpi("\u2605", "Mức tham chiếu", "—");

        _banner = new AlertBanner
        {
            Text = "Có dữ liệu cần kiểm tra",
            Dock = DockStyle.Fill,
            Margin = Padding.Empty,
            Visible = false,        // chỉ hiện khi dịch vụ báo cáo trả về cảnh báo
            Cursor = Cursors.Hand,
        };
        _banner.Click += (_, _) => HienChiTietCanhBao();

        var kpis = NewTable(new float[] { 24, 22, 24, 30 }, Fill());
        kpis.Controls.Add(_kpiThangNay, 0, 0);
        kpis.Controls.Add(_kpiThangTruoc, 1, 0);
        kpis.Controls.Add(_kpiThamChieu, 2, 0);
        kpis.Controls.Add(_banner, 3, 0);

        _noteCongSuat = new InfoNote
        {
            Text = GhiChuThamChieu,
            Dock = DockStyle.Fill,
            Margin = Padding.Empty,
        };

        // Biểu đồ + bảng
        _chartCongSuat = new OccupancyChart
        {
            Dock = DockStyle.Fill,
            Margin = Padding.Empty,
            Categories = SampleData.Months,
            Current = new double?[12],
            Previous = new double?[12],
        };

        _gridCongSuat = ReportGrid.Create(
            new[] { "Tháng", "Giờ-phòng có khách", "Giờ-phòng danh nghĩa", "Công suất" },
            new float[] { 22, 28, 30, 20 }, headerHeight: 38, rowHeight: 19);

        var gridHost = new Panel { Dock = DockStyle.Fill, Margin = Padding.Empty, Padding = new Padding(14, 0, 0, 0) };
        gridHost.Controls.Add(_gridCongSuat);

        var split = NewTable(new float[] { 66, 34 }, Fill());
        split.Controls.Add(_chartCongSuat, 0, 0);
        split.Controls.Add(gridHost, 1, 0);

        var body = NewTable(new float[] { 100 }, Abs(74), Abs(32), Fill());
        body.Controls.Add(kpis, 0, 0);
        body.Controls.Add(_noteCongSuat, 0, 1);
        body.Controls.Add(split, 0, 2);

        section.Controls.Add(body);
        return section;
    }

    // ───────────────────────── BC-02 · Doanh thu ─────────────────────────
    private SectionPanel BuildRevenueSection()
    {
        // Cao hơn bản mẫu vì bảng giờ hiển thị đủ 12 tháng + dòng tổng năm
        var section = new SectionPanel { TabText = "BC-02 • DOANH THU", Dock = DockStyle.Top, Height = 660 };

        _kpiDoanhThu = NewKpi("\U0001F4B0", "Tổng doanh thu", "—");
        _kpiCoc = NewKpi("\U0001F6E1", "Cọc giữ lại", "—");
        _kpiGhiNhan = NewKpi("\U0001F4C8", "Tổng giá trị ghi nhận", "—");

        var kpis = NewTable(new float[] { 23, 23, 23, 31 }, Fill());
        kpis.Controls.Add(_kpiDoanhThu, 0, 0);
        kpis.Controls.Add(_kpiCoc, 1, 0);
        kpis.Controls.Add(_kpiGhiNhan, 2, 0);

        _chartDoanhThu = new RevenueChart
        {
            Dock = DockStyle.Fill,
            Margin = Padding.Empty,
            Categories = SampleData.Months,
            Room = new double?[12],
            Service = new double?[12],
            Deposit = new double?[12],
            MaxValue = 1,
            Step = 1,
            Unit = "triệu",
        };

        _gridDoanhThu = ReportGrid.Create(
            new[] { "Tháng", "Doanh thu phòng", "Doanh thu dịch vụ", "Tổng doanh thu", "Cọc giữ lại", "Tổng giá trị ghi nhận" },
            new float[] { 12, 19, 19, 19, 15, 19 }, headerHeight: 28, rowHeight: 21);

        var gridHost = new Panel { Dock = DockStyle.Fill, Margin = Padding.Empty };
        gridHost.Controls.Add(_gridDoanhThu);

        var note = new InfoNote
        {
            Text = "Tổng giá trị ghi nhận = Tổng doanh thu + Cọc giữ lại; không phải tiền mặt thực nhận.",
            Dock = DockStyle.Fill,
            Margin = Padding.Empty,
        };

        var body = NewTable(new float[] { 100 }, Abs(66), Abs(170), Abs(310), Fill());
        body.Controls.Add(kpis, 0, 0);
        body.Controls.Add(_chartDoanhThu, 0, 1);
        body.Controls.Add(gridHost, 0, 2);
        body.Controls.Add(note, 0, 3);

        section.Controls.Add(body);
        return section;
    }

    // ───────────────────────── Hàm tiện ích dựng giao diện ─────────────────────────
    private static RowStyle Abs(float height) => new(SizeType.Absolute, height);
    private static RowStyle Fill() => new(SizeType.Percent, 100);

    /// <summary>TableLayoutPanel: cột chia theo % (colPercents), hàng theo rows.</summary>
    private static TableLayoutPanel NewTable(float[] colPercents, params RowStyle[] rows)
    {
        var table = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Margin = Padding.Empty,
            Padding = Padding.Empty,
            ColumnCount = colPercents.Length,
            RowCount = rows.Length,
        };
        foreach (var percent in colPercents) table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, percent));
        foreach (var row in rows) table.RowStyles.Add(row);
        return table;
    }

    private static KpiCard NewKpi(string icon, string caption, string value) => new()
    {
        Icon = icon,
        Caption = caption,
        Value = value,
        Dock = DockStyle.Fill,
        Margin = new Padding(0, 0, 12, 0),
    };
}