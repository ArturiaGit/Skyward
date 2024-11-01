using System;
using System.Collections.Generic;

namespace Skyward.Session1.WebAPI.Entities;

public partial class Staff
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public string Telephone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Password { get; set; }

    public int RoleId { get; set; }

    public string? Photo { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<Warehouse> Warehouses { get; set; } = new List<Warehouse>();
}
