using Server.Models;
using Server.ViewModels;

namespace Server.UnitTests;

public class TestViewModelExtensionTests
{
    [Fact]
    public void ToTestModel_From_TestViewModel_Success()
    {
        // arrange
        var expected = MakeTestsModel();
        var viewModel = new TestViewModel()
        {
            Id = "1111",
            QuestionTexts = new[] { "Question 1" },
            Answers = new[] { new[] { "Answer" } },
            CorrectAnswersIndex = new[] { 0 }
        };

        // act
        var actual = viewModel.ToTestModel();

        // assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTestViewModel_From_TestsModel_Success()
    {
        // arrange
        var expected = new TestViewModel()
        {
            Id = "1111",
            QuestionTexts = new[] { "Question 1" },
            Answers = new[] { new[] { "Answer" } },
            CorrectAnswersIndex = new[] { 0 }
        };

        var testModel = MakeTestsModel();

        // act
        var actual = testModel.ToTestViewModel();

        // assert
        Assert.Equal(expected, actual);
    }

    private TestsModel MakeTestsModel()
    {
        var model = new TestsModel();
        model.Id = "1111";
        var question = new Question("Question 1");
        question.AnswersCollection.Add(new Answer()
        {
            Text = "Answer",
            IsCorrect = true
        });
        model.Questions.Add(question);
        return model;
    }
}