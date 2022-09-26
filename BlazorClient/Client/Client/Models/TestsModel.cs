using System.Text.Json.Serialization;

namespace Client.Models;

public class TestsModel
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;
    
    [JsonPropertyName("questions")]
    public List<Question> Questions { get; set; } = new();

    public bool IsValid()
    {
        foreach (var question in Questions)
        {
            var hash = new HashSet<string>(question.AnswersCollection.Select(a => a.Text));
            if (hash.Count != question.AnswersCollection.Count())
                return false;

            var correctCount = question.AnswersCollection.Count(a => a.IsCorrect);
            if (correctCount != 1)
                return false;
        }

        return true;
    }
}