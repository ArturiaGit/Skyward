using System;
using System.Collections.Generic;

namespace Skyward.Session1.WebAPI.Entities;

public partial class Role
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}
