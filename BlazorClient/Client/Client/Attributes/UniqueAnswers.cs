using System.ComponentModel.DataAnnotations;
using Client.Models;

namespace Client.Attributes;

public class UniqueAnswers : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is null) return true;
        if (value is not AnswersCollection collection) return false;

        var answers = collection.Select(a => a.Text).ToList();
        var hash = new HashSet<string>(answers);

        if (hash.Count == answers.Count)
            return true;

        ErrorMessage = "Only unique answers allowed";
        return false;
    }
}