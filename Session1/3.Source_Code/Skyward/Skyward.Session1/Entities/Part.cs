using System.Text.Json.Serialization;

namespace Skyward.Session1.Entities;

public record Part
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    
    [JsonPropertyName("category")]
    public string? Category { get; set; }
    
    [JsonPropertyName("specification")]
    public string? Specification { get; set; }
    
    [JsonPropertyName("amount")]
    public int Amount { get; set; }
    
    [JsonPropertyName("minInventory")]
    public int MinInventory { get; set; }
}