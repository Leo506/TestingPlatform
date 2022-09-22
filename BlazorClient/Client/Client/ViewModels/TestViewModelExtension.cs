using Client.Models;

namespace Client.ViewModels;

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

    public static TestViewModel ToTestViewModel(this TestsModel model)
    {

        var questionTexts = new List<string>();
        var answers = new List<List<string>>();
        var correctIndexes = new List<int>();
        
        foreach (var question in model.Questions)
        {
            questionTexts.Add(question.QuestionText);

            var tmp = new List<string>();

            for (int i = 0; i < question.AnswersCount; i++)
            {
                var a = question.AnswersCollection[i];
                if (a.IsCorrect)
                    correctIndexes.Add(i);
                tmp.Add(a.Text);
            }
            
            answers.Add(tmp);
        }

        var answerArray = new string[answers.Count][];
        for (var i = 0; i < answerArray.Length; i++)
        {
            answerArray[i] = answers[i].ToArray();
        }

        return new TestViewModel()
        {
            Answers = answerArray,
            QuestionTexts = questionTexts.ToArray(),
            CorrectAnswersIndex = correctIndexes.ToArray()
        };
    }
}