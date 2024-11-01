using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Skyward.Session1.Entities;

public record Zone
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    
    [JsonPropertyName("partDisplayDtos")]
    public ICollection<Part>?  PartDisplayDtos { get; set; }
}