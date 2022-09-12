namespace Server.Models;

public enum QuestionStatuses
{
    None,
    Failed,
    Correct
}

public class Question
{
    public int AnswersCount => _answersCollection?.Count() ?? 0;
    public string QuestionText { get; set; } = null!;
    
    public QuestionStatuses Status { get; private set; }

    private readonly AnswersCollection _answersCollection = new();

    public Question(string questionText)
    {

        QuestionText = questionText;

        Status = QuestionStatuses.None;
    }

    public void SelectAnswer(string answerText)
    {
        var answer = _answersCollection.FirstOrDefault(a => a.Text == answerText);

        if (answer is null)
            throw new InvalidOperationException($"There is no answer with this text: {answerText}");

        Status = answer.IsCorrect ? QuestionStatuses.Correct : QuestionStatuses.Failed;
    }

    public void AddAnswers(params Answer[] answers) => _answersCollection.Add(answers);
}