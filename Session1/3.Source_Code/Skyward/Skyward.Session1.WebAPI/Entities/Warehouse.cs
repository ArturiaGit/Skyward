using System;
using System.Collections.Generic;

namespace Skyward.Session1.WebAPI.Entities;

public partial class Warehouse
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public int Manager { get; set; }

    public int Area { get; set; }

    public virtual Staff ManagerNavigation { get; set; } = null!;

    public virtual ICollection<Zone> Zones { get; set; } = new List<Zone>();
}
