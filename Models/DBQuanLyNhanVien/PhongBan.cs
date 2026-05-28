using System;
using System.Collections.Generic;

namespace _28_backend_netcore_06.Models.DBQuanLyNhanVien;

public partial class PhongBan
{
    public int Id { get; set; }

    public string? TenPb { get; set; }

    public virtual ICollection<NhanVien> NhanViens { get; set; } = new List<NhanVien>();
}
