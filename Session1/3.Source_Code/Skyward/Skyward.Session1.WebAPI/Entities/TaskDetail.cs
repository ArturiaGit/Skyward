using System;
using System.Collections.Generic;

namespace Skyward.Session1.WebAPI.Entities;

public partial class TaskDetail
{
    public int Id { get; set; }

    public int TaskId { get; set; }

    public int PartId { get; set; }

    public int StockAmount { get; set; }

    public int? CheckAmount { get; set; }

    public int IsChecked { get; set; }

    public virtual Part Part { get; set; } = null!;

    public virtual InventoryCheckingTask Task { get; set; } = null!;
}
