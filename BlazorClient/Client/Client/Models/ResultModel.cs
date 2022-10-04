using System.Text.Json.Serialization;

namespace Client.Models;

public class ResultModel
{
    public string TestId { get; set; } = null!;

    public string IdempotencyKey { get; set; } = null!;
    public int QuestionCount { get; set; }
    public int CorrectAnswersCount { get; set; }

    [JsonIgnore]
    public string ReturnUrl { get; set; } = null!;
}