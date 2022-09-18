using Server.ViewModels;

namespace Server.UnitTests;

[TestFixture]
public class TestViewModelExtensionTests
{
    [Test]
    public void ToTestModel_CorrectData_Success()
    {
        var testViewModel = new TestViewModel()
        {
            QuestionTexts = new[]
            {
                "Question1",
                "Question2"
            },
            Answers = new[]
            {
                new[]
                {
                    "Answer1",
                    "Answer2",
                    "Answer3"
                },
                new[]
                {
                    "Answer4",
                    "Answer5"
                }
            },
            CorrectAnswersIndex = new[]
            {
                0,
                1
            }
        };

        var testModel = testViewModel.ToTestModel();
        
        Assert.That(testModel.Questions.Count, Is.EqualTo(2));
        Assert.That(testModel.Questions[0].QuestionText, Is.EqualTo("Question1"));
        Assert.That(testModel.Questions[1].QuestionText, Is.EqualTo("Question2"));
        Assert.That(testModel.Questions[0].AnswersCount, Is.EqualTo(3));
        Assert.That(testModel.Questions[1].AnswersCount, Is.EqualTo(2));
    }
}