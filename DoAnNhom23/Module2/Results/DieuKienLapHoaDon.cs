using DoAnNhom23.Module2.Enums;

namespace DoAnNhom23.Module2.Results;

public record DieuKienLapHoaDon(
    bool DuDieuKien,
    LyDoChuaLapHoaDon LyDo,
    IReadOnlyList<int> DongDangO,
    IReadOnlyList<int> DongChuaNhan);