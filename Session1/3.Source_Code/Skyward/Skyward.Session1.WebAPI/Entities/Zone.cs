using System;
using System.Collections.Generic;

namespace Skyward.Session1.WebAPI.Entities;

public partial class Zone
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int WarehouseId { get; set; }

    public int UpperLeftX { get; set; }

    public int UpperLeftY { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }

    public virtual ICollection<PartStorage> PartStorages { get; set; } = new List<PartStorage>();

    public virtual Warehouse Warehouse { get; set; } = null!;
}