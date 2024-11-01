using System;
using System.Collections.Generic;

namespace Skyward.Session1.WebAPI.Entities;

public partial class PartStorage
{
    public int Id { get; set; }

    public int PartId { get; set; }

    public int ZoneId { get; set; }

    public int Amount { get; set; }

    public virtual Part Part { get; set; } = null!;

    public virtual Zone Zone { get; set; } = null!;
}
