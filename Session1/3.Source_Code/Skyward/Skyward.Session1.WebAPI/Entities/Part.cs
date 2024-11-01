using System;
using System.Collections.Generic;

namespace Skyward.Session1.WebAPI.Entities;

public partial class Part
{
    public int Id { get; set; }

    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public string Unit { get; set; } = null!;

    public string? Specification { get; set; }

    public int MinInventory { get; set; }

    public int MaxInventory { get; set; }

    public virtual PartCategory Category { get; set; } = null!;

    public virtual ICollection<PartStorage> PartStorages { get; set; } = new List<PartStorage>();

    public virtual ICollection<TaskDetail> TaskDetails { get; set; } = new List<TaskDetail>();
}
