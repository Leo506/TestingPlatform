using MongoDB.Bson.Serialization.Attributes;

namespace Server.Models;

public enum QuestionStatuses
{
    None,
    Failed,
    Correct
}

public class Question
{
    [BsonIgnore]
    public int AnswersCount => AnswersCollection?.Count() ?? 0;
    public string QuestionText { get; set; } = null!;
    
    public QuestionStatuses Status { get; private set; }
    
    public AnswersCollection AnswersCollection { get; set; } = new();

    public Question(string questionText)
    {

        QuestionText = questionText;

        Status = QuestionStatuses.None;
    }

    public void SelectAnswer(string answerText)
    {
        var answer = AnswersCollection.FirstOrDefault(a => a.Text == answerText);

        if (answer is null)
            throw new InvalidOperationException($"There is no answer with this text: {answerText}");

        Status = answer.IsCorrect ? QuestionStatuses.Correct : QuestionStatuses.Failed;
    }

    public void AddAnswers(params Answer[] answers) => AnswersCollection.Add(answers);
}