using Server.Models;

namespace Server.ViewModels;

public static class TestViewModelExtension
{
    public static TestsModel ToTestModel(this TestViewModel viewModel)
    {
        var test = new TestsModel();

        for (var i = 0; i < viewModel.QuestionTexts.Length; i++)
        {
            var question = new Question(viewModel.QuestionTexts[i]);
            for (var j = 0; j < viewModel.Answers[i].Length; j++)
            {
                question.AddAnswers(new Answer()
                {
                    Text = viewModel.Answers[i][j],
                    IsCorrect = j == viewModel.CorrectAnswersIndex[i]
                });
            }
            test.Questions.Add(question);
        }

        return test;
    }
}