using System.Globalization;

namespace DoAnNhom23.Data;

public static class Gio  {
    //lưu chuỗi định dạng chuẩn gần giống với định dạng ISO/SQL - DATETIME
    //HH: giờ theo hệ 24 giờ, nếu mà dùng hh thì là hệ 12 giờ
    private const string Dang = "yyyy-MM-dd HH:mm:ss";
    //cần InvariantCulture vì máy dùng ngôn ngữ khác có thể in dấu : hoặc số khác
    //chuyển DateTime thành string
    //ví dụ 25/11/2026 19:29:05 -> "2026-11-25 19:29:05"
    public static string ThanhChuoi(DateTime t) => t.ToString(Dang, CultureInfo.InvariantCulture);
    //chuyển string thành DateTime
    //ví dụ 25/11/2026 19:29:05 <- "2026-11-25 19:29:05"
    //nếu mà chuỗi s sai đinhj dạng thì DaeTime.ParseExact sẽ ném ngoại lệ ForrmatException hoặc là ArgumentNullException
    public static DateTime TuChuoi(string s) => DateTime.ParseExact(s, Dang, CultureInfo.InvariantCulture);
    //lấy thời gian hiện tại dưới dạng chuỗi chuẩn
    //lấy thời điểm hiện tại trên máy rồi gọi ThanhChuoi để trả về chuỗi theo định dạng "yyy-MM-dd HH:mm:ss" 
    public static string Bay() => ThanhChuoi(DateTime.Now);
}