namespace Server.ViewModels;

public class TestViewModel
{
    public string Id { get; set; } = null!;
    public string[] QuestionTexts { get; init; } = null!;

    public string[][] Answers { get; init; } = null!;

    public int[] CorrectAnswersIndex { get; init; } = null!;
}