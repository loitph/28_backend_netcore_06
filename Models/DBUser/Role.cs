using System;
using System.Collections.Generic;

namespace _28_backend_netcore_06.Models.DBUser;

public partial class Role
{
    public int Id { get; set; }

    public string Rolename { get; set; } = null!;

    public bool Deleted { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
