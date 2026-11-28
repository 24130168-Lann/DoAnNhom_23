using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

namespace khachSan.GiaoDien;
 
/// <summary>Control tự vẽ: bật double-buffer, vẽ lại khi đổi kích thước / đổi Text.</summary>
internal abstract class PaintControl : Control
{
    protected PaintControl()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint
                 | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        TabStop = false;
    }
 
    /// <summary>Màu nền phía sau control (để tô sạch 4 góc khi bo tròn).</summary>
    protected Color Backdrop => Parent?.BackColor ?? Theme.Paper;
 
    protected override void OnTextChanged(EventArgs e)
    {
        base.OnTextChanged(e);
        Invalidate();
    }
}
 
/// <summary>Panel nền bo góc + viền mảnh.</summary>
internal class RoundedPanel : Panel
{
    private Color _fill = Theme.Paper;
 
    public RoundedPanel()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint
                 | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        BackColor = _fill;
    }
 
    public int CornerRadius { get; set; } = 12;
    public Color BorderColor { get; set; } = Theme.Border;
 
    /// <summary>Màu nền thẻ. Đồng thời là BackColor để control con kế thừa đúng màu.</summary>
    public Color FillColor
    {
        get => _fill;
        set { _fill = value; BackColor = value; }
    }
 
    protected virtual GraphicsPath CreateCardPath()
        => Gfx.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), CornerRadius);
 
    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.Clear(Parent?.BackColor ?? Theme.Cream);
        if (Width < 8 || Height < 8) return;
 
        g.SmoothingMode = SmoothingMode.AntiAlias;
        using var path = CreateCardPath();
        using var fill = new SolidBrush(_fill);
        using var pen = new Pen(BorderColor);
        g.FillPath(fill, path);
        g.DrawPath(pen, path);
        base.OnPaint(e);
    }
}
 
/// <summary>Khối báo cáo (BC-01, BC-02): thẻ trắng bo góc với "tab" xanh navy ở góc trên trái.</summary>
internal sealed class SectionPanel : RoundedPanel
{
    public const int TabHeight = 34;
 
    public SectionPanel()
    {
        CornerRadius = 14;
        Padding = new Padding(18, TabHeight + 14, 18, 14);
    }
 
    public string TabText { get; set; } = string.Empty;
 
    protected override GraphicsPath CreateCardPath()
        => Gfx.RoundRect(new Rectangle(0, TabHeight, Width - 1, Height - TabHeight - 1),
                         0, CornerRadius, CornerRadius, CornerRadius);
 
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        if (Width < 8 || Height < 8) return;
 
        var g = e.Graphics;
        int textWidth = TextRenderer.MeasureText(g, TabText, Theme.SectionTab, Size.Empty, TextFormatFlags.NoPadding).Width;
        var tab = new Rectangle(0, 0, textWidth + 56, TabHeight + 1);
 
        using var path = Gfx.RoundRect(tab, 10, 22, 0, 0);
        using var navy = new SolidBrush(Theme.Navy);
        g.FillPath(navy, path);
        Gfx.Text(g, TabText, Theme.SectionTab, Color.White,
                 new Rectangle(tab.X + 20, 0, tab.Width - 20, TabHeight), Gfx.Left);
    }
}
 
/// <summary>Thẻ chỉ số: icon tròn màu vàng nhạt + tiêu đề + giá trị lớn.</summary>
internal sealed class KpiCard : PaintControl
{
    private string _icon = "", _caption = "", _value = "";
    private Color _iconColor = Theme.Navy;
 
    public string Icon    { get => _icon;    set { _icon = value;    Invalidate(); } }
    public string Caption { get => _caption; set { _caption = value; Invalidate(); } }
    public string Value   { get => _value;   set { _value = value;   Invalidate(); } }
    public Color IconColor { get => _iconColor; set { _iconColor = value; Invalidate(); } }
 
    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.Clear(Backdrop);
        if (Width < 40 || Height < 30) return;
        g.SmoothingMode = SmoothingMode.AntiAlias;
 
