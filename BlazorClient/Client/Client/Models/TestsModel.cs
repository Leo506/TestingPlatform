using System.Text.Json.Serialization;

namespace Client.Models;

public class TestsModel
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;
    
    [JsonPropertyName("questions")]
    public List<Question> Questions { get; set; } = new();
}