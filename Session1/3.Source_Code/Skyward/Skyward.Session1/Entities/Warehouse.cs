using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Skyward.Session1.Entities;

public record Warehouse
{
    public string? Name { get; set; }
    
    public int Area { get; set; }
    
    public string? WarehouseKeeper { get; set; }
    
    public string? Phone { get; set; }
    
    public string? Address { get; set; }
    
    public DateOnly? LastCheckingDate { get; set; }
    
    public ICollection<Zone>? Zones { get; set; }
}