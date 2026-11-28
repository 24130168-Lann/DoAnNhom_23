using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CanariaHotel
{
    public partial class FormManHinh00 : Form
    {
        private Bitmap? _renderedBackground;
        private Bitmap? _transparentWhaleLogo;

        private Panel? _pnlHeroLeft;
        private Label? _lblBrandTitle;
        private PictureBox? _picWhale;
        private Label? _lblWelcome;
        private Label? _lblSubtitle;

        private Panel? _cardWalkIn;
        private Panel? _cardBooked;
        private Label? _lblFooterNote;
        private LinkLabel? _lnkStaffLogin;
        private FormDatPhongTrucTiep? _walkInScreen;

        private readonly Color _navyDark =
            ColorTranslator.FromHtml("#16283C");

        private readonly Color _textMuted =
            ColorTranslator.FromHtml("#526071");

        private const string FontFamilyName = "Segoe UI";

        // Các thông số điều chỉnh hiệu ứng nền
        private const float PhotoStartRatio = 0.17f;
        private const float FadeStartRatio = 0.21f;
        private const float FadeEndRatio = 0.76f;
        private const float PhotoOpacity = 0.94f;

        public FormManHinh00()
        {
            InitializeComponent();
            BuildCanariaWelcomeScreen();
        }

        private void BuildCanariaWelcomeScreen()
        {
            // 1. CẤU HÌNH FORM
            this.Text = "Canaria";
            this.Size = new Size(1280, 740);
            this.MinimumSize = new Size(1060, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            this.DoubleBuffered = true;

            // Đường dẫn hai hình ảnh
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            string lobbyPath = Path.Combine(
                baseDir,
                "Assets",
                "sanhkhachsan.jpg"
            );

            string logoPath = Path.Combine(
                baseDir,
                "Assets",
                "logoca.jpg"
            );

            // 2. TÁCH NỀN TRẮNG LOGO CÁ VOI
            if (File.Exists(logoPath))
            {
                using var rawLogo = new Bitmap(logoPath);
                _transparentWhaleLogo = RemoveWhiteBackground(rawLogo);
            }

            // 3. KHỐI THƯƠNG HIỆU VÀ LỜI CHÀO
            int heroW = 540;
            int heroH = 310;

            _pnlHeroLeft = new Panel
            {
                BackColor = Color.Transparent,
                Size = new Size(heroW, heroH)
            };

            // Tên khách sạn
            _lblBrandTitle = new Label
            {
                Text = "C A N A R I A   H O T E L",
                Font = new Font(
                    "Palatino Linotype",
                    19F,
                    FontStyle.Regular
                ),
                ForeColor = _navyDark,
                BackColor = Color.Transparent,
                AutoSize = false,
                Size = new Size(heroW, 38),
                Location = new Point(0, 4),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Logo cá voi
            int logoSize = 144;

            _picWhale = new PictureBox
            {
                Image = _transparentWhaleLogo,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent,
                Size = new Size(logoSize, logoSize),
                Location = new Point(
                    (heroW - logoSize) / 2,
                    48
                )
            };

            // Lời chào
            _lblWelcome = new Label
            {
                Text = "Chào mừng quý khách",
                Font = new Font(
                    FontFamilyName,
                    22,
                    FontStyle.Regular
                ),
                ForeColor = _navyDark,
                BackColor = Color.Transparent,
                AutoSize = false,
                Size = new Size(heroW, 48),
                Location = new Point(0, 200),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Dòng mô tả
            _lblSubtitle = new Label
            {
                Text = "—   Chúng tôi luôn sẵn sàng phục vụ bạn   —",
                Font = new Font(
                    FontFamilyName,
                    11,
                    FontStyle.Italic
                ),
                ForeColor = _textMuted,
                BackColor = Color.Transparent,
                AutoSize = false,
                Size = new Size(heroW, 28),
                Location = new Point(0, 246),
                TextAlign = ContentAlignment.MiddleCenter
            };

            _pnlHeroLeft.Controls.AddRange(
                new Control[]
                {
                    _lblBrandTitle,
                    _picWhale,
                    _lblWelcome,
                    _lblSubtitle
                }
            );

            // 4. HAI THẺ LỰA CHỌN
            _cardWalkIn = CreateOptionCard(
                title: "Khách đến trực tiếp",
                subtitle: "Thuê phòng ngay tại khách sạn",
                bgColor: _navyDark,
                titleColor: Color.White,
                subColor: ColorTranslator.FromHtml("#D1D5DB"),
                iconColor: Color.White,
                circleBg: Color.White,
                arrowColor: _navyDark,
                hasBorder: false,
                iconType: CardIconType.Suitcase
            );

            _cardBooked = CreateOptionCard(
                title: "Tôi đã đặt phòng",
                subtitle: "Check-in với đặt phòng của mình",
                bgColor: Color.White,
                titleColor: _navyDark,
                subColor: _textMuted,
                iconColor: _navyDark,
                circleBg: _navyDark,
                arrowColor: Color.White,
                hasBorder: true,
                iconType: CardIconType.CalendarCheck
            );

            // Gắn sự kiện cho toàn bộ thẻ Khách đến trực tiếp
            // Bao gồm cả Panel và các Label nằm bên trong thẻ.
            if (_cardWalkIn != null)
            {
                AttachClickToAllControls(
                    _cardWalkIn,
                    OpenWalkInScreen
                );
            }

            // 5. CHÂN TRANG
            _lblFooterNote = new Label
            {
                Text = "Bạn cần hỗ trợ? Vui lòng liên hệ quầy lễ tân.",
                Font = new Font(
                    FontFamilyName,
                    11.5f,
                    FontStyle.Italic
                ),
                ForeColor = ColorTranslator.FromHtml("#293B50"),
                BackColor = Color.Transparent,
                AutoSize = true
            };

            _lnkStaffLogin = new LinkLabel
            {
                Text = "Đội ngũ Canaria →",
                Font = new Font(
                    FontFamilyName,
                    11.5f,
                    FontStyle.Regular
                ),
                LinkColor = ColorTranslator.FromHtml("#243B53"),
                ActiveLinkColor = ColorTranslator.FromHtml("#16283C"),
                VisitedLinkColor = ColorTranslator.FromHtml("#243B53"),
                LinkBehavior = LinkBehavior.HoverUnderline,
                BackColor = Color.Transparent,
                AutoSize = true
            };

            // 6. ĐƯA CÁC THÀNH PHẦN LÊN FORM
            this.Controls.AddRange(
                new Control[]
                {
                    _pnlHeroLeft,
                    _cardWalkIn,
                    _cardBooked,
                    _lblFooterNote,
                    _lnkStaffLogin
                }
            );

            // 7. BỐ TRÍ GIAO DIỆN
            void RebuildLayout()
            {
                int w = Math.Max(1, this.ClientSize.Width);
                int h = Math.Max(1, this.ClientSize.Height);

                // Kích thước hai thẻ
                int cardW = Math.Max(
                    260,
                    Math.Min(420, (w - 160) / 2)
                );

                int cardH = 195;
                int gap = 36;
                int totalCardW = cardW * 2 + gap;
                int startX = (w - totalCardW) / 2;

                // Khối thương hiệu giữ nguyên vị trí bên trái như ban đầu
                int leftX = Math.Max(
                    36,
                    (int)(w * 0.05f)
                );

                int topY = Math.Max(
                    22,
                    (int)(h * 0.06f)
                );

                if (_pnlHeroLeft != null)
                {
                    _pnlHeroLeft.Location = new Point(leftX, topY);
                }

                // Đặt hai thẻ theo chiều dọc dựa trên khối thương hiệu.
                // Khoảng cách được giới hạn để khi full màn hình,
                // hai thẻ không bị tách quá xa logo và lời chào.
                int desiredCardY = (int)(h * 0.53f);
                int minimumCardY = topY + heroH + 34;
                int maximumCardY = topY + heroH + 105;

                int cardY = Math.Max(
                    minimumCardY,
                    Math.Min(desiredCardY, maximumCardY)
                );

                // Hai thẻ lựa chọn
                if (_cardWalkIn != null)
                {
                    _cardWalkIn.Size = new Size(cardW, cardH);
                    _cardWalkIn.Location = new Point(startX, cardY);
                    UpdateCardLabels(_cardWalkIn, cardW);
                }

                if (_cardBooked != null)
                {
                    _cardBooked.Size = new Size(cardW, cardH);
                    _cardBooked.Location = new Point(
                        startX + cardW + gap,
                        cardY
                    );
                    UpdateCardLabels(_cardBooked, cardW);
                }

                // Dòng hỗ trợ
                if (_lblFooterNote != null)
                {
                    _lblFooterNote.Location = new Point(
                        (w - _lblFooterNote.Width) / 2,
                        cardY + cardH + 26
                    );
                }

                // Liên kết Đội ngũ Canaria
                if (_lnkStaffLogin != null)
                {
                    _lnkStaffLogin.Location = new Point(
                        w - _lnkStaffLogin.Width - 28,
                        h - _lnkStaffLogin.Height - 20
                    );
                }

                // Tạo lại hình nền theo kích thước mới
                Bitmap? newBackground = null;

                if (File.Exists(lobbyPath))
                {
                    using var rawLobby = Image.FromFile(lobbyPath);
                    newBackground = CreateSoftLobbyBackground(
                        rawLobby,
                        w,
                        h
                    );
                }

                var oldBackground = _renderedBackground;
                _renderedBackground = newBackground;
                oldBackground?.Dispose();

                this.Invalidate();
            }

            this.Resize += (s, e) => RebuildLayout();

            this.FormClosed += (s, e) =>
            {
                _renderedBackground?.Dispose();
                _transparentWhaleLogo?.Dispose();
            };

            RebuildLayout();
        }

        // 8. GẮN SỰ KIỆN NHẤN CHO PANEL VÀ CÁC CONTROL CON
        private void AttachClickToAllControls(
            Control control,
            EventHandler handler)
        {
            control.Click += handler;

            foreach (Control child in control.Controls)
            {
                AttachClickToAllControls(child, handler);
            }
        }

        // 9. HIỂN THỊ MÀN HÌNH ĐẶT PHÒNG TRONG CÙNG CỬA SỔ
        private void OpenWalkInScreen(
            object? sender,
            EventArgs e)
        {
            // Tránh tạo thêm một bản màn hình nếu sự kiện bị nhấn lặp.
            if (_walkInScreen != null)
            {
                _walkInScreen.BringToFront();
                return;
            }

            _walkInScreen = new FormDatPhongTrucTiep
            {
                Dock = DockStyle.Fill
            };

            _walkInScreen.BackRequested += (_, _) => CloseWalkInScreen();

            // Đây là UserControl được đặt phủ lên Form hiện tại, không tạo cửa sổ mới.
            this.Controls.Add(_walkInScreen);
            _walkInScreen.BringToFront();
            this.Text = "Canaria Hotel - Đặt phòng trực tiếp";
        }

        // 10. QUAY LẠI TRANG CHỦ TRONG CÙNG CỬA SỔ
        private void CloseWalkInScreen()
        {
            if (_walkInScreen == null)
                return;

            FormDatPhongTrucTiep screen = _walkInScreen;
            _walkInScreen = null;

            this.Controls.Remove(screen);
            screen.Dispose();

            this.Text = "Canaria";
            this.Invalidate();
        }

        // 10. TẠO ẢNH NỀN VỚI HIỆU ỨNG HÒA TRỘN
        private Bitmap CreateSoftLobbyBackground(
            Image lobbyImg,
            int targetW,
            int targetH)
        {
            targetW = Math.Max(1, targetW);
            targetH = Math.Max(1, targetH);

            // Tạo bitmap toàn màn hình
            var result = new Bitmap(
                targetW,
                targetH,
                PixelFormat.Format32bppArgb
            );

            using (Graphics g = Graphics.FromImage(result))
            {
                // Nền trắng hoàn toàn
                g.Clear(Color.White);
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                // Vị trí bắt đầu đặt ảnh
                int imageStartX = (int)(targetW * PhotoStartRatio);
                int imageAreaW = targetW - imageStartX;

                var destinationRect = new Rectangle(
                    imageStartX,
                    0,
                    imageAreaW,
                    targetH
                );

                // Crop ảnh để phủ kín khu vực bên phải,
                // giữ tỷ lệ hình ảnh, không kéo giãn.
                Rectangle sourceRect = CalculateCoverSourceRectangle(
                    lobbyImg,
                    imageAreaW,
                    targetH
                );

                // Vẽ ảnh gốc
                g.DrawImage(
                    lobbyImg,
                    destinationRect,
                    sourceRect,
                    GraphicsUnit.Pixel
                );
            }

            // TẠO MẶT NẠ ĐỘ TRONG SUỐT THEO CHIỀU NGANG
            Rectangle lockRect = new Rectangle(
                0,
                0,
                targetW,
                targetH
            );

            BitmapData data = result.LockBits(
                lockRect,
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb
            );

            try
            {
                int stride = Math.Abs(data.Stride);
                byte[] pixels = new byte[stride * targetH];

                Marshal.Copy(
                    data.Scan0,
                    pixels,
                    0,
                    pixels.Length
                );

                // Điểm bắt đầu và kết thúc chuyển tiếp
                int fadeStartX = (int)(targetW * FadeStartRatio);
                int fadeEndX = (int)(targetW * FadeEndRatio);
                int fadeWidth = Math.Max(1, fadeEndX - fadeStartX);

                for (int x = 0; x < targetW; x++)
                {
                    float progress;

                    if (x <= fadeStartX)
                    {
                        progress = 0f;
                    }
                    else if (x >= fadeEndX)
                    {
                        progress = 1f;
                    }
                    else
                    {
                        progress = (float)(x - fadeStartX) / fadeWidth;
                    }

                    // Smoothstep tạo đường chuyển tiếp tự nhiên
                    float smoothProgress =
                        progress * progress * (3f - 2f * progress);

                    float opacity = smoothProgress * PhotoOpacity;

                    for (int y = 0; y < targetH; y++)
                    {
                        int rowOffset = data.Stride >= 0
                            ? y * stride
                            : (targetH - 1 - y) * stride;

                        int index = rowOffset + x * 4;

                        // Bitmap lưu thứ tự BGRA.
                        // Trộn từng kênh màu với nền trắng.
                        pixels[index] = BlendWithWhite(
                            pixels[index],
                            opacity
                        );

                        pixels[index + 1] = BlendWithWhite(
                            pixels[index + 1],
                            opacity
                        );

                        pixels[index + 2] = BlendWithWhite(
                            pixels[index + 2],
                            opacity
                        );

                        pixels[index + 3] = 255;
                    }
                }

                Marshal.Copy(
                    pixels,
                    0,
                    data.Scan0,
                    pixels.Length
                );
            }
            finally
            {
                result.UnlockBits(data);
            }

            return result;
        }

        // 11. TÍNH VÙNG CROP ẢNH, KHÔNG KÉO GIÃN
        private Rectangle CalculateCoverSourceRectangle(
            Image image,
            int targetWidth,
            int targetHeight)
        {
            float sourceAspect = image.Width / (float)image.Height;
            float targetAspect = targetWidth / (float)targetHeight;

            if (sourceAspect > targetAspect)
            {
                // Ảnh nguồn rộng hơn khu vực đích
                int cropWidth = (int)(image.Height * targetAspect);
                int cropX = (image.Width - cropWidth) / 2;

                return new Rectangle(
                    cropX,
                    0,
                    cropWidth,
                    image.Height
                );
            }
            else
            {
                // Ảnh nguồn cao hơn khu vực đích
                int cropHeight = (int)(image.Width / targetAspect);
                int remainingHeight = image.Height - cropHeight;

                // Dịch nhẹ vùng crop lên trên
                // để giữ được phần thác nước và sảnh.
                int cropY = (int)(remainingHeight * 0.34f);

                return new Rectangle(
                    0,
                    cropY,
                    image.Width,
                    cropHeight
                );
            }
        }

        // 12. TRỘN MÀU ẢNH VỚI NỀN TRẮNG
        private byte BlendWithWhite(
            byte originalColor,
            float opacity)
        {
            int result = (int)(
                255 - (255 - originalColor) * opacity
            );

            return (byte)Math.Clamp(result, 0, 255);
        }

        // 13. VẼ BACKGROUND TRƯỚC CÁC CONTROL
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (_renderedBackground != null)
            {
                e.Graphics.DrawImageUnscaled(
                    _renderedBackground,
                    0,
                    0
                );
            }
            else
            {
                base.OnPaintBackground(e);
            }
        }

        // 14. XÓA NỀN TRẮNG CỦA LOGO CÁ VOI
        private Bitmap RemoveWhiteBackground(Bitmap source)
        {
            var bmp = new Bitmap(
                source.Width,
                source.Height,
                PixelFormat.Format32bppArgb
            );

            for (int y = 0; y < source.Height; y++)
            {
                for (int x = 0; x < source.Width; x++)
                {
                    Color c = source.GetPixel(x, y);

                    if (c.R > 238 && c.G > 238 && c.B > 238)
                    {
                        bmp.SetPixel(
                            x,
                            y,
                            Color.Transparent
                        );
                    }
                    else
                    {
                        bmp.SetPixel(x, y, c);
                    }
                }
            }

            return bmp;
        }

        // 15. CĂN CHỈNH LABEL TRÊN THẺ
        private void UpdateCardLabels(
            Panel card,
            int cardW)
        {
            int labelW = Math.Max(140, cardW - 40);

            foreach (Control ctrl in card.Controls)
            {
                if (ctrl is Label lbl)
                {
                    lbl.Width = labelW;
                    lbl.Left = 20;
                }
            }
        }

        // 16. LOẠI ICON TRÊN HAI THẺ
        private enum CardIconType
        {
            Suitcase,
            CalendarCheck
        }

        // 17. TẠO THẺ LỰA CHỌN
        private Panel CreateOptionCard(
            string title,
            string subtitle,
            Color bgColor,
            Color titleColor,
            Color subColor,
            Color iconColor,
            Color circleBg,
            Color arrowColor,
            bool hasBorder,
            CardIconType iconType)
        {
            var card = new Panel
            {
                BackColor = bgColor,
                Cursor = Cursors.Hand
            };

            // Tạo góc bo tròn
            void UpdateRoundedRegion()
            {
                using var path = CreateRoundedRect(
                    new Rectangle(
                        0,
                        0,
                        Math.Max(1, card.Width - 1),
                        Math.Max(1, card.Height - 1)
                    ),
                    16
                );

                var newRegion = new Region(path);
                var oldRegion = card.Region;
                card.Region = newRegion;
                oldRegion?.Dispose();
            }

            card.SizeChanged += (s, e) => UpdateRoundedRegion();

            // Vẽ icon và viền thẻ
            card.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                using var path = CreateRoundedRect(
                    new Rectangle(
                        0,
                        0,
                        Math.Max(1, card.Width - 1),
                        Math.Max(1, card.Height - 1)
                    ),
                    16
                );

                if (hasBorder)
                {
                    using var pen = new Pen(
                        ColorTranslator.FromHtml("#D8E0E8"),
                        1.5f
                    );

                    e.Graphics.DrawPath(pen, path);
                }

                int cx = card.Width / 2;
                int topY = 26;

                DrawCardVectorIcon(
                    e.Graphics,
                    iconType,
                    iconColor,
                    cx,
                    topY
                );

                // Vòng tròn chứa dấu mũi tên
                int d = 34;

                var circleRect = new Rectangle(
                    card.Width - d - 18,
                    card.Height - d - 16,
                    d,
                    d
                );

                using var circleBrush = new SolidBrush(circleBg);
                e.Graphics.FillEllipse(circleBrush, circleRect);

                using var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                using var arrowFont = new Font(
                    FontFamilyName,
                    14,
                    FontStyle.Bold
                );

                using var arrowBrush = new SolidBrush(arrowColor);

                e.Graphics.DrawString(
                    "›",
                    arrowFont,
                    arrowBrush,
                    circleRect,
                    sf
                );
            };

            // Tiêu đề thẻ
            var lblTitle = new Label
            {
                Text = title,
                Font = new Font(
                    FontFamilyName,
                    14.5f,
                    FontStyle.Bold
                ),
                ForeColor = titleColor,
                BackColor = Color.Transparent,
                AutoSize = false,
                Size = new Size(340, 32),
                Location = new Point(20, 92),
                TextAlign = ContentAlignment.MiddleCenter,
                Cursor = Cursors.Hand
            };

            // Mô tả thẻ
            var lblSub = new Label
            {
                Text = subtitle,
                Font = new Font(
                    FontFamilyName,
                    10,
                    FontStyle.Italic
                ),
                ForeColor = subColor,
                BackColor = Color.Transparent,
                AutoSize = false,
                Size = new Size(340, 26),
                Location = new Point(20, 128),
                TextAlign = ContentAlignment.MiddleCenter,
                Cursor = Cursors.Hand
            };

            card.Controls.Add(lblTitle);
            card.Controls.Add(lblSub);

            UpdateRoundedRegion();

            return card;
        }

        // 18. VẼ ICON VALI VÀ LỊCH
        private void DrawCardVectorIcon(
            Graphics g,
            CardIconType iconType,
            Color color,
            int cx,
            int topY)
        {
            using var pen = new Pen(color, 2.2f)
            {
                StartCap = LineCap.Round,
                EndCap = LineCap.Round,
                LineJoin = LineJoin.Round
            };

            if (iconType == CardIconType.Suitcase)
            {
                g.DrawRectangle(
                    pen,
                    cx - 8,
                    topY,
                    16,
                    8
                );

                using var bodyPath = CreateRoundedRect(
                    new Rectangle(
                        cx - 18,
                        topY + 8,
                        36,
                        38
                    ),
                    5
                );

                g.DrawPath(pen, bodyPath);

                g.DrawLine(
                    pen,
                    cx - 7,
                    topY + 14,
                    cx - 7,
                    topY + 40
                );

                g.DrawLine(
                    pen,
                    cx + 7,
                    topY + 14,
                    cx + 7,
                    topY + 40
                );
            }
            else
            {
                using var calPath = CreateRoundedRect(
                    new Rectangle(
                        cx - 20,
                        topY + 5,
                        40,
                        38
                    ),
                    5
                );

                g.DrawPath(pen, calPath);

                g.DrawLine(
                    pen,
                    cx - 20,
                    topY + 16,
                    cx + 20,
                    topY + 16
                );

                g.DrawLine(
                    pen,
                    cx - 10,
                    topY + 1,
                    cx - 10,
                    topY + 9
                );

                g.DrawLine(
                    pen,
                    cx + 10,
                    topY + 1,
                    cx + 10,
                    topY + 9
                );

                g.DrawLines(
                    pen,
                    new[]
                    {
                        new Point(cx - 8, topY + 28),
                        new Point(cx - 2, topY + 34),
                        new Point(cx + 10, topY + 22)
                    }
                );
            }
        }

        // 19. HÀM TẠO HÌNH CHỮ NHẬT BO GÓC
        private GraphicsPath CreateRoundedRect(
            Rectangle bounds,
            int radius)
        {
            int r = Math.Max(
                1,
                Math.Min(
                    radius,
                    Math.Min(bounds.Width, bounds.Height) / 2
                )
            );

            int d = r * 2;
            var path = new GraphicsPath();

            path.AddArc(
                bounds.X,
                bounds.Y,
                d,
                d,
                180,
                90
            );

            path.AddArc(
                bounds.Right - d,
                bounds.Y,
                d,
                d,
                270,
                90
            );

            path.AddArc(
                bounds.Right - d,
                bounds.Bottom - d,
                d,
                d,
                0,
                90
            );

            path.AddArc(
                bounds.X,
                bounds.Bottom - d,
                d,
                d,
                90,
                90
            );

            path.CloseFigure();
            return path;
        }
    }
}