        using (var path = Gfx.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 10))
        using (var fill = new SolidBrush(Theme.CardFill))
        using (var pen = new Pen(Theme.Border))
        {
            g.FillPath(fill, path);
            g.DrawPath(pen, path);
        }
 
        int d = Math.Min(Height - 22, 48);
        var circle = new Rectangle(16, (Height - d) / 2, d, d);
        using (var soft = new SolidBrush(Theme.GoldSoft)) g.FillEllipse(soft, circle);
        Gfx.Text(g, _icon, Theme.Icon, _iconColor, circle, Gfx.Center);
 
        int x = circle.Right + 16;
        int w = Math.Max(10, Width - x - 8);
        int capH = TextRenderer.MeasureText(g, "Ag", Theme.Caption, Size.Empty, TextFormatFlags.NoPadding).Height;
        int valH = TextRenderer.MeasureText(g, "Ag", Theme.KpiValue, Size.Empty, TextFormatFlags.NoPadding).Height;
        int top = (Height - capH - valH) / 2;
 
        Gfx.Text(g, _caption, Theme.Caption, Theme.TextDark, new Rectangle(x, top, w, capH), Gfx.Left);
        Gfx.Text(g, _value, Theme.KpiValue, Theme.Navy, new Rectangle(x, top + capH, w, valH), Gfx.Left);
    }
}
 
/// <summary>Banner cảnh báo màu cam nhạt ("Có dữ liệu cần kiểm tra"). Nội dung lấy từ Text.</summary>
internal sealed class AlertBanner : PaintControl
{
    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.Clear(Backdrop);
        if (Width < 40 || Height < 30) return;
        g.SmoothingMode = SmoothingMode.AntiAlias;
 
        using (var path = Gfx.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 10))
        using (var fill = new SolidBrush(Theme.WarningBg))
        {
            g.FillPath(fill, path);
        }
 
        Gfx.Text(g, "\u26A0", Theme.IconLarge, Theme.Warning, new Rectangle(20, 0, 44, Height), Gfx.Center);
        Gfx.Text(g, Text, Theme.Banner, Theme.Warning, new Rectangle(76, 0, Math.Max(10, Width - 84), Height), Gfx.Left);
    }
}
 
/// <summary>Dòng ghi chú có biểu tượng (i) tròn xanh phía trước. Nội dung lấy từ Text.</summary>
internal sealed class InfoNote : PaintControl
{
    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.Clear(Backdrop);
        g.SmoothingMode = SmoothingMode.AntiAlias;
 
        const int d = 17;
        var circle = new Rectangle(2, (Height - d) / 2, d, d);
        using (var navy = new SolidBrush(Theme.Navy)) g.FillEllipse(navy, circle);
        Gfx.Text(g, "i", Theme.InfoGlyph, Color.White, circle, Gfx.Center);
        Gfx.Text(g, Text, Theme.Note, Theme.TextDark, new Rectangle(28, 0, Math.Max(10, Width - 28), Height), Gfx.Left);
    }
}
 
/// <summary>Mục menu bên trái. Selected = true thì nền vàng, chữ navy.</summary>
internal sealed class SidebarButton : PaintControl
{
    private string _icon = "";
    private bool _selected;
 
    public string Icon     { get => _icon;     set { _icon = value;     Invalidate(); } }
    public bool Selected { get => _selected; set { _selected = value; Invalidate(); } }
 
    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.Clear(Backdrop);
        g.SmoothingMode = SmoothingMode.AntiAlias;
 
        if (_selected)
        {
            using var path = Gfx.RoundRect(new Rectangle(0, 0, Width, Height), 0, 10, 10, 0);
            using var gold = new SolidBrush(Theme.Gold);
            g.FillPath(gold, path);
        }
 
        var fore = _selected ? Theme.Navy : Color.White;
        Gfx.Text(g, _icon, Theme.Icon, fore, new Rectangle(16, 0, 36, Height), Gfx.Center);
        Gfx.Text(g, Text, Theme.NavItem, fore, new Rectangle(62, 0, Math.Max(10, Width - 66), Height), Gfx.Left);
    }
}
 
/// <summary>Logo Canaria (hoa sen vẽ đơn giản + chữ) ở góc trên trái.</summary>
internal sealed class LogoPanel : PaintControl
{
    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.Clear(Theme.Cream);
        g.SmoothingMode = SmoothingMode.AntiAlias;
 
