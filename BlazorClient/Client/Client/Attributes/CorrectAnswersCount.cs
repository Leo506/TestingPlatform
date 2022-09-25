using System.ComponentModel.DataAnnotations;
using Client.Models;

namespace Client.Attributes;

public class CorrectAnswersCount : ValidationAttribute
{
    private readonly int _allowedCorrectCount;

    public CorrectAnswersCount(int allowedCorrectCount)
    {
        _allowedCorrectCount = allowedCorrectCount;
    }

    public override bool IsValid(object? value)
    {
        if (value is null) return true;
        if (value is not AnswersCollection collection) return false;

        var correctCount = collection.Count(a => a.IsCorrect);

        if (correctCount > _allowedCorrectCount)
        {
            ErrorMessage = $"Must be not bigger than {_allowedCorrectCount} correct answers";
            return false;
        }

        if (correctCount == 0)
        {
            ErrorMessage = "Must be at least 1 correct answers";
            return false;
        }

        return true;
    }
}