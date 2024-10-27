using System;
using System.Collections.Generic;

namespace Skyward.Session1.WebAPI.Entities;

public partial class TaskType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<InventoryCheckingTask> InventoryCheckingTasks { get; set; } = new List<InventoryCheckingTask>();
}
