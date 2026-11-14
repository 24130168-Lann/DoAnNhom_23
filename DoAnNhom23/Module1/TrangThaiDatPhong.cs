namespace DoAnNhom23;

public enum TrangThaiDatPhong
{
    CHO_COC, //CHO_COC: chờ thanh toán cọc, mặc định là 10 phút
    DA_DAT, //DA_DAT: đã cọc, chưa có phòng nào checkin
    DANG_O, //DANG_O: có ít nhất là một phòng đã checkin, chưa trả hết
            //ở đây có nghĩa là trong 1 (đơn) đặt phòng sẽ có nhiều phòng
            //nếu 1 trong các phòng trong đơn đã checkout rồi nhưng các phòng 
            //còn lại chưa checkout thì cái đặt phòng đó vẫn là DANG_O
            //tuy nhiên không phải là phòng đã checkout đó đang ở trạng thái DANG_O
    DA_TRA, //DA_TRA: mọi phòng đã check-out
    DA_HUY
}