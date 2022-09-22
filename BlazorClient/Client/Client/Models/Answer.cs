using System.Text.Json.Serialization;

namespace Client.Models;

public class Answer
{
    [JsonPropertyName("text")]
    public string Text { get; set; } = null;

    [JsonPropertyName("isCorrect")]
    public bool IsCorrect { get; set; } = false;
}