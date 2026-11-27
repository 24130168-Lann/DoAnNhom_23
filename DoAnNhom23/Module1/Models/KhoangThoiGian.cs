namespace DoAnNhom23.Module1.Models;

public class KhoangThoiGian
{
    public DateTime Tu { get; }
    public DateTime Den { get; }

    public KhoangThoiGian(DateTime tu, DateTime den)
    {
        if (den <= tu) throw new ArgumentException("Thời điểm kết thúc phải lớn hơn thời điểm bắt đầu");
        Tu = tu;
        Den = den;
    }

    public bool Giao (KhoangThoiGian k) => Tu < k.Den && k.Tu < Den;
}