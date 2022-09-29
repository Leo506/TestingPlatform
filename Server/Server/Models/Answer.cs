namespace Server.Models;

public class Answer
{
    public string Text { get; set; } = null;

    public bool IsCorrect { get; set; } = false;

    public override bool Equals(object? obj)
    {
        if (obj is not Answer answer)
            return false;

        return answer.Text == Text;
    }
}