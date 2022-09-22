﻿using System.Collections;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Server.Models;

public class AnswersCollection : IEnumerable<Answer>
{
    private readonly List<Answer> _answers = new();

    public IEnumerator<Answer> GetEnumerator() => _answers.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Add(Answer answer)
    {
        if (!CheckCorrectAnswersCount(answer))
            throw new ArgumentException("Can not add second correct answer");

        if (!CheckForUniqAnswerText(answer))
            throw new InvalidOperationException("Can not add two answers with the same text");
        
        _answers.Add(answer);
    }

    public void Add(params Answer[] answers)
    {
        foreach (var answer in answers)
        {
            Add(answer);
        }
    }

    private bool CheckCorrectAnswersCount(Answer answer)
    {
        var correctAnswersCount = _answers.Count(a => a.IsCorrect);
        correctAnswersCount += answer.IsCorrect ? 1 : 0;

        return correctAnswersCount <= 1;
    }

    private bool CheckForUniqAnswerText(Answer answer)
    {
        return !_answers.Select(a => a.Text).Contains(answer.Text);
    }

    public Answer this[int index] => _answers[index];
}