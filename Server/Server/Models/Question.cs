namespace Server.Models;

public enum QuestionStatuses
{
    None,
    Failed,
    Correct
}

public class Question
{
    public int AnswersCount => _answers?.Count ?? 0;
    public string QuestionText { get; set; } = null!;
    
    public QuestionStatuses Status { get; private set; }
    
    private List<Answer> _answers;

    public Question(string questionText, params Answer[] answers)
    {
        if (!CheckCorrectAnswersCount(1, in answers))
            throw new ArgumentException("Can not be more than one correct answers");

        if (!CheckForSameAnswersText(in answers))
            throw new ArgumentException("Can not be more than one same answer");
        
        _answers = new List<Answer>();
        foreach (var answer in answers)
        {
            _answers.Add(answer);
        }

        QuestionText = questionText;

        Status = QuestionStatuses.None;
    }

    private bool CheckCorrectAnswersCount(int mustBe, in Answer[] answers)
    {
        var correctAnswersCount = answers.Count(a => a.IsCorrect);

        return correctAnswersCount == mustBe;
    }

    private bool CheckForSameAnswersText(in Answer[] answers)
    {
        var uniqAnswerText = new HashSet<string>(answers.Select(a => a.Text));

        return uniqAnswerText.Count == answers.Length;
    }

    public void SelectAnswer(string answerText)
    {
        var answer = _answers.FirstOrDefault(a => a.Text == answerText);

        if (answer is null)
            throw new InvalidOperationException($"There is no answer with this text: {answerText}");

        Status = answer.IsCorrect ? QuestionStatuses.Correct : QuestionStatuses.Failed;
    }
}