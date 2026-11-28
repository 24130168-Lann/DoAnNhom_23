namespace CanariaHotel;

static class Program
{
   
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
   
    static void Main()
    {
        DoAnNhom23.Data.Db.KhoiTao();
        Application.Run(new FormManHinh00());
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.Run(new FormManHinh00());
    }
}