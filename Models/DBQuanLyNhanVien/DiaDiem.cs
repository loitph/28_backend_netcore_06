using System;
using System.Collections.Generic;

namespace _28_backend_netcore_06.Models.DBQuanLyNhanVien;

public partial class DiaDiem
{
    public int Id { get; set; }

    public string TenDiaDiem { get; set; } = null!;

    public string? DiaChi { get; set; }

    public virtual ICollection<DiaDiemDuAn> DiaDiemDuAns { get; set; } = new List<DiaDiemDuAn>();

    public virtual ICollection<DuAn> DuAns { get; set; } = new List<DuAn>();
}
