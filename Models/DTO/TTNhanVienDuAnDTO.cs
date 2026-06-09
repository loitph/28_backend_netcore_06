using System.Text.Json;

namespace backend_netcore_06.Models.DTO
{
  public class ChiTietDuAnDTO
  {
      public int MaDuAn { get; set; }
      public string TenDuAn { get; set; }
      public int SoNhanVien { get; set; }
      public string DanhSachNhanVien { get; set; }
      public IEnumerable<TTNhanVienDuAnDTO> DanhSachNhanVienChiTiet { get; set; } = new List<TTNhanVienDuAnDTO>();

      public void ConvertJsonNhanVienDuAn()
      {
          if (!string.IsNullOrEmpty(DanhSachNhanVien))
          {
              DanhSachNhanVienChiTiet = JsonSerializer.Deserialize<IEnumerable<TTNhanVienDuAnDTO>>(DanhSachNhanVien);
          }
      }
  }

  public class TTNhanVienDuAnDTO
  {
      public int MaNhanVien { get; set; }
      public string TenNV { get; set; }
      public string NgaySinh { get; set; }
      public string SoDienThoai { get; set; }
  }
}