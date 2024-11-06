namespace Skyward.Session1.Models;

public class PartToBeCheckedDisplayDto
{
    public string? ZoneName { get; set; }
    public int PartId { get; set; }
    public string? PartName { get; set; }
    public string? Unit { get; set; }
    public string? Specification { get; set; }
    public int StockAmount { get; set; }
}