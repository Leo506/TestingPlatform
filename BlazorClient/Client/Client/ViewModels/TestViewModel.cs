namespace Client.ViewModels;

public class TestViewModel
{
    public string[] QuestionTexts { get; init; } = null!;

    public string[][] Answers { get; init; } = null!;

    public int[] CorrectAnswersIndex { get; init; } = null!;
}