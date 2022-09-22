using System.Text.Json;
using Client.Models;

namespace Client.UnitTests;

public class JsonSerializationTests
{
    private TestsModel MakeTest()
    {
        var question = new Question("QuestionText")
        {
            AnswersCollection = new AnswersCollection()
        };
        question.AnswersCollection.Add(new []
        {
            new Answer()
            {
                Text = "Answer",
                IsCorrect = true
            }
        });

        var test = new TestsModel();
        test.Questions.Add(question);

        return test;
    }

    [Test]
    public void Serialization_Success()
    {
        var test = MakeTest();
        
        var json = JsonSerializer.Serialize(test);

        const string expected = "{\"id\":null,\"questions\":[{"+
                                "\"questionText\":\"QuestionText\",\"status\":0,\"answersCollection\":" +
                                "[{\"text\":\"Answer\",\"isCorrect\":true}]}]}";
        
        Assert.That(json, Is.EqualTo(expected));
    }

    [Test]
    public void Deserialize_Success()
    {
        var json = "{\"id\":null,\"questions\":[{"+
                   "\"questionText\":\"QuestionText\",\"status\":0,\"answersCollection\":" +
                   "[{\"text\":\"Answer\",\"isCorrect\":true}]}]}";

        var testModel = JsonSerializer.Deserialize<TestsModel>(json);
        
        Assert.That(testModel, Is.EqualTo(MakeTest()));
    }
}