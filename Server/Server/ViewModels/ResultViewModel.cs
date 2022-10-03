namespace Server.ViewModels;

public class ResultViewModel
{
    public string TestId { get; set; } = null!;
    public string IdempotencyKey { get; set; } = null!;
    public int QuestionCount { get; set; }
    public int CorrectAnswersCount { get; set; }
}