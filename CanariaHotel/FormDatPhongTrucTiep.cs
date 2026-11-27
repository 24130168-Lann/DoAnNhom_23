using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Windows.Forms;

namespace CanariaHotel
{
    public class FormDatPhongTrucTiep : UserControl
    {
        // Bảng màu lạnh theo đúng concept đã thống nhất: trắng xanh xám + navy.
        private readonly Color _navy = ColorTranslator.FromHtml("#183653");
        private readonly Color _navyDark = ColorTranslator.FromHtml("#142B45");
        private readonly Color _navyMuted = ColorTranslator.FromHtml("#687A91");
        private readonly Color _border = ColorTranslator.FromHtml("#D9E3EF");
        private readonly Color _page = ColorTranslator.FromHtml("#F4F7FB");
        private readonly Color _paleBlue = ColorTranslator.FromHtml("#E8EEF6");
        private readonly Color _selectedFill = ColorTranslator.FromHtml("#F7FAFE");
        private const string FontName = "Segoe UI";

        private readonly DateTime _checkInTime = DateTime.Now;
        private readonly List<RoomTypeSelection> _roomSelections = new List<RoomTypeSelection>();

        private Panel _hourlyCard = null!;
        private Panel _dailyCard = null!;
        private Panel _roomPreview = null!;
        private PictureBox _roomPreviewPicture = null!;
        private Label _roomPreviewPlaceholder = null!;
        private Label _hoursValueLabel = null!;
        private DateTimePicker _returnDateInput = null!;
        private ComboBox _returnTimeInput = null!;

        private Label _summaryMode = null!;
        private Label _summaryRoomType = null!;
        private Label _summaryCheckIn = null!;
        private Label _summaryCheckOut = null!;
        private Label _summaryQuantity = null!;

        private Image? _logoImage;
        private Image? _bannerImage;
        private Image? _standardRoomImage;
        private Image? _doubleRoomImage;
        private Image? _middleRoomImage;
        private Image? _vipRoomImage;

        private bool _hourlySelected = true;
        private int _hours = 1;

        // Form đặt phòng được hiển thị bên trong cửa sổ chính, không mở cửa sổ mới.
        public event EventHandler? BackRequested;

        private sealed class RoomTypeSelection
        {
            public string Name { get; set; } = string.Empty;
            public int HourlyPrice { get; set; }
            public int DailyPrice { get; set; }
            public Panel Card { get; set; } = null!;
            public ComboBox Quantity { get; set; } = null!;
            public Panel ImageHost { get; set; } = null!;
            public bool IsSelected { get; set; }
        }

        private enum IconKind
        {
            Bed,
            Clock,
            Calendar,
            Information,
            Room,
            Hourglass
        }

        public FormDatPhongTrucTiep()
        {
            Dock = DockStyle.Fill;
            BackColor = _page;
            Font = new Font(FontName, 10F, FontStyle.Regular);
            AutoScaleMode = AutoScaleMode.Dpi;
            DoubleBuffered = true;
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw,
                true);

            LoadImageAssets();
            BuildInterface();
            UpdateSummary();
        }

        // -----------------------------------------------------------------
        // ASSETS
        // -----------------------------------------------------------------
        private void LoadImageAssets()
        {
            _logoImage = LoadAsset("logoca.jpg");
            _bannerImage = LoadAsset("sanhkhachsan-banner.jpg") ?? LoadAsset("sanhkhachsan.jpg");

            // Mỗi loại phòng sử dụng một ảnh có sẵn trong Assets.
            _standardRoomImage = LoadAsset("phongkhachsan1.jpg");
            _doubleRoomImage = LoadAsset("phongkhachsan2.jpg");
            _middleRoomImage = LoadAsset("phongkhachsan3.jpg");
            _vipRoomImage = LoadAsset("phongkhachsan.jpg");
        }

        private Image? LoadAsset(string fileName)
        {
            var candidates = new List<string>();
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            candidates.Add(Path.Combine(baseDirectory, "Assets", fileName));
            candidates.Add(Path.Combine(Environment.CurrentDirectory, "Assets", fileName));
            candidates.Add(Path.Combine(baseDirectory, fileName));

            // Khi chạy từ bin/Debug hoặc bin/Release, tìm ngược về thư mục project.
            DirectoryInfo? directory = new DirectoryInfo(baseDirectory);
            for (int level = 0; level < 5 && directory != null; level++)
            {
                candidates.Add(Path.Combine(directory.FullName, "Assets", fileName));
                directory = directory.Parent;
            }

            foreach (string path in candidates.Distinct(StringComparer.OrdinalIgnoreCase))
            {
                if (!File.Exists(path))
                    continue;

                try
                {
                    using Image source = Image.FromFile(path);
                    return new Bitmap(source);
                }
                catch
                {
                    // Bỏ qua ảnh không đọc được và tiếp tục tìm đường dẫn khác.
                }
            }

            return null;
        }

