using System;
using System.Collections.Generic;

namespace _28_backend_netcore_06.Models.DBUser;

public partial class IpCount
{
    public int Id { get; set; }

    public string Ip { get; set; } = null!;

    public int Count { get; set; }

    public DateTime? DateRequest { get; set; }
}