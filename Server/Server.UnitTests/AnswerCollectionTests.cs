using Server.Models;
namespace Server.UnitTests;

public class AnswerCollectionTests
{
    [Fact]
    public void CreateCollection_Empty_Success()
    {
        // arrange
        const int expected = 0;
        var sut = new AnswersCollection();

        // act
        var actual = sut.Count();
        
        // assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CreateCollection_Not_Empty_Success()
    {
        // arrange
        const int expected = 1;
        var sut = new AnswersCollection()
        {
            new Answer()
            {
                Text = "Answer 1",
                IsCorrect = true
            }
        };

        // act
        var actual = sut.Count();

        // assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Add_Answer_Success()
    {
        // arrange
        const int expected = 1;
        var sut = new AnswersCollection();

        // act
        sut.Add(new Answer()
        {
            Text = "Answer 1",
            IsCorrect = true
        });
        var actual = sut.Count();

        // assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Add_Two_Same_Answers_Throws()
    {
        // arrange
        var answer1 = new Answer()
        {
            Text = "Answer 1"
        };
        var answer2 = new Answer()
        {
            Text = "Answer 1"
        };

        var sut = new AnswersCollection();

        // act
        sut.Add(answer1);

        // assert
        Assert.Throws<InvalidOperationException>(() => sut.Add(answer2));
    }
}