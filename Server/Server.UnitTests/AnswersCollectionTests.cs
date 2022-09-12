using Server.Models;

namespace Server.UnitTests;

[TestFixture]
public class AnswersCollectionTests
{
    [Test]
    public void Add_OneAnswer_Success()
    {
        var collection = new AnswersCollection();

        collection.Add(new Answer()
        {
            Text = "Answer 1"
        });
        
        Assert.That(collection.Count(), Is.EqualTo(1));
    }

    [Test]
    public void Add_SomeAnswers_Success()
    {
        var collection = new AnswersCollection();
        
        collection.Add(new []
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
        
        Assert.That(collection.Count(), Is.EqualTo(2));
    }

    [Test]
    public void Add_TwoCorrectAnswers_Throws()
    {
        var collection = new AnswersCollection();

        var ex = Assert.Catch(() =>
        {

            collection.Add(new Answer()
            {
                Text = "Answer 1",
                IsCorrect = true
            });

            collection.Add(new Answer()
            {
                Text = "Answer 2",
                IsCorrect = true
            });
        });
        
        StringAssert.Contains("Can not add second correct answer", ex?.Message);
    }

    [Test]
    public void Add_TwoSameAnswers_Throws()
    {
        var collection = new AnswersCollection();

        var ex = Assert.Catch(() =>
        {
            collection.Add(new Answer()
            {
                Text = "Answer 1",
                IsCorrect = true
            });

            collection.Add(new Answer()
            {
                Text = "Answer 1"
            });
        });
        
        StringAssert.Contains("Can not add two answers with the same text", ex?.Message);
    }
}