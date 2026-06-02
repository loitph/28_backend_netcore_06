using System;
using System.Collections.Generic;

namespace _28_backend_netcore_06.Models.DBQuanLyNhanVien;

public partial class DiaDiemDuAn
{
    public int Id { get; set; }

    public int? MaDuAn { get; set; }

    public int? MaDiaDiem { get; set; }

    public virtual DiaDiem? MaDiaDiemNavigation { get; set; }

    public virtual DuAn? MaDuAnNavigation { get; set; }
}
