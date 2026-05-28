using System;
using System.Collections.Generic;

namespace _28_backend_netcore_06.Models.DBQuanLyNhanVien;

public partial class NhamChuc
{
    public int Id { get; set; }

    public string? TenChucVu { get; set; }

    public int? MaNv { get; set; }

    public virtual NhanVien? MaNvNavigation { get; set; }
}
