
using System.ComponentModel.DataAnnotations.Schema;

[Table("NhanVien")]
public class ThemNhanVienNhanhDTO
{
  public string TenNV { get; set; }
  public string SoDienThoai { get; set; }
  public string MaPB { get; set; }
}