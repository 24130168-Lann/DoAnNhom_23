using System.ComponentModel.Design.Serialization;
using System.Reflection.Metadata;
using Microsoft.Win32.SafeHandles;

namespace DoAnNhom23;
//bảng PHONG
public class Phong
{
  public long Id  { get; set; }
  public long LoaiPhongId { get; set; }
  public string SoPhong  { get; set; } = "";
  public int Tang { get; set; }
  public CheDoPhong CheDo { get; set; }
  public bool CanDonDep { get; set; }
  public DateTime? DonXongLuc { get; set; }
}





