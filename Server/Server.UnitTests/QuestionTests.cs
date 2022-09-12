using Server.Models;

namespace Server.UnitTests;

public class QuestionTests
{
    [Test]
    public void CreateQuestion_CorrectData_Success()
    {
        var questionText = "Question text";
        var question = new Question(questionText, new[]
        {
            new Answer()
            {
                Text = "Answer 1",
                IsCorrect = true
            },

            new Answer()
            {
                Text = "Answer 2"
            }
        });
        
        Assert.That(question.AnswersCount, Is.EqualTo(2));
        Assert.That(question.QuestionText, Is.EqualTo(questionText));
    }

    [Test]
    public void CreateQuestion_TwoCorrectAnswers_Throws()
    {
        var ex = Assert.Catch(() =>
        {
            var question = new Question("Text", new[]
            {
                new Answer()
                {
                    Text = "Answer 1",
                    IsCorrect = true
                },
                new Answer()
                {
                    Text = "Answer 2",
                    IsCorrect = true
                }
            });
        });
        
        StringAssert.Contains("Can not be more than one correct answers", ex?.Message);
    }

    [Test]
    public void CreateQuestion_TwoSameAnswers_Throws()
    {
        var ex = Assert.Catch(() =>
        {
            var question = new Question("Text", new[]
            {
                new Answer()
                {
                    Text = "Answer 1",
                    IsCorrect = true
                },
                new Answer()
                {
                    Text = "Answer 1"
                }
            });
        });
        
        StringAssert.Contains("Can not be more than one same answer", ex?.Message);
    }

    [Test]
    public void SelectAnswer_CorrectAnswer_QuestionStatusChanges()
    {
        var question = MakeQuestion();

        question.SelectAnswer("Answer 1");
        
        Assert.That(question.Status, Is.EqualTo(QuestionStatuses.Correct));
    }

    [Test]
    public void SelectAnswer_IncorrectAnswer_QuestionStatusChanges()
    {
        var question = MakeQuestion();
        
        question.SelectAnswer("Answer 2");
        
        Assert.That(question.Status, Is.EqualTo(QuestionStatuses.Failed));
    }

    [Test]
    public void SelectAnswer_AnswerNotExists_Throws()
    {
        var question = MakeQuestion();

        var ex = Assert.Catch(() => question.SelectAnswer("Answer 3"));
        
        StringAssert.Contains("There is no answer with this text: ", ex?.Message);
    }

    private Question MakeQuestion()
    {
        return new Question("Text", new[]
        {
            new Answer()
            {
                Text = "Answer 1",
                IsCorrect = true
            },
            new Answer()
            {
                Text = "Answer 2"
            }
        });
    }
}