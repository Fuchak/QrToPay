using System.Text.Json.Serialization;

namespace QrToPay.Models;
public class Attraction
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("attractionname")]
    public string? AttractionName { get; set; }

    [JsonPropertyName("price")]
    public double? Price { get; set; }
}