namespace Client.Models;

public class ResultModel
{
    public int QuestionCount { get; set; }
    public int CorrectAnswersCount { get; set; }
    
    public string ReturnUrl { get; set; }
}