        float cx = Width / 2f, baseY = 38f;
        using (var pen = new Pen(Theme.Navy, 1.4f))
        {
            for (int angle = -60; angle <= 60; angle += 30)
            {
                var state = g.Save();
                g.TranslateTransform(cx, baseY);
                g.RotateTransform(angle);
                float len = 30f - Math.Abs(angle) / 5f;
                g.DrawEllipse(pen, -6f, -len, 12f, len);
                g.Restore(state);
            }
        }
 
        Gfx.Text(g, "Canaria", Theme.Logo, Theme.Navy, new Rectangle(0, 38, Width, 40), Gfx.Center);
        Gfx.Text(g, "B O U T I Q U E   H O T E L", Theme.LogoSub, Theme.Navy, new Rectangle(0, 76, Width, 16), Gfx.Center);
 
        using var divider = new Pen(Theme.Border);
        g.DrawLine(divider, Width - 1, 24, Width - 1, Height - 24);
    }
}
 
/// <summary>Tạo DataGridView theo phong cách báo cáo (header navy, dòng "Tổng năm" nền vàng nhạt).</summary>
internal static class ReportGrid
{
    public static DataGridView Create(string[] headers, float[] fillWeights, int headerHeight, int rowHeight)
    {
        var grid = new DataGridView
        {
            Dock = DockStyle.Top,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AllowUserToResizeRows = false,
            AllowUserToResizeColumns = false,
            AllowUserToOrderColumns = false,
            RowHeadersVisible = false,
            MultiSelect = false,
            TabStop = false,
            ScrollBars = ScrollBars.None,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
            ColumnHeadersHeight = headerHeight,
            EnableHeadersVisualStyles = false,
            ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single,
            CellBorderStyle = DataGridViewCellBorderStyle.Single,
            GridColor = Theme.Border,
            BackgroundColor = Theme.Paper,
            BorderStyle = BorderStyle.None,
            Font = Theme.BodySmall,
        };
        grid.RowTemplate.Height = rowHeight;
 
        var head = grid.ColumnHeadersDefaultCellStyle;
        head.BackColor = Theme.Navy;
        head.ForeColor = Color.White;
        head.SelectionBackColor = Theme.Navy;
        head.SelectionForeColor = Color.White;
        head.Font = Theme.BodySmallBold;
        head.Alignment = DataGridViewContentAlignment.MiddleCenter;
        head.WrapMode = DataGridViewTriState.True;
 
        var cell = grid.DefaultCellStyle;
        cell.BackColor = Theme.Paper;
        cell.ForeColor = Theme.TextDark;
        cell.SelectionBackColor = Theme.Paper;   // không tô chọn: đây là bảng chỉ để xem
        cell.SelectionForeColor = Theme.TextDark;
        cell.Alignment = DataGridViewContentAlignment.MiddleCenter;
 
        for (int i = 0; i < headers.Length; i++)
        {
            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = headers[i],
                FillWeight = fillWeights[i],
                MinimumWidth = 20,
                SortMode = DataGridViewColumnSortMode.NotSortable,
            });
        }
 
        // Cột "Tháng" căn trái
        grid.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        grid.Columns[0].DefaultCellStyle.Padding = new Padding(6, 0, 0, 0);
        return grid;
    }
 
    /// <summary>Thêm dòng tổng (đậm, nền vàng nhạt).</summary>
    public static void AddTotal(DataGridView grid, params object[] cells)
    {
        int index = grid.Rows.Add(cells);
        var style = grid.Rows[index].DefaultCellStyle;
        style.BackColor = Theme.GoldSoft;
        style.SelectionBackColor = Theme.GoldSoft;
        style.Font = Theme.BodySmallBold;
    }
 
    /// <summary>Chỉnh chiều cao bảng vừa khít nội dung (gọi sau khi đã thêm hết dòng).</summary>
    public static void FitHeight(DataGridView grid)
        => grid.Height = grid.ColumnHeadersHeight + grid.Rows.Count * grid.RowTemplate.Height + 2;
}
 

 
