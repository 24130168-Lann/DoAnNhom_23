using System.ComponentModel.Design.Serialization;
using System.Reflection.Metadata;
using Microsoft.Win32.SafeHandles;

namespace DoAnNhom23;
//bảng PHONG
public class Phong
{
  public int Id  { get; set; }
  public string SoPhong { get; set; } = string.Empty;
  public int Tang { get; set; }
  public int LoaiPhongId  { get; set; }
  //true khi phòng vừa check-out và đang dọn
  public bool CanDonDep { get; set; }
  //thời điểm dọn xong dự kiến (ở module 2), trống khi không cần dọn
  public DateTime? DonXongLuc  { get; set; } 
}





