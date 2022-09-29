using Client.Attributes;

namespace Client.Models;

public class Question
{
    public string QuestionText { get; set; } = null!;

    [UniqueAnswers]
    [CorrectAnswersCount(1)]
    public AnswersCollection AnswersCollection { get; set; } = new();
    
    public bool IsChooseCorrectAnswer { get; set; }

    public Question(string questionText)
    {
        QuestionText = questionText;
    }
}