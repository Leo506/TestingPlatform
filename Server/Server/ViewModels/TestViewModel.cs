namespace Server.ViewModels;

public class TestViewModel
{
    public string Id { get; set; } = null!;
    public string[] QuestionTexts { get; init; } = null!;

    public string[][] Answers { get; init; } = null!;

    public int[] CorrectAnswersIndex { get; init; } = null!;

    public override bool Equals(object? obj)
    {
        if (obj is not TestViewModel viewModel)
            return false;

        if (QuestionTexts.Length != viewModel.QuestionTexts.Length)
            return false;

        if (Answers.Length != viewModel.Answers.Length)
            return false;

        if (CorrectAnswersIndex.Length != viewModel.CorrectAnswersIndex.Length)
            return false;
        
        for (var i = 0; i < QuestionTexts.Length; i++)
        {
            if (!QuestionTexts[i].Equals(viewModel.QuestionTexts[i]))
                return false;
        }
        
        for (var i = 0; i < Answers.Length; i++)
        {
            if (Answers[i].Length != viewModel.Answers[i].Length)
                return false;

            for (var j = 0; j < Answers[i].Length; j++)
            {
                if (!Answers[i][j].Equals(viewModel.Answers[i][j]))
                    return false;
            }
        }
        
        for (var i = 0; i < CorrectAnswersIndex.Length; i++)
        {
            if (CorrectAnswersIndex[i] != viewModel.CorrectAnswersIndex[i])
                return false;
        }

        return true;
    }
}