using Client.Models;

namespace Client.ViewModels;

public static class TestViewModelExtension
{
    public static TestsModel ToTestModel(this TestViewModel viewModel)
    {
        var questionList = new List<Question>();

        if (viewModel?.QuestionTexts is null)
            return new TestsModel()
            {
                Id = viewModel.Id,
                Questions = questionList
            };
        
        for (var i = 0; i < viewModel.QuestionTexts.Length; i++)
        {
            var question = new Question(viewModel.QuestionTexts[i]);
            for (var j = 0; j < viewModel.Answers[i].Length; j++)
            {
                question.AnswersCollection.Add(new Answer()
                {
                    Text = viewModel.Answers[i][j],
                    IsCorrect = j == viewModel.CorrectAnswersIndex[i]
                });
            }

            questionList.Add(question);
        }

        return new TestsModel()
        {
            Id = viewModel.Id,
            Questions = questionList
        };
    }

    public static TestViewModel ToTestViewModel(this TestsModel model)
    {

        var questionTexts = new List<string>();
        var answersList = new List<List<string>>();
        var correctAnswersIndexes = new List<int>();
        
        foreach (var question in model.Questions)
        {
            questionTexts.Add(question.QuestionText);

            var tmp = new List<string>();
            for (int i = 0; i < question.AnswersCollection.Count(); i++)
            {
                tmp.Add(question.AnswersCollection[i].Text);
                if (question.AnswersCollection[i].IsCorrect)
                    correctAnswersIndexes.Add(i);
            }
            answersList.Add(tmp);
        }

        return new TestViewModel()
        {
            Id = model.Id,
            Answers = answersList.Select(x => x.ToArray()).ToArray(),
            CorrectAnswersIndex = correctAnswersIndexes.ToArray(),
            QuestionTexts = questionTexts.ToArray()
        };
    }
}