namespace Client.Models;

public class ResultModel
{
    public string TestId { get; set; } = null!;
    public int QuestionCount { get; set; }
    public int CorrectAnswersCount { get; set; }

    public string ReturnUrl { get; set; } = null!;
}