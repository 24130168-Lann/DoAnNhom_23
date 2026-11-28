using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

namespace khachSan.GiaoDien;
 
internal abstract class ChartControl : PaintControl
{
    protected ChartControl() => BackColor = Theme.Paper;

    /// <summary>Chú thích (ô màu + tên) căn giữa phía trên biểu đồ.</summary>
    protected void DrawLegend(Graphics g, IReadOnlyList<(string Text, Brush Fill)> items, int y)
    {
        const int box = 12, gap = 6, spacing = 24;
        var widths = items
            .Select(i => TextRenderer.MeasureText(g, i.Text, Theme.Body, Size.Empty, TextFormatFlags.NoPadding).Width)
            .ToArray();

        int total = widths.Sum() + items.Count * (box + gap) + (items.Count - 1) * spacing;
        int x = (Width - total) / 2;

        for (int i = 0; i < items.Count; i++)
        {
            g.FillRectangle(items[i].Fill, x, y + 3, box, box);
            Gfx.Text(g, items[i].Text, Theme.Body, Theme.TextDark,
                     new Rectangle(x + box + gap, y, widths[i] + 4, 18), Gfx.Left);
            x += box + gap + widths[i] + spacing;
        }
    }

    protected static double? At(double?[] data, int index) => index < data.Length ? data[index] : null;
}

/// <summary>BC-01: biểu đồ cột đôi (năm nay / kỳ trước), trục 0–100%.</summary>
internal sealed class OccupancyChart : ChartControl
{
    public string[] Categories { get; set; } = Array.Empty<string>();
    public double?[] Current  { get; set; } = Array.Empty<double?>();
    public double?[] Previous { get; set; } = Array.Empty<double?>();

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.Clear(BackColor);
        if (Width < 160 || Height < 120 || Categories.Length == 0) return;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        using var navy = new SolidBrush(Theme.Navy);
        using var gold = new SolidBrush(Theme.Gold);
        DrawLegend(g, new (string, Brush)[] { ("Năm nay", navy), ("Tháng trước", gold) }, 2);

        const int left = 48, right = 10, top = 34, bottom = 28;
        var plot = new Rectangle(left, top, Width - left - right, Height - top - bottom);

        using var axisPen = new Pen(Theme.TextMuted);
        using var gridPen = new Pen(Theme.Grid);
        for (int i = 0; i <= 4; i++)
        {
            int y = plot.Bottom - plot.Height * i / 4;
            g.DrawLine(i == 0 ? axisPen : gridPen, plot.Left, y, plot.Right, y);
            Gfx.Text(g, $"{i * 25}%", Theme.Body, Theme.TextDark, new Rectangle(0, y - 9, left - 8, 18), Gfx.Right);
        }

        float cw = plot.Width / (float)Categories.Length;
        int bw = Math.Max(6, (int)(cw * 0.30f));

        for (int i = 0; i < Categories.Length; i++)
        {
            float cx = plot.Left + cw * (i + 0.5f);
            DrawBar(g, navy, At(Current, i), (int)cx - bw - 1, bw, plot, -3);
            DrawBar(g, gold, At(Previous, i), (int)cx + 1, bw, plot, +3);
            Gfx.Text(g, Categories[i], Theme.Body, Theme.TextDark,
                     new Rectangle((int)(cx - cw / 2), plot.Bottom + 4, (int)cw, 20), Gfx.Center);
        }
    }

    private static void DrawBar(Graphics g, Brush brush, double? value, int x, int bw, Rectangle plot, int labelShift)
    {
        if (value is not double v) return;

        int h = (int)Math.Round(plot.Height * v / 100.0);
        var bar = new Rectangle(x, plot.Bottom - h, bw, h);
        g.FillRectangle(brush, bar);

        Gfx.Text(g, v.ToString("0.0", Theme.Vi) + "%", Theme.Tiny, Theme.TextDark,
                 new Rectangle(x + bw / 2 - 24 + labelShift, bar.Top - 15, 48, 14), Gfx.Center);
    }
}