        // -----------------------------------------------------------------
        // ROOT LAYOUT – chia vùng cố định và vùng co giãn để tránh chồng chữ khi đổi kích thước.
        // -----------------------------------------------------------------
                private void BuildInterface()
        {
            var root = new TableLayoutPanel
            {
                Name = "pnlBoCucGoc",
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                BackColor = _page,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };

            // Header và progress gọn hơn để nhường diện tích cho nội dung.
            // Vẫn giữ đủ khoảng thở và footer mảnh theo mẫu tham chiếu.
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 108F));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));

            root.Controls.Add(BuildHeader(), 0, 0);
            root.Controls.Add(BuildProgress(), 0, 1);
            root.Controls.Add(BuildMainContent(), 0, 2);
            root.Controls.Add(BuildFooter(), 0, 3);

            Controls.Add(root);
        }

        // -----------------------------------------------------------------
        // HEADER - THƯƠNG HIỆU VÀ ẢNH SẢNH
        // -----------------------------------------------------------------
                private Control BuildHeader()
        {
            var header = new Panel
            {
                Name = "pnlHeaderCanaria",
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(30, 8, 30, 8)
            };

            // Banner sảnh dùng ảnh thật trong Assets, làm nền mờ nhẹ để
            // logo và slogan vẫn là lớp thông tin chính.
            header.Paint += (_, e) =>
            {
                e.Graphics.Clear(Color.White);
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

                if (_bannerImage != null && header.Width > 520)
                {
                    var destination = new Rectangle(390, 0, Math.Max(1, header.Width - 650), header.Height);
                    DrawImageCover(e.Graphics, _bannerImage, destination, 0.78F);

                    // Làm mềm mép trái ảnh để chuyển mượt từ logo sang ảnh sảnh.
                    using var leftFade = new LinearGradientBrush(
                        new Rectangle(350, 0, 330, header.Height),
                        Color.White,
                        Color.FromArgb(0, Color.White),
                        LinearGradientMode.Horizontal);
                    e.Graphics.FillRectangle(leftFade, 350, 0, 330, header.Height);

                    // Vùng sau slogan sáng hơn để chữ không bị chìm.
                    using var rightFade = new LinearGradientBrush(
                        new Rectangle(Math.Max(390, header.Width - 500), 0, 500, header.Height),
                        Color.FromArgb(0, Color.White),
                        Color.FromArgb(210, Color.White),
                        LinearGradientMode.Horizontal);
                    e.Graphics.FillRectangle(rightFade,
                        Math.Max(390, header.Width - 500), 0, 500, header.Height);
                }
            };

            var logo = new PictureBox
            {
                Name = "picLogoCanaria",
                Size = new Size(82, 86),
                Location = new Point(24, 14),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent,
                Image = _logoImage
            };

            var brand = MakeLabel("Canaria", 34F, _navy, false);
            brand.Font = new Font("Palatino Linotype", 34F, FontStyle.Regular);
            brand.Location = new Point(118, 12);
            brand.Size = new Size(218, 48);
            brand.TextAlign = ContentAlignment.MiddleLeft;

            var brandSub = MakeLabel("B O U T I Q U E   H O T E L", 8.5F, _navyMuted, false);
            brandSub.Location = new Point(125, 62);
            brandSub.Size = new Size(220, 18);
            brandSub.TextAlign = ContentAlignment.MiddleLeft;

            var divider = new Panel
            {
                Name = "pnlPhanCachLogo",
                BackColor = ColorTranslator.FromHtml("#AEBED0"),
                Size = new Size(1, 72),
                Location = new Point(360, 24)
            };

            var tagline = MakeLabel("Trải nghiệm lưu trú\ntrọn vẹn hơn", 20F, _navy, false);
            tagline.Font = new Font("Palatino Linotype", 20F, FontStyle.Italic);
            tagline.Size = new Size(295, 78);
            tagline.Location = new Point(Math.Max(390, header.ClientSize.Width - 325), 16);
            tagline.TextAlign = ContentAlignment.MiddleCenter;
            tagline.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tagline.AutoEllipsis = false;
            header.Resize += (_, _) =>
                tagline.Left = Math.Max(390, header.ClientSize.Width - tagline.Width - 12);

            header.Controls.Add(logo);
            header.Controls.Add(brand);
            header.Controls.Add(brandSub);
            header.Controls.Add(divider);
            header.Controls.Add(tagline);

            var bottomLine = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 1,
                BackColor = _border
            };
            header.Controls.Add(bottomLine);

            return header;
        }

        // -----------------------------------------------------------------
        // PROGRESS INDICATOR
        // -----------------------------------------------------------------
                private Control BuildProgress()
        {
            var progress = new Panel
            {
                Name = "pnlTienTrinh",
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(150, 4, 150, 3)
            };

            var grid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 1,
                BackColor = Color.White,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };

            for (int i = 0; i < 4; i++)
                grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));

            grid.Controls.Add(CreateProgressStep(1, "Hình thức thuê & nhu cầu phòng", true), 0, 0);
            grid.Controls.Add(CreateProgressStep(2, "Chọn phòng", false), 1, 0);
            grid.Controls.Add(CreateProgressStep(3, "Thông tin khách lưu trú", false), 2, 0);
            grid.Controls.Add(CreateProgressStep(4, "Xác nhận", false), 3, 0);

            progress.Controls.Add(grid);
            return progress;
        }

                private Control CreateProgressStep(int number, string title, bool active)
        {
            var cell = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(2, 1, 4, 2)
            };

            // Đường nối giữa các bước, nằm phía sau vòng tròn và nhãn.
            if (number < 4)
            {
                var connector = new Panel
                {
                    BackColor = _border,
                    Height = 1,
                    Width = 64,
                    Location = new Point(0, 29),
                    Anchor = AnchorStyles.Top | AnchorStyles.Right
                };
                cell.Controls.Add(connector);
                cell.Resize += (_, _) =>
                    connector.Left = Math.Max(90, cell.ClientSize.Width - 72);
                connector.SendToBack();
            }

            var circle = new Panel
            {
                Name = "pnlBuoc" + number,
                Size = new Size(44, 44),
                Location = new Point(4, 3),
                BackColor = Color.Transparent
            };
            circle.Paint += (_, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var fill = new SolidBrush(active ? _navy : Color.White);
                using var outline = new Pen(active ? _navy : _border, 1.35F);
                e.Graphics.FillEllipse(fill, 1, 1, 41, 41);
                e.Graphics.DrawEllipse(outline, 1, 1, 41, 41);
                using var font = new Font(FontName, 13F, FontStyle.Bold);
                TextRenderer.DrawText(e.Graphics, number.ToString(), font,
                    new Rectangle(0, 0, 44, 44),
                    active ? Color.White : _navy,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };

            var label = MakeLabel(title, 10.6F, active ? _navy : _navyMuted, active);
            label.Location = new Point(58, 4);
            label.Size = new Size(250, 42);
            label.TextAlign = ContentAlignment.MiddleLeft;
            label.AutoEllipsis = true;
            label.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cell.Resize += (_, _) => label.Width = Math.Max(90, cell.ClientSize.Width - 126);

            cell.Controls.Add(circle);
            cell.Controls.Add(label);

            if (active)
            {
                var underline = new Panel
                {
                    BackColor = _navy,
                    Height = 2,
                    Location = new Point(58, 53),
                    Width = Math.Min(250, Math.Max(100, cell.ClientSize.Width - 130)),
                    Anchor = AnchorStyles.Left | AnchorStyles.Bottom
                };
                cell.Controls.Add(underline);
            }

            return cell;
        }

                private Control BuildMainContent()
        {
            var body = new TableLayoutPanel
            {
                Name = "pnlNoiDungChinh",
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(32, 10, 32, 10),
                BackColor = _page,
                Margin = Padding.Empty
            };

            body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 71F));
            body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 29F));

            body.Controls.Add(BuildLeftContent(), 0, 0);

            // Bảng tóm tắt không kéo dài hết chiều cao màn hình.
            // Giữ đúng tỷ lệ gọn như mẫu: tập trung vào ảnh, thông tin và lưu ý cọc.
            var summaryHost = new Panel
            {
                Name = "pnlKhungTomTat",
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(8, 3, 0, 0),
                Margin = Padding.Empty
            };

            var summary = BuildSummary();
            summary.Dock = DockStyle.Top;
            summary.Height = 574;
            summaryHost.Controls.Add(summary);

            body.Controls.Add(summaryHost, 1, 0);
            return body;
        }

                private Control BuildLeftContent()
        {
            var left = new TableLayoutPanel
            {
                Name = "pnlNoiDungBenTrai",
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 6,
                BackColor = _page,
                Padding = new Padding(10, 0, 22, 0),
                Margin = Padding.Empty
            };

            // Chiều cao được cân theo cửa sổ desktop: tiêu đề 80,
            // lựa chọn thuê 222, tiêu đề loại phòng 42, thẻ phòng có ảnh 216,
            // khoảng thở co giãn và hàng nút 56.
            left.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            left.RowStyles.Add(new RowStyle(SizeType.Absolute, 222F));
            left.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            left.RowStyles.Add(new RowStyle(SizeType.Absolute, 216F));
            left.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            left.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));

            left.Controls.Add(BuildSectionHeading(), 0, 0);
            left.Controls.Add(BuildRentalCards(), 0, 1);
            left.Controls.Add(BuildRoomHeading(), 0, 2);
            left.Controls.Add(BuildRoomCards(), 0, 3);
            left.Controls.Add(new Panel { Dock = DockStyle.Fill, BackColor = _page }, 0, 4);
            left.Controls.Add(BuildActions(), 0, 5);

            return left;
        }

                private Control BuildSectionHeading()
        {
            var panel = new Panel
            {
                Name = "pnlTieuDeHinhThucThue",
                Dock = DockStyle.Fill,
                BackColor = _page
            };

            var bedIcon = CreateIcon(IconKind.Bed, 34);
            bedIcon.Location = new Point(2, 13);
            bedIcon.Size = new Size(38, 36);

            var title = MakeLabel("Chọn hình thức thuê", 22F, _navy, true);
            title.Font = new Font("Palatino Linotype", 22F, FontStyle.Bold);
            title.Location = new Point(50, 7);
            title.Size = new Size(570, 43);
            title.TextAlign = ContentAlignment.MiddleLeft;

            var clockIcon = CreateIcon(IconKind.Clock, 22);
            clockIcon.Location = new Point(9, 57);
            clockIcon.Size = new Size(22, 22);

            var time = MakeLabel(
                "Thời điểm nhận phòng: " + _checkInTime.ToString("HH:mm 'ngày' dd/MM/yyyy") +
                "   (tự động ghi nhận khi xác nhận)",
                10.3F,
                _navyMuted,
                false);
            time.Location = new Point(42, 54);
            time.Size = new Size(820, 27);
            time.TextAlign = ContentAlignment.MiddleLeft;
            time.AutoEllipsis = true;
            time.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            panel.Resize += (_, _) => time.Width = Math.Max(200, panel.ClientSize.Width - 50);

            panel.Controls.Add(bedIcon);
            panel.Controls.Add(title);
            panel.Controls.Add(clockIcon);
            panel.Controls.Add(time);
            return panel;
        }

                private Control BuildRoomHeading()
        {
            var panel = new Panel
            {
                Name = "pnlTieuDeLoaiPhong",
                Dock = DockStyle.Fill,
                BackColor = _page
            };

            var icon = CreateIcon(IconKind.Bed, 32);
            icon.Location = new Point(1, 6);
            icon.Size = new Size(38, 35);

            var label = MakeLabel("Chọn loại phòng và số lượng", 21F, _navy, true);
            label.Font = new Font("Palatino Linotype", 21F, FontStyle.Bold);
            label.Location = new Point(50, 1);
            label.Size = new Size(620, 43);
            label.TextAlign = ContentAlignment.MiddleLeft;

            panel.Controls.Add(icon);
            panel.Controls.Add(label);
            return panel;
        }

        // -----------------------------------------------------------------
        // RENTAL MODE CARDS
        // -----------------------------------------------------------------
        private Control BuildRentalCards()
        {
            var grid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = _page,
                Padding = new Padding(2, 0, 2, 0),
                Margin = Padding.Empty
            };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            _hourlyCard = BuildHourlyCard();
            _dailyCard = BuildDailyCard();

            grid.Controls.Add(_hourlyCard, 0, 0);
            grid.Controls.Add(_dailyCard, 1, 0);
            SetRentalMode(true);
            return grid;
        }

                private Panel BuildHourlyCard()
        {
            var card = CreateSelectableCard(true);
            card.Name = "pnlThueTheoGio";
            card.Margin = new Padding(4, 2, 10, 2);
            card.Padding = new Padding(12, 8, 12, 8);

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 6,
                BackColor = Color.Transparent,
                Padding = new Padding(8, 0, 8, 0),
                Margin = Padding.Empty
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            var icon = CreateIcon(IconKind.Clock, 36);
            icon.Dock = DockStyle.Fill;

            var title = MakeLabel("Thuê theo giờ", 20F, _navy, true);
            title.Font = new Font("Palatino Linotype", 20F, FontStyle.Bold);
            title.Dock = DockStyle.Fill;
            title.TextAlign = ContentAlignment.MiddleCenter;

            var desc = MakeLabel("Phù hợp cho nhu cầu nghỉ ngắn hạn", 10.8F, _navyMuted, false);
            desc.Dock = DockStyle.Fill;
            desc.TextAlign = ContentAlignment.MiddleCenter;

            var hoursTitle = MakeLabel("Số giờ dự kiến lưu trú", 10.7F, _navy, false);
            hoursTitle.Dock = DockStyle.Fill;
            hoursTitle.TextAlign = ContentAlignment.MiddleCenter;

            var stepper = BuildHourStepper();
            stepper.Dock = DockStyle.Fill;

            var notePanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };
            var infoIcon = CreateIcon(IconKind.Information, 19);
            infoIcon.Location = new Point(15, 6);
            infoIcon.Size = new Size(20, 20);

            var note = MakeLabel("Thời gian trả phòng dự kiến phải trong cùng ngày.", 10F, _navyMuted, false);
            note.Location = new Point(43, 2);
            note.Size = new Size(500, 28);
            note.TextAlign = ContentAlignment.MiddleLeft;
            note.AutoEllipsis = true;

            notePanel.Controls.Add(infoIcon);
            notePanel.Controls.Add(note);
            notePanel.Resize += (_, _) =>
            {
                infoIcon.Left = 14;
                note.Left = 42;
                note.Width = Math.Max(120, notePanel.ClientSize.Width - 52);
            };

            layout.Controls.Add(icon, 0, 0);
            layout.Controls.Add(title, 0, 1);
            layout.Controls.Add(desc, 0, 2);
            layout.Controls.Add(hoursTitle, 0, 3);
            layout.Controls.Add(stepper, 0, 4);
            layout.Controls.Add(notePanel, 0, 5);
            card.Controls.Add(layout);

            AttachCardClick(card, () => SetRentalMode(true), stepper);
            return card;
        }

                private Control BuildHourStepper()
        {
            var host = new Panel
            {
                BackColor = Color.Transparent
            };

            var minus = MakeSmallButton("−");
            minus.Size = new Size(56, 38);
            minus.Location = new Point(0, 0);
            minus.Click += (_, _) =>
            {
                SetRentalMode(true);
                if (_hours > 1)
                    _hours--;
                UpdateHoursLabel();
                UpdateSummary();
            };

            var valueBox = new Panel
            {
                Size = new Size(108, 38),
                Location = new Point(56, 0),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            _hoursValueLabel = MakeLabel(_hours.ToString(), 13F, _navy, true);
            _hoursValueLabel.Dock = DockStyle.Fill;
            _hoursValueLabel.TextAlign = ContentAlignment.MiddleCenter;
            valueBox.Controls.Add(_hoursValueLabel);

            var plus = MakeSmallButton("+");
            plus.Size = new Size(56, 38);
            plus.Location = new Point(164, 0);
            plus.Click += (_, _) =>
            {
                SetRentalMode(true);
                int maxHours = Math.Max(1, 23 - _checkInTime.Hour);
                if (_hours < Math.Min(12, maxHours))
                    _hours++;
                UpdateHoursLabel();
                UpdateSummary();
            };

            host.Controls.Add(minus);
            host.Controls.Add(valueBox);
            host.Controls.Add(plus);
            host.Resize += (_, _) =>
            {
                int totalWidth = 220;
                int startX = Math.Max(0, (host.ClientSize.Width - totalWidth) / 2);
                minus.Left = startX;
                valueBox.Left = startX + 56;
                plus.Left = startX + 164;
            };
            return host;
        }

        private void UpdateHoursLabel()
        {
            if (_hoursValueLabel != null)
                _hoursValueLabel.Text = _hours.ToString();
        }

                private Panel BuildDailyCard()
        {
            var card = CreateSelectableCard(false);
            card.Name = "pnlThueTheoNgay";
            card.Margin = new Padding(10, 2, 4, 2);
            card.Padding = new Padding(12, 8, 12, 8);

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 5,
                BackColor = Color.Transparent,
                Padding = new Padding(8, 0, 8, 2),
                Margin = Padding.Empty
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            var icon = CreateIcon(IconKind.Calendar, 36);
            icon.Dock = DockStyle.Fill;

            var title = MakeLabel("Thuê theo ngày", 20F, _navy, true);
            title.Font = new Font("Palatino Linotype", 20F, FontStyle.Bold);
            title.Dock = DockStyle.Fill;
            title.TextAlign = ContentAlignment.MiddleCenter;

            var desc = MakeLabel("Nghỉ dưỡng thoải mái, đầy đủ tiện nghi", 10.8F, _navyMuted, false);
            desc.Dock = DockStyle.Fill;
            desc.TextAlign = ContentAlignment.MiddleCenter;

            var labels = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 2, 0, 0)
            };
            labels.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            labels.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            var dateLabel = MakeLabel("Ngày trả dự kiến", 10.8F, _navy, false);
            dateLabel.Dock = DockStyle.Fill;
            dateLabel.TextAlign = ContentAlignment.BottomLeft;
            dateLabel.Padding = new Padding(12, 0, 0, 3);

            var timeLabel = MakeLabel("Giờ trả dự kiến", 10.8F, _navy, false);
            timeLabel.Dock = DockStyle.Fill;
            timeLabel.TextAlign = ContentAlignment.BottomLeft;
            timeLabel.Padding = new Padding(12, 0, 0, 3);

            labels.Controls.Add(dateLabel, 0, 0);
            labels.Controls.Add(timeLabel, 1, 0);

            var inputs = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            inputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            inputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            _returnDateInput = new DateTimePicker
            {
                Name = "dtpNgayTraDuKien",
                Dock = DockStyle.Fill,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy",
                MinDate = DateTime.Today,
                Value = DateTime.Today.AddDays(1),
                Font = new Font(FontName, 10.7F),
                CalendarFont = new Font(FontName, 10F),
                Margin = new Padding(7, 2, 10, 2)
            };
            _returnDateInput.ValueChanged += (_, _) => SetRentalMode(false);

            _returnTimeInput = new ComboBox
            {
                Name = "cboGioTraDuKien",
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat,
                Font = new Font(FontName, 10.7F),
                BackColor = Color.White,
                Margin = new Padding(10, 2, 7, 2),
                IntegralHeight = false,
                DropDownHeight = 220
            };
            for (int hour = 0; hour < 24; hour++)
                _returnTimeInput.Items.Add(hour.ToString("00") + ":00");
            _returnTimeInput.SelectedItem = "12:00";
            _returnTimeInput.SelectedIndexChanged += (_, _) => SetRentalMode(false);

            inputs.Controls.Add(_returnDateInput, 0, 0);
            inputs.Controls.Add(_returnTimeInput, 1, 0);

            layout.Controls.Add(icon, 0, 0);
            layout.Controls.Add(title, 0, 1);
            layout.Controls.Add(desc, 0, 2);
            layout.Controls.Add(labels, 0, 3);
            layout.Controls.Add(inputs, 0, 4);
            card.Controls.Add(layout);

            AttachCardClick(card, () => SetRentalMode(false), _returnDateInput, _returnTimeInput);
            return card;
        }

        private void SetRentalMode(bool hourly)
        {
            _hourlySelected = hourly;
            UpdateSelectableCard(_hourlyCard, hourly);
            UpdateSelectableCard(_dailyCard, !hourly);
            UpdateSummary();
        }

        // -----------------------------------------------------------------
        // ROOM TYPE CARDS - CLICK CARD TO SELECT
        // -----------------------------------------------------------------
        private Control BuildRoomCards()
        {
            var grid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 1,
                BackColor = _page,
                Padding = new Padding(0, 2, 0, 2),
                Margin = Padding.Empty
            };

            for (int i = 0; i < 4; i++)
                grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));

            grid.Controls.Add(CreateRoomCard("Phòng thường", 50000, 300000), 0, 0);
            grid.Controls.Add(CreateRoomCard("Phòng đôi", 70000, 450000), 1, 0);
            grid.Controls.Add(CreateRoomCard("Phòng tầm trung", 110000, 700000), 2, 0);
            grid.Controls.Add(CreateRoomCard("Phòng VIP", 300000, 2000000), 3, 0);

            return grid;
        }

        private Image? GetRoomTypeImage(string name)
        {
            return name switch
            {
                "Phòng thường" => _standardRoomImage,
                "Phòng đôi" => _doubleRoomImage,
                "Phòng tầm trung" => _middleRoomImage,
                "Phòng VIP" => _vipRoomImage,
                _ => null
            };
        }

                private Panel CreateRoomCard(string name, int hourlyPrice, int dailyPrice)
        {
            var card = CreateSelectableCard(false);
            card.Name = "pnlThe_" + name.Replace(" ", "");
            card.Margin = new Padding(5, 3, 5, 3);
            card.Padding = new Padding(8);

            // Ảnh được đặt ngay trong thẻ, phía trên tên và giá phòng.
            // Tỷ lệ ảnh rộng giúp 4 thẻ đồng nhất, ảnh được crop nhẹ thay vì méo.
            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                BackColor = Color.Transparent,
                Padding = Padding.Empty,
                Margin = Padding.Empty
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 88F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 31F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 43F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            RoomTypeSelection? selection = null;
            Image? roomImage = GetRoomTypeImage(name);

            var imageHost = new Panel
            {
                Name = "pnlAnh" + name.Replace(" ", ""),
                Dock = DockStyle.Fill,
                BackColor = _paleBlue,
                Margin = new Padding(0, 0, 0, 4)
            };
            imageHost.SizeChanged += (_, _) =>
            {
                ApplyRoundedRegion(imageHost, 8);
                imageHost.Invalidate();
            };
            imageHost.Paint += (_, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                if (roomImage != null)
                {
                    DrawImageCover(g, roomImage, imageHost.ClientRectangle, 1F);
                }
                else
                {
                    g.Clear(_paleBlue);
                    using var fallbackFont = new Font(FontName, 8.5F);
                    TextRenderer.DrawText(
                        g,
                        "Chưa có hình ảnh",
                        fallbackFont,
                        imageHost.ClientRectangle,
                        _navyMuted,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }

                // Dấu chọn nhỏ nằm đè lên ảnh, tránh chiếm diện tích nội dung.
                if (selection?.IsSelected == true)
                {
                    var badge = new Rectangle(imageHost.Width - 31, 7, 23, 23);
                    using var badgeBrush = new SolidBrush(_navy);
                    using var badgePen = new Pen(Color.White, 1.5F);
                    g.FillEllipse(badgeBrush, badge);
                    g.DrawEllipse(badgePen, badge);
                    using var checkPen = new Pen(Color.White, 2F)
                    {
                        StartCap = LineCap.Round,
                        EndCap = LineCap.Round
                    };
                    g.DrawLines(checkPen, new[]
                    {
                        new Point(badge.X + 6, badge.Y + 12),
                        new Point(badge.X + 10, badge.Y + 16),
                        new Point(badge.X + 17, badge.Y + 8)
                    });
                }
            };

            var title = MakeLabel(name, 13.7F, _navy, true);
            title.Font = new Font("Palatino Linotype", 13.7F, FontStyle.Bold);
            title.Dock = DockStyle.Fill;
            title.TextAlign = ContentAlignment.MiddleCenter;
            title.AutoEllipsis = true;

            string hourlyText = hourlyPrice.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + "đ/giờ";
            string dailyText = dailyPrice.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + "đ/ngày";
            var price = MakeLabel(hourlyText + Environment.NewLine + dailyText,
                10.5F, _navyMuted, false);
            price.Dock = DockStyle.Fill;
            price.TextAlign = ContentAlignment.MiddleCenter;

            var quantityHost = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };

            var quantity = new ComboBox
            {
                Name = "cboSoLuong" + name.Replace(" ", ""),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat,
                Width = 126,
                Height = 30,
                Enabled = true,
                Font = new Font(FontName, 9.8F),
                BackColor = Color.White,
                IntegralHeight = false,
                DropDownHeight = 180,
                Margin = Padding.Empty
            };
            quantity.Items.Add("Chọn số lượng");
            for (int count = 1; count <= 5; count++)
                quantity.Items.Add(count.ToString());
            quantity.SelectedIndex = 0;

            quantityHost.Controls.Add(quantity);
            quantityHost.Resize += (_, _) =>
            {
                quantity.Left = Math.Max(0, (quantityHost.ClientSize.Width - quantity.Width) / 2);
                quantity.Top = Math.Max(0, (quantityHost.ClientSize.Height - quantity.Height) / 2);
            };

            selection = new RoomTypeSelection
            {
                Name = name,
                HourlyPrice = hourlyPrice,
                DailyPrice = dailyPrice,
                Card = card,
                Quantity = quantity,
                ImageHost = imageHost,
                IsSelected = false
            };
            _roomSelections.Add(selection);

            quantity.SelectedIndexChanged += (_, _) =>
            {
                int count = Math.Max(0, quantity.SelectedIndex);
                selection.IsSelected = count > 0;
                UpdateRoomCardAppearance(selection);
                UpdateSummary();
            };

            layout.Controls.Add(imageHost, 0, 0);
            layout.Controls.Add(title, 0, 1);
            layout.Controls.Add(price, 0, 2);
            layout.Controls.Add(quantityHost, 0, 3);
            card.Controls.Add(layout);

            AttachCardClick(card, () => ToggleRoomType(selection), quantity);
            UpdateRoomCardAppearance(selection);
            return card;
        }

        private void ToggleRoomType(RoomTypeSelection selection)
        {
            selection.IsSelected = !selection.IsSelected;

            if (selection.IsSelected)
            {
                if (selection.Quantity.SelectedIndex <= 0)
                    selection.Quantity.SelectedIndex = 1;
            }
            else
            {
                selection.Quantity.SelectedIndex = 0;
            }

            selection.Quantity.Enabled = selection.IsSelected;
            UpdateRoomCardAppearance(selection);
            UpdateSummary();
        }

        private void UpdateRoomCardAppearance(RoomTypeSelection selection)
        {
            selection.Card.Tag = selection.IsSelected;
            selection.Card.BackColor = selection.IsSelected ? _selectedFill : Color.White;
            selection.Card.Invalidate();
            selection.ImageHost?.Invalidate();
        }

        // -----------------------------------------------------------------
        // RIGHT SUMMARY PANEL
        // -----------------------------------------------------------------
                private Control BuildSummary()
        {
            var summary = new Panel
            {
                Name = "pnlThongTinThuePhong",
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(12),
                Margin = new Padding(0, 0, 0, 0)
            };
            summary.SizeChanged += (_, _) => ApplyRoundedRegion(summary, 16);
            summary.Paint += (_, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var path = CreateRoundedRectangle(
                    new Rectangle(1, 1, Math.Max(1, summary.Width - 3), Math.Max(1, summary.Height - 3)), 16);
                using var pen = new Pen(_border, 1.2F);
                e.Graphics.DrawPath(pen, path);
            };

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 7,
                BackColor = Color.White,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 198F));
            for (int i = 0; i < 5; i++)
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 43F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 112F));

            _roomPreview = BuildRoomPreview();
            layout.Controls.Add(_roomPreview, 0, 0);

            layout.Controls.Add(
                CreateSummaryRow(IconKind.Clock, "Hình thức thuê", "Theo giờ", out _summaryMode), 0, 1);
            layout.Controls.Add(
                CreateSummaryRow(IconKind.Bed, "Loại phòng", "Chưa chọn", out _summaryRoomType), 0, 2);
            layout.Controls.Add(
                CreateSummaryRow(IconKind.Hourglass, "Thời gian nhận phòng", "", out _summaryCheckIn), 0, 3);
            layout.Controls.Add(
                CreateSummaryRow(IconKind.Clock, "Thời gian dự kiến trả", "Chưa xác định", out _summaryCheckOut), 0, 4);
            layout.Controls.Add(
                CreateSummaryRow(IconKind.Room, "Số lượng phòng", "0 phòng", out _summaryQuantity), 0, 5);

            var policyPanel = new Panel
            {
                Name = "pnlLuuYDatCoc",
                Dock = DockStyle.Fill,
                BackColor = _paleBlue,
                Padding = new Padding(14, 10, 13, 9),
                Margin = new Padding(0, 7, 0, 0)
            };
            policyPanel.SizeChanged += (_, _) => ApplyRoundedRegion(policyPanel, 12);

            var infoBadge = new Panel
            {
                Size = new Size(30, 30),
                Location = new Point(8, 12),
                BackColor = _navy
            };
            infoBadge.Paint += (_, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var brush = new SolidBrush(_navy);
                e.Graphics.FillEllipse(brush, 1, 1, 28, 28);
                using var font = new Font("Georgia", 15F, FontStyle.Bold);
                TextRenderer.DrawText(e.Graphics, "i", font,
                    new Rectangle(1, 0, 28, 29), Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };

            var policyTitle = MakeLabel("Không yêu cầu đặt cọc", 14.2F, _navy, true);
            policyTitle.Font = new Font("Palatino Linotype", 14.2F, FontStyle.Bold);
            policyTitle.Location = new Point(48, 8);
            policyTitle.Size = new Size(330, 27);
            policyTitle.TextAlign = ContentAlignment.MiddleLeft;
            policyTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            var policyText = MakeLabel(
                "Tiền phòng sẽ được tính dựa trên thời gian lưu trú thực tế khi quý khách trả phòng.",
                9.6F, _navyMuted, false);
            policyText.Location = new Point(48, 38);
            policyText.Size = new Size(330, 50);
            policyText.TextAlign = ContentAlignment.TopLeft;
            policyText.AutoEllipsis = true;
            policyText.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            policyPanel.Resize += (_, _) =>
            {
                policyTitle.Width = Math.Max(160, policyPanel.ClientSize.Width - 60);
                policyText.Width = Math.Max(160, policyPanel.ClientSize.Width - 60);
            };

            policyPanel.Controls.Add(infoBadge);
            policyPanel.Controls.Add(policyTitle);
            policyPanel.Controls.Add(policyText);
            layout.Controls.Add(policyPanel, 0, 6);

            summary.Controls.Add(layout);
            return summary;
        }

                private Panel BuildRoomPreview()
        {
            var preview = new Panel
            {
                Name = "pnlAnhXemTruoc",
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Margin = new Padding(0, 0, 0, 5)
            };
            preview.SizeChanged += (_, _) => ApplyRoundedRegion(preview, 10);

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = Color.White,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));

            var imageHost = new Panel
            {
                Name = "pnlVungAnhPhong",
                Dock = DockStyle.Fill,
                BackColor = ColorTranslator.FromHtml("#E8EEF5"),
                Margin = Padding.Empty
            };

            _roomPreviewPicture = new PictureBox
            {
                Name = "picAnhPhongXemTruoc",
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Normal,
                BackColor = ColorTranslator.FromHtml("#E8EEF5"),
                Image = GetPreviewImage()
            };
            _roomPreviewPicture.Paint += (_, e) =>
            {
                if (_roomPreviewPicture.Image == null)
                    return;

                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                DrawImageCover(e.Graphics, _roomPreviewPicture.Image,
                    _roomPreviewPicture.ClientRectangle, 1F);
            };

            _roomPreviewPlaceholder = MakeLabel(
                "Hình ảnh phòng sẽ hiển thị tại đây",
                9.8F, _navyMuted, false);
            _roomPreviewPlaceholder.Dock = DockStyle.Fill;
            _roomPreviewPlaceholder.TextAlign = ContentAlignment.MiddleCenter;
            _roomPreviewPlaceholder.Visible = _roomPreviewPicture.Image == null;

            imageHost.Controls.Add(_roomPreviewPicture);
            imageHost.Controls.Add(_roomPreviewPlaceholder);
            _roomPreviewPlaceholder.BringToFront();

            var caption = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = _navy,
                Margin = Padding.Empty
            };
            var captionLabel = MakeLabel("Thông tin thuê phòng", 14.5F, Color.White, true);
            captionLabel.Font = new Font("Palatino Linotype", 14.5F, FontStyle.Bold);
            captionLabel.Dock = DockStyle.Fill;
            captionLabel.Padding = new Padding(12, 0, 0, 0);
            captionLabel.TextAlign = ContentAlignment.MiddleLeft;
            caption.Controls.Add(captionLabel);

            layout.Controls.Add(imageHost, 0, 0);
            layout.Controls.Add(caption, 0, 1);
            preview.Controls.Add(layout);
            return preview;
        }

                private Image? GetPreviewImage()
        {
            // Hiển thị ảnh của loại phòng được chọn đầu tiên.
            var selected = _roomSelections.FirstOrDefault(x => x.IsSelected);
            if (selected != null)
                return GetRoomTypeImage(selected.Name) ??
                       _standardRoomImage ?? _doubleRoomImage ?? _vipRoomImage;

            // Mặc định dùng ảnh phòng thường giống mẫu tham chiếu.
            return _standardRoomImage ?? _doubleRoomImage ?? _vipRoomImage;
        }

                private Panel CreateSummaryRow(
            IconKind kind,
            string name,
            string initialValue,
            out Label valueLabel)
        {
            var row = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };

            var cells = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                BackColor = Color.White,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            cells.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 34F));
            cells.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
            cells.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));

            var icon = CreateIcon(kind, 27);
            icon.Dock = DockStyle.Fill;

            var nameLabel = MakeLabel(name, 9.4F, _navyMuted, false);
            nameLabel.Dock = DockStyle.Fill;
            nameLabel.TextAlign = ContentAlignment.MiddleLeft;
            nameLabel.AutoEllipsis = true;

            valueLabel = MakeLabel(initialValue, 9.35F, _navy, true);
            valueLabel.Dock = DockStyle.Fill;
            valueLabel.TextAlign = ContentAlignment.MiddleRight;
            valueLabel.AutoEllipsis = true;
            valueLabel.Padding = new Padding(2, 0, 3, 0);

            cells.Controls.Add(icon, 0, 0);
            cells.Controls.Add(nameLabel, 1, 0);
            cells.Controls.Add(valueLabel, 2, 0);
            row.Controls.Add(cells);

            var divider = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 1,
                BackColor = _border
            };
            row.Controls.Add(divider);
            divider.BringToFront();
            return row;
        }

        // -----------------------------------------------------------------
        // SUMMARY UPDATES
        // -----------------------------------------------------------------
        private void UpdateSummary()
        {
            if (_summaryMode == null)
                return;

            _summaryMode.Text = _hourlySelected ? "Theo giờ" : "Theo ngày";
            _summaryCheckIn.Text = _checkInTime.ToString("HH:mm 'ngày' dd/MM/yyyy");

            if (_hourlySelected)
            {
                _summaryCheckOut.Text = _checkInTime.AddHours(_hours).ToString("HH:mm 'ngày' dd/MM/yyyy");
            }
            else
            {
                string returnTime = _returnTimeInput?.SelectedItem?.ToString() ?? "12:00";
                _summaryCheckOut.Text = _returnDateInput.Value.ToString("dd/MM/yyyy") + " · " + returnTime;
            }

            var selected = _roomSelections.Where(x => x.IsSelected && x.Quantity.SelectedIndex > 0).ToList();
            int total = selected.Sum(x => x.Quantity.SelectedIndex);

            if (selected.Count == 0)
            {
                _summaryRoomType.Text = "Chưa chọn";
            }
            else if (selected.Count == 1)
            {
                _summaryRoomType.Text = selected[0].Name + " × " + selected[0].Quantity.SelectedIndex;
            }
            else
            {
                _summaryRoomType.Text = selected.Count + " loại phòng";
            }

            _summaryQuantity.Text = total + " phòng";

            if (_roomPreviewPicture != null)
            {
                _roomPreviewPicture.Image = GetPreviewImage();
                _roomPreviewPlaceholder.Visible = _roomPreviewPicture.Image == null;
                _roomPreviewPicture.Invalidate();
                _roomPreviewPlaceholder.Invalidate();
            }
        }

        // -----------------------------------------------------------------
        // ACTIONS AND FOOTER
        // -----------------------------------------------------------------
                private Control BuildActions()
        {
            var grid = new TableLayoutPanel
            {
                Name = "pnlNutDieuHuong",
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = _page,
                Padding = new Padding(0, 1, 0, 0),
                Margin = Padding.Empty
            };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            var backHost = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = _page
            };

            var backButton = MakeButton("‹   Quay lại", false);
            backButton.Name = "btnQuayLai";
            backButton.Size = new Size(240, 58);
            backButton.Location = new Point(0, 0);
            backButton.Anchor = AnchorStyles.Left | AnchorStyles.Top;
            backButton.Click += (_, _) => BackRequested?.Invoke(this, EventArgs.Empty);
            backHost.Controls.Add(backButton);

            var nextHost = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = _page
            };

            var checkButton = MakeButton("⌕   Kiểm tra phòng phù hợp   →", true);
            checkButton.Name = "btnKiemTraPhongPhuHop";
            checkButton.Size = new Size(560, 58);
            checkButton.Location = new Point(0, 0);
            checkButton.Anchor = AnchorStyles.Left | AnchorStyles.Top;

            void ResizeCheckButton()
            {
                int availableWidth = Math.Max(280, nextHost.ClientSize.Width - 8);
                checkButton.Width = Math.Min(560, availableWidth);
                checkButton.Left = Math.Max(0, (nextHost.ClientSize.Width - checkButton.Width) / 2);
            }

            nextHost.Resize += (_, _) => ResizeCheckButton();
            ResizeCheckButton();
            nextHost.Controls.Add(checkButton);
            checkButton.Click += (_, _) => ValidateRoomSelection();

            grid.Controls.Add(backHost, 0, 0);
            grid.Controls.Add(nextHost, 1, 0);
            return grid;
        }

        private void ValidateRoomSelection()
        {
            var selected = _roomSelections.Where(x => x.IsSelected && x.Quantity.SelectedIndex > 0).ToList();
            int total = selected.Sum(x => x.Quantity.SelectedIndex);

            if (selected.Count == 0 || total == 0)
            {
                MessageBox.Show(
                    "Vui lòng nhấn chọn ít nhất một thẻ loại phòng và chọn số lượng phòng.",
                    "Chưa chọn loại phòng",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            if (_hourlySelected && total != 1)
            {
                MessageBox.Show(
                    "Đối với hình thức thuê theo giờ, mỗi lượt đặt chỉ áp dụng cho một phòng. Vui lòng chọn một phòng.",
                    "Số lượng phòng chưa phù hợp",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            if (!_hourlySelected && _returnDateInput.Value.Date <= _checkInTime.Date)
            {
                MessageBox.Show(
                    "Ngày trả phòng phải sau ngày nhận phòng.",
                    "Thời gian trả phòng không hợp lệ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show(
                "Đã ghi nhận hình thức thuê, loại phòng và số lượng phòng.\n\n" +
                "Màn hình tiếp theo sẽ hiển thị các phòng còn trống theo lựa chọn này.",
                "Thông tin lựa chọn",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

                private Control BuildFooter()
        {
            var footer = new Panel
            {
                Name = "pnlChanTrang",
                Dock = DockStyle.Fill,
                BackColor = _page,
                Padding = new Padding(34, 0, 34, 0)
            };

            var line = new Panel
            {
                Dock = DockStyle.Top,
                Height = 1,
                BackColor = _border
            };

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                BackColor = _page,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));

            var leftLine = new Panel
            {
                Height = 1,
                BackColor = ColorTranslator.FromHtml("#B8C6D7"),
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };
            var rightLine = new Panel
            {
                Height = 1,
                BackColor = ColorTranslator.FromHtml("#B8C6D7"),
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };

            var note = MakeLabel(
                "Khách vãng lai không cần đặt cọc  ·  Đặt phòng và check-in được thực hiện cùng lúc",
                12.2F,
                _navy,
                false);
            note.Font = new Font("Palatino Linotype", 12.2F, FontStyle.Italic);
            note.Dock = DockStyle.Fill;
            note.TextAlign = ContentAlignment.MiddleCenter;
            note.AutoEllipsis = true;

            var leftHost = new Panel { Dock = DockStyle.Fill, BackColor = _page };
            var rightHost = new Panel { Dock = DockStyle.Fill, BackColor = _page };
            leftHost.Controls.Add(leftLine);
            rightHost.Controls.Add(rightLine);
            leftHost.Resize += (_, _) =>
            {
                leftLine.Width = Math.Max(20, leftHost.ClientSize.Width - 16);
                leftLine.Left = 0;
                leftLine.Top = leftHost.ClientSize.Height / 2;
            };
            rightHost.Resize += (_, _) =>
            {
                rightLine.Width = Math.Max(20, rightHost.ClientSize.Width - 16);
                rightLine.Left = 16;
                rightLine.Top = rightHost.ClientSize.Height / 2;
            };

            layout.Controls.Add(leftHost, 0, 0);
            layout.Controls.Add(note, 1, 0);
            layout.Controls.Add(rightHost, 2, 0);

            footer.Controls.Add(layout);
            footer.Controls.Add(line);
            return footer;
        }

        // -----------------------------------------------------------------
        // CONTROL HELPERS
        // -----------------------------------------------------------------
        private Panel CreateSelectableCard(bool selected)
        {
            var card = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = selected ? _selectedFill : Color.White,
                Padding = new Padding(5),
                Tag = selected
            };

            card.SizeChanged += (_, _) => ApplyRoundedRegion(card, 13);
            card.Paint += (_, e) =>
            {
                bool isSelected = card.Tag is bool value && value;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var path = CreateRoundedRectangle(
                    new Rectangle(1, 1, Math.Max(1, card.Width - 3), Math.Max(1, card.Height - 3)), 13);
                using var pen = new Pen(isSelected ? _navy : _border, isSelected ? 1.9F : 1.2F);
                e.Graphics.DrawPath(pen, path);

                if (isSelected)
                {
                    using var brush = new SolidBrush(_navy);
                    e.Graphics.FillEllipse(brush, card.Width - 43, 12, 26, 26);
                    using var font = new Font(FontName, 12F, FontStyle.Bold);
                    TextRenderer.DrawText(e.Graphics, "✓", font,
                        new Rectangle(card.Width - 43, 11, 26, 26), Color.White,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
            };

            return card;
        }

        private void UpdateSelectableCard(Panel card, bool selected)
        {
            if (card == null)
                return;

            card.Tag = selected;
            card.BackColor = selected ? _selectedFill : Color.White;
            card.Invalidate();
        }

        private void ApplyRoundedRegion(Control control, int radius)
        {
            if (control.Width < 2 || control.Height < 2)
                return;

            using var path = CreateRoundedRectangle(
                new Rectangle(0, 0, control.Width, control.Height), radius);
            var region = new Region(path);
            var oldRegion = control.Region;
            control.Region = region;
            oldRegion?.Dispose();
        }

        private GraphicsPath CreateRoundedRectangle(Rectangle bounds, int radius)
        {
            int r = Math.Max(1, Math.Min(radius, Math.Min(bounds.Width, bounds.Height) / 2));
            int d = r * 2;
            var path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void AttachCardClick(Panel card, Action action, params Control[] excludedControls)
        {
            var excluded = new HashSet<Control>(excludedControls);

            void Attach(Control control)
            {
                if (excluded.Contains(control))
                    return;

                control.Cursor = Cursors.Hand;
                control.Click += (_, _) => action();

                foreach (Control child in control.Controls)
                    Attach(child);
            }

            Attach(card);
        }

        private Panel CreateIcon(IconKind kind, int size)
        {
            var icon = new Panel
            {
                Size = new Size(size, size),
                BackColor = Color.Transparent
            };

            icon.Paint += (_, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                float scale = Math.Min(icon.ClientSize.Width, icon.ClientSize.Height) / 42F;
                if (scale <= 0F) scale = 1F;
                g.TranslateTransform(
                    (icon.ClientSize.Width - 42F * scale) / 2F,
                    (icon.ClientSize.Height - 42F * scale) / 2F);
                g.ScaleTransform(scale, scale);

                using var pen = new Pen(_navy, 2.2F)
                {
                    StartCap = LineCap.Round,
                    EndCap = LineCap.Round,
                    LineJoin = LineJoin.Round
                };

                switch (kind)
                {
                    case IconKind.Clock:
                        g.DrawEllipse(pen, 7, 7, 28, 28);
                        g.DrawLine(pen, 21, 11, 21, 21);
                        g.DrawLine(pen, 21, 21, 29, 25);
                        break;
                    case IconKind.Calendar:
                        g.DrawRectangle(pen, 7, 10, 28, 25);
                        g.DrawLine(pen, 7, 17, 35, 17);
                        g.DrawLine(pen, 14, 6, 14, 13);
                        g.DrawLine(pen, 28, 6, 28, 13);
                        for (int x = 15; x <= 27; x += 6)
                            for (int y = 23; y <= 29; y += 6)
                                g.DrawEllipse(pen, x - 1, y - 1, 2, 2);
                        break;
                    case IconKind.Bed:
                        g.DrawLine(pen, 5, 31, 37, 31);
                        g.DrawLine(pen, 7, 18, 7, 35);
                        g.DrawLine(pen, 35, 23, 35, 35);
                        g.DrawLine(pen, 7, 24, 35, 24);
                        g.DrawRectangle(pen, 10, 15, 9, 8);
                        g.DrawLine(pen, 19, 22, 35, 22);
                        break;
                    case IconKind.Information:
                        g.DrawEllipse(pen, 8, 8, 26, 26);
                        g.DrawLine(pen, 21, 18, 21, 27);
                        g.DrawEllipse(pen, 20, 12, 2, 2);
                        break;
                    case IconKind.Room:
                        g.DrawRectangle(pen, 8, 8, 26, 26);
                        g.DrawLine(pen, 14, 15, 28, 15);
                        g.DrawLine(pen, 14, 21, 28, 21);
                        g.DrawLine(pen, 14, 27, 28, 27);
                        break;
                    case IconKind.Hourglass:
                        g.DrawLine(pen, 12, 8, 30, 8);
                        g.DrawLine(pen, 12, 34, 30, 34);
                        g.DrawLine(pen, 14, 9, 14, 16);
                        g.DrawLine(pen, 28, 9, 28, 16);
                        g.DrawLine(pen, 14, 16, 21, 22);
                        g.DrawLine(pen, 28, 16, 21, 22);
                        g.DrawLine(pen, 14, 26, 21, 20);
                        g.DrawLine(pen, 28, 26, 21, 20);
                        g.DrawLine(pen, 14, 26, 14, 33);
                        g.DrawLine(pen, 28, 26, 28, 33);
                        break;
                }

                g.ResetTransform();
            };

            return icon;
        }

        private Label MakeLabel(string text, float size, Color color, bool bold)
        {
            return new Label
            {
                Text = text,
                Font = new Font(FontName, size, bold ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = color,
                BackColor = Color.Transparent,
                AutoSize = false,
                UseCompatibleTextRendering = false
            };
        }

        private Button MakeSmallButton(string text)
        {
            var button = new Button
            {
                Text = text,
                BackColor = _paleBlue,
                ForeColor = _navy,
                Font = new Font(FontName, 15F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                UseVisualStyleBackColor = false,
                Cursor = Cursors.Hand,
                Margin = Padding.Empty
            };
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#DCE5EF");
            return button;
        }

        private Button MakeButton(string text, bool primary)
        {
            var button = new Button
            {
                Text = text,
                BackColor = primary ? _navy : Color.White,
                ForeColor = primary ? Color.White : _navy,
                Font = new Font(primary ? "Palatino Linotype" : FontName,
                    primary ? 15.5F : 12.5F,
                    primary ? FontStyle.Bold : FontStyle.Regular),
                FlatStyle = FlatStyle.Flat,
                UseVisualStyleBackColor = false,
                Cursor = Cursors.Hand,
                Margin = Padding.Empty,
                TextAlign = ContentAlignment.MiddleCenter
            };
            button.FlatAppearance.BorderColor = _navy;
            button.FlatAppearance.BorderSize = primary ? 0 : 1;
            button.FlatAppearance.MouseOverBackColor = primary
                ? ColorTranslator.FromHtml("#263E58")
                : ColorTranslator.FromHtml("#F0F3F7");
            button.FlatAppearance.MouseDownBackColor = ColorTranslator.FromHtml("#DCE4ED");
            return button;
        }

        private void DrawImageCover(Graphics graphics, Image image, Rectangle destination, float opacity)
        {
            if (destination.Width <= 0 || destination.Height <= 0)
                return;

            float targetAspect = destination.Width / (float)destination.Height;
            float imageAspect = image.Width / (float)image.Height;
            Rectangle source;

            if (imageAspect > targetAspect)
            {
                int cropWidth = (int)(image.Height * targetAspect);
                source = new Rectangle((image.Width - cropWidth) / 2, 0, cropWidth, image.Height);
            }
            else
            {
                int cropHeight = (int)(image.Width / targetAspect);
                source = new Rectangle(0, (image.Height - cropHeight) / 2, image.Width, cropHeight);
            }

            using var attributes = new ImageAttributes();
            var matrix = new ColorMatrix { Matrix33 = Math.Max(0F, Math.Min(1F, opacity)) };
            attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            graphics.DrawImage(image, destination,
                source.X, source.Y, source.Width, source.Height,
                GraphicsUnit.Pixel, attributes);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_roomPreviewPicture != null)
                    _roomPreviewPicture.Image = null;

                _logoImage?.Dispose();
                _bannerImage?.Dispose();
                _standardRoomImage?.Dispose();
                _doubleRoomImage?.Dispose();
                _middleRoomImage?.Dispose();
                _vipRoomImage?.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
