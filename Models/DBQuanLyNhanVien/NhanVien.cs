using System;
using System.Collections.Generic;

namespace _28_backend_netcore_06.Models.DBQuanLyNhanVien;

public partial class NhanVien
{
    public int Id { get; set; }

    public string? Ten { get; set; }

    public DateOnly? NgaySinh { get; set; }

    public string? DiaChi { get; set; }

    public string? SoDienThoai { get; set; }

    public int? MaPb { get; set; }

    public virtual PhongBan? MaPbNavigation { get; set; }

    public virtual ICollection<NhamChuc> NhamChucs { get; set; } = new List<NhamChuc>();
}
