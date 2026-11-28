using khachSan.Data;

namespace khachSan.GiaoDien;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        Application.SetHighDpiMode(HighDpiMode.SystemAware);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        try
        {
            Db.KhoiTao();   // tạo file QuanLyKhachSan.db từ schema.sql + data.sql ở lần chạy đầu
        }
        catch (Exception ex)
        {
            MessageBox.Show("Không khởi tạo được cơ sở dữ liệu:\n\n" + ex.Message,
                "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        // Báo cáo chỉ dành cho Chủ khách sạn. Khi có màn hình đăng nhập, truyền vai trò thật vào đây.
        Application.Run(new BaoCaoKhachSanForm(new NguonDuLieuSqlite(), VaiTro.Chu));
    }
}