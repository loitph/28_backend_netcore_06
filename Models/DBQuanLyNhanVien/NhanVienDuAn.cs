using System;
using System.Collections.Generic;

namespace _28_backend_netcore_06.Models.DBQuanLyNhanVien;

public partial class NhanVienDuAn
{
    public int Id { get; set; }

    public int? MaNv { get; set; }

    public int? MaDuAn { get; set; }

    public virtual DuAn? MaDuAnNavigation { get; set; }

    public virtual NhanVien? MaNvNavigation { get; set; }
}
