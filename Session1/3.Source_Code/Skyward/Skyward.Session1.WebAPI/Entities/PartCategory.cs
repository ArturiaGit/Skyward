using System;
using System.Collections.Generic;

namespace Skyward.Session1.WebAPI.Entities;

public partial class PartCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Part> Parts { get; set; } = new List<Part>();
}
