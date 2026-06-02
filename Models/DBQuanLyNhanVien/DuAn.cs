using System;
using System.Collections.Generic;

namespace _28_backend_netcore_06.Models.DBQuanLyNhanVien;

public partial class DuAn
{
    public int Id { get; set; }

    public string TenDuAn { get; set; } = null!;

    public string? MoTa { get; set; }

    public DateOnly? NgayBatDau { get; set; }

    public DateOnly? NgayKetThuc { get; set; }

    public int? MaDiaDiem { get; set; }

    public virtual ICollection<DiaDiemDuAn> DiaDiemDuAns { get; set; } = new List<DiaDiemDuAn>();

    public virtual DiaDiem? MaDiaDiemNavigation { get; set; }

    public virtual ICollection<NhanVienDuAn> NhanVienDuAns { get; set; } = new List<NhanVienDuAn>();
}
