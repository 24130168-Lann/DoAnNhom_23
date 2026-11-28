using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;

namespace khachSan.GiaoDien;

/// <summary>Màu sắc, font và định dạng số dùng chung cho toàn bộ giao diện báo cáo.</summary>
internal static class Theme
{
    // ── Màu ──────────────────────────────────────────────
    public static readonly Color Navy      = Color.FromArgb(30, 47, 87);
    public static readonly Color Gold      = Color.FromArgb(224, 185, 113);
    public static readonly Color GoldSoft  = Color.FromArgb(246, 233, 205);
    public static readonly Color Cream     = Color.FromArgb(250, 246, 238);
    public static readonly Color Paper     = Color.FromArgb(253, 251, 247);
    public static readonly Color CardFill  = Color.FromArgb(255, 254, 252);
    public static readonly Color Border    = Color.FromArgb(232, 223, 207);
    public static readonly Color Grid      = Color.FromArgb(235, 229, 218);
    public static readonly Color TextDark  = Color.FromArgb(31, 42, 68);
    public static readonly Color TextMuted = Color.FromArgb(120, 124, 135);
    public static readonly Color Warning   = Color.FromArgb(184, 116, 26);
    public static readonly Color WarningBg = Color.FromArgb(246, 229, 197);

    // ── Font ─────────────────────────────────────────────
    public static readonly Font Body          = new("Segoe UI", 9f);
    public static readonly Font BodySmall     = new("Segoe UI", 8.5f);
    public static readonly Font BodySmallBold = new("Segoe UI", 8.5f, FontStyle.Bold);
    public static readonly Font Tiny          = new("Segoe UI", 7.5f);
    public static readonly Font Combo         = new("Segoe UI", 10.5f);

    public static readonly Font Title      = new("Georgia", 26f, FontStyle.Bold);
    public static readonly Font Subtitle   = new("Georgia", 13f, FontStyle.Italic);
    public static readonly Font SectionTab = new("Georgia", 13f, FontStyle.Bold);
    public static readonly Font Caption    = new("Georgia", 11.5f);
    public static readonly Font KpiValue   = new("Georgia", 19f, FontStyle.Bold);
    public static readonly Font NavItem    = new("Georgia", 12f);
    public static readonly Font Note       = new("Georgia", 10.5f);
    public static readonly Font Banner     = new("Georgia", 13f, FontStyle.Bold);
    public static readonly Font Logo       = new("Georgia", 26f);
    public static readonly Font LogoSub    = new("Segoe UI", 6.5f);
    public static readonly Font InfoGlyph  = new("Georgia", 8.5f, FontStyle.Bold);

    // Icon dạng ký tự (Segoe UI Symbol). Có thể thay bằng ảnh PNG/SVG trong Resources sau.
    public static readonly Font Icon      = new("Segoe UI Symbol", 15f);
    public static readonly Font IconLarge = new("Segoe UI Symbol", 20f);

    // ── Định dạng số kiểu Việt Nam: 18.720 / 65,2 ────────
    public static readonly CultureInfo Vi = CultureInfo.GetCultureInfo("vi-VN");
}

/// <summary>Hàm vẽ dùng chung (hình chữ nhật bo góc, chữ).</summary>
internal static class Gfx
{
    public const TextFormatFlags Center = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                                          | TextFormatFlags.NoPadding | TextFormatFlags.SingleLine;
    public const TextFormatFlags Left = TextFormatFlags.Left | TextFormatFlags.VerticalCenter
                                        | TextFormatFlags.NoPadding | TextFormatFlags.SingleLine
                                        | TextFormatFlags.EndEllipsis;
    public const TextFormatFlags Right = TextFormatFlags.Right | TextFormatFlags.VerticalCenter
                                         | TextFormatFlags.NoPadding | TextFormatFlags.SingleLine;

    public static void Text(Graphics g, string text, Font font, Color color, Rectangle bounds, TextFormatFlags flags)
        => TextRenderer.DrawText(g, text, font, bounds, color, flags);

    public static GraphicsPath RoundRect(Rectangle r, int radius) => RoundRect(r, radius, radius, radius, radius);

    /// <summary>Hình chữ nhật với bán kính bo riêng cho từng góc (0 = góc vuông).</summary>
    public static GraphicsPath RoundRect(Rectangle r, int tl, int tr, int br, int bl)
    {
        var p = new GraphicsPath();
        int x = r.X, y = r.Y, w = r.Width, h = r.Height;

        if (tl > 0) p.AddArc(x, y, tl * 2, tl * 2, 180, 90); else p.AddLine(x, y, x + 1, y);
        if (tr > 0) p.AddArc(x + w - tr * 2, y, tr * 2, tr * 2, 270, 90); else p.AddLine(x + w - 1, y, x + w, y);
        if (br > 0) p.AddArc(x + w - br * 2, y + h - br * 2, br * 2, br * 2, 0, 90); else p.AddLine(x + w, y + h - 1, x + w, y + h);
        if (bl > 0) p.AddArc(x, y + h - bl * 2, bl * 2, bl * 2, 90, 90); else p.AddLine(x + 1, y + h, x, y + h);

        p.CloseFigure();
        return p;
    }
}