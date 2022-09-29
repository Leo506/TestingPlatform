using MongoDB.Bson.Serialization.Attributes;

namespace Server.Models;

public class Question
{
    public string QuestionText { get; set; } = null!;

    public AnswersCollection AnswersCollection { get; set; } = new();

    public Question(string questionText)
    {
        QuestionText = questionText;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Question question)
            return false;

        if (question.QuestionText != QuestionText)
            return false;

        if (question.AnswersCollection.Count() != AnswersCollection.Count())
            return false;

        for (int i = 0; i < AnswersCollection.Count(); i++)
        {
            if (!AnswersCollection[i].Equals(question.AnswersCollection[i]))
                return false;
        }

        return true;
    }
}