/// <summary>BC-02: 3 cột mỗi tháng (doanh thu phòng, dịch vụ, cọc giữ lại – cột gạch chéo). Đơn vị: tỷ đồng.</summary>
internal sealed class RevenueChart : ChartControl
{
    public string[] Categories { get; set; } = Array.Empty<string>();
    public double?[] Room    { get; set; } = Array.Empty<double?>();
    public double?[] Service { get; set; } = Array.Empty<double?>();
    public double?[] Deposit { get; set; } = Array.Empty<double?>();
    public double MaxValue   { get; set; } = 2;
    /// <summary>Khoảng cách giữa hai vạch trên trục tung (cùng đơn vị với giá trị).</summary>
    public double Step       { get; set; } = 1;
    /// <summary>Tên đơn vị in ở vạch trên cùng, ví dụ "tỷ" hoặc "triệu".</summary>
    public string Unit       { get; set; } = "tỷ";

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.Clear(BackColor);
        if (Width < 160 || Height < 100 || Categories.Length == 0) return;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        using var navy = new SolidBrush(Theme.Navy);
        using var gold = new SolidBrush(Theme.Gold);
        using var hatch = new HatchBrush(HatchStyle.WideUpwardDiagonal, Theme.Navy, Color.White);
        DrawLegend(g, new (string, Brush)[]
        {
            ("Doanh thu phòng", navy), ("Doanh thu dịch vụ", gold), ("Cọc giữ lại", hatch),
        }, 0);

        const int left = 78, right = 10, top = 30, bottom = 22;
        var plot = new Rectangle(left, top, Width - left - right, Height - top - bottom);

        using var axisPen = new Pen(Theme.TextMuted);
        using var gridPen = new Pen(Theme.Grid);
        double step = Step > 0 ? Step : 1;
        int steps = (int)Math.Round(MaxValue / step);
        for (int i = 0; i <= steps; i++)
        {
            int y = plot.Bottom - (int)(plot.Height * (i * step) / MaxValue);
            g.DrawLine(i == 0 ? axisPen : gridPen, plot.Left, y, plot.Right, y);
            string label = (i * step).ToString("0.##", Theme.Vi);
            if (i == steps) label += " " + Unit;
            Gfx.Text(g, label, Theme.Body, Theme.TextDark, new Rectangle(0, y - 9, left - 8, 18), Gfx.Right);
        }

        float cw = plot.Width / (float)Categories.Length;
        int bw = Math.Max(6, (int)(cw * 0.15f));
        const int gap = 2;
        int groupWidth = bw * 3 + gap * 2;
        using var outline = new Pen(Theme.Navy);

        for (int i = 0; i < Categories.Length; i++)
        {
            float cx = plot.Left + cw * (i + 0.5f);
            int x0 = (int)cx - groupWidth / 2;

            double? room = At(Room, i), service = At(Service, i), deposit = At(Deposit, i);
            if (room is null && service is null && deposit is null)
            {
                Gfx.Text(g, "–   –   –", Theme.Tiny, Theme.TextMuted,
                         new Rectangle((int)cx - 30, plot.Bottom - 14, 60, 12), Gfx.Center);
            }
            else
            {
                DrawBar(g, navy, null, room, x0, bw, plot);
                DrawBar(g, gold, null, service, x0 + bw + gap, bw, plot);
                DrawBar(g, hatch, outline, deposit, x0 + (bw + gap) * 2, bw, plot);
            }

            Gfx.Text(g, Categories[i], Theme.Body, Theme.TextDark,
                     new Rectangle((int)(cx - cw / 2), plot.Bottom + 2, (int)cw, 18), Gfx.Center);
        }
    }

    private void DrawBar(Graphics g, Brush brush, Pen? outline, double? value, int x, int bw, Rectangle plot)
    {
        if (value is not double v) return;

        int h = (int)Math.Round(plot.Height * v / MaxValue);
        var bar = new Rectangle(x, plot.Bottom - h, bw, h);
        g.FillRectangle(brush, bar);
        if (outline is not null) g.DrawRectangle(outline, bar);

        Gfx.Text(g, v.ToString(v >= 100 ? "0" : "0.0", Theme.Vi), Theme.Tiny, Theme.TextDark,
                 new Rectangle(x + bw / 2 - 14, bar.Top - 14, 28, 13), Gfx.Center);
    }
}
