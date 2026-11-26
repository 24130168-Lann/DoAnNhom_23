using DoAnNhom23.Module2.Enums;

namespace DoAnNhom23.Module2.Results;

// Kết quả tính tiền phòng (TP-01 đến TP-07)
public record KetQuaTienPhong(
    long TienPhong,
    HinhThucThue Hinhthuc,
    int SoDem,      // số đêm tính tiền (0 nếu cùng ngày)
    int SoGio,      // số giờ tính tiền (chỉ dùng cho nhánh cùng ngày)
    int SoGioTre,   // số giờ trễ sau giờ trả chuẩn (chỉ dùng cho nhánh qua đêm)
    long TienDem,   // tiền đêm (nhánh qua đêm)
    long TienGio,   // tiền theo giờ (nhánh cùng ngày) hoặc phụ thu trễ (nhánh qua đêm)
    string CachTinh);