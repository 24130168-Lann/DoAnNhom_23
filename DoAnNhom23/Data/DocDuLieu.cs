using Microsoft.Data.Sqlite;

namespace DoAnNhom23.Data;

public static class DocDuLieu
{
    public static long Long(this SqliteDataReader r, string cot) => r.GetInt64(r.GetOrdinal(cot));
    public static int Int(this SqliteDataReader r, string cot) => r.GetInt32(r.GetOrdinal(cot));
    public static bool Bool (this SqliteDataReader r, string cot) => r.GetInt64(r.GetOrdinal(cot)) == 1;
    public static string Chuoi(this SqliteDataReader r, string cot) => r.GetString(r.GetOrdinal(cot));
    public static DateTime ThoiGian (this SqliteDataReader r, string cot) => Gio.TuChuoi(r.Chuoi(cot));

    public static T Enum<T>(this SqliteDataReader r, string cot) where T : struct, Enum =>
        System.Enum.Parse<T>(r.Chuoi(cot));
    
    // các hàm cho cột có thẻe Null
    public static long? LongNull(this SqliteDataReader r, string cot)
        => r.IsDBNull(r.GetOrdinal(cot)) ? null : r.Long(cot);

    public static string? ChuoiNull(this SqliteDataReader r, string cot)
        => r.IsDBNull(r.GetOrdinal(cot)) ? null : r.Chuoi(cot);
    public static DateTime? ThoiGianNull(this SqliteDataReader r, string cot)
    => r.IsDBNull(r.GetOrdinal(cot)) ? null : r.ThoiGian(cot);

    public static T? EnumNull<T>(this SqliteDataReader r, string cot) where T : struct, Enum
        => r.IsDBNull(r.GetOrdinal(cot)) ? null : r.Enum<T>(cot);

}