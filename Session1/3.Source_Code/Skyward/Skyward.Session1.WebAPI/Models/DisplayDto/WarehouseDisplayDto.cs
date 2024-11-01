namespace Skyward.Session1.WebAPI.Models.DisplayDto;

public record WarehouseDisplayDto
{
    public string? Name { get; set; }
    public int Area { get; set; }
    public string? WarehouseKeeper { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public DateOnly? LastCheckingDate { get; set; }  
}