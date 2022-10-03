using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers;
using Server.Definitions.Database.Contexts;
using Server.Models;
using Server.UnitTests.Helpers;
using Server.ViewModels;

namespace Server.UnitTests;

public class ResultsControllerTests
{
    [Fact]
    public async Task SaveResult_All_Good_Success()
    {
        // arrange
        var dbContext = DatabaseHelper.GetDbContext<ResultsDbContext>(nameof(SaveResult_All_Good_Success));
        var sut = new ResultsController(dbContext);
        sut.ControllerContext = new ControllerContext()
        {
            HttpContext = HttpContextHelper.GetContext()
        };

        // act
        await sut.SaveResult(new ResultViewModel()
        {
            CorrectAnswersCount = 0,
            IdempotencyKey = Guid.NewGuid().ToString(),
            QuestionCount = 1,
            TestId = "1111"
        });
        var actual = dbContext.Results.Any();

        // assert
        Assert.True(actual);
    }

    [Fact]
    public async Task SaveResult_Same_Idempotency_Key_Ignore()
    {
        // arrange
        var dbContext = DatabaseHelper.GetDbContext<ResultsDbContext>(nameof(SaveResult_Same_Idempotency_Key_Ignore));
        var sut = new ResultsController(dbContext);
        sut.ControllerContext = new ControllerContext()
        {
            HttpContext = HttpContextHelper.GetContext()
        };

        var viewModel = new ResultViewModel()
        {
            CorrectAnswersCount = 0,
            IdempotencyKey = Guid.NewGuid().ToString(),
            QuestionCount = 1,
            TestId = "1111"
        };

        const int expected = 1;

        await sut.SaveResult(viewModel);

        // act
        await sut.SaveResult(viewModel);
        var actual = dbContext.Results.Count();
        
        // assert
        Assert.Equal(expected, actual);
    }
}