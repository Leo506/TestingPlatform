using System.Collections;

namespace Client.Models;

public class AnswersCollection : IEnumerable<Answer>
{
    private readonly List<Answer> _answers = new();

    public IEnumerator<Answer> GetEnumerator() => _answers.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private void Add(Answer answer)
    {
        _answers.Add(answer);
    }

    public void Add(params Answer[] answers)
    {
        foreach (var answer in answers)
        {
            Add(answer);
        }
    }

    public Answer this[int index] => _answers[index];
}