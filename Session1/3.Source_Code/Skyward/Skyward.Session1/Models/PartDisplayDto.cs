namespace Skyward.Session1.Models;

public class PartDisplayDto
{
    public string? PartName { get; set; }
    public string? ZoneName { get; set; }
    
    public string? Category { get; set; }
    
    public string? Specification { get; set; }
    
    public int Amount { get; set; }
    
    public int MinInventory { get; set; }
}