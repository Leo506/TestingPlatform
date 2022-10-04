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
    #region SaveResult
    
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
    
    #endregion

    #region GetAllResults

    [Fact]
    public async Task GetAllResults_For_User_Returns_Only_User_Results()
    {
        // arrange
        var dbContext = DatabaseHelper.GetDbContext<ResultsDbContext>(nameof(GetAllResults_For_User_Returns_Only_User_Results));
        var sut = new ResultsController(dbContext);
        sut.ControllerContext = new ControllerContext()
        {
            HttpContext = HttpContextHelper.GetUserContext()
        };

        // user result
        dbContext.Results.Add(new ResultModel()
        {
            IdempotencyKey = Guid.NewGuid(),
            Result = 1,
            TestId = "1111",
            UserId = Constants.UserId
        });

        // admin result
        dbContext.Results.Add(new ResultModel()
        {
            IdempotencyKey = Guid.NewGuid(),
            Result = 1,
            TestId = "1111",
            UserId = Constants.AdminId
        });

        await dbContext.SaveChangesAsync();

        const int expected = 1;
        
        // act
        var result = await sut.GetAllResults();
        var tmp = (result.Result as OkObjectResult)?.Value as IEnumerable<ResultModel>;
        var actual = tmp?.Count();
        
        // assert
        Assert.NotNull(tmp);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task GetAllResults_For_Admin_Returns_All_Results()
    {
        // arrange
        var dbContext =
            DatabaseHelper.GetDbContext<ResultsDbContext>(nameof(GetAllResults_For_Admin_Returns_All_Results));
        var sut = new ResultsController(dbContext);
        sut.ControllerContext = new ControllerContext()
        {
            HttpContext = HttpContextHelper.GetAdminContext()
        };

        // user result
        dbContext.Results.Add(new ResultModel()
        {
            IdempotencyKey = Guid.NewGuid(),
            Result = 1,
            TestId = "1111",
            UserId = Constants.UserId
        });

        // admin result
        dbContext.Results.Add(new ResultModel()
        {
            IdempotencyKey = Guid.NewGuid(),
            Result = 1,
            TestId = "1111",
            UserId = Constants.AdminId
        });

        await dbContext.SaveChangesAsync();

        const int expected = 2;
        
        // act
        var result = await sut.GetAllResults();
        var tmp = (result.Result as OkObjectResult)?.Value as IEnumerable<ResultModel>;
        var actual = tmp?.Count();
        
        // assert
        Assert.NotNull(tmp);
        Assert.Equal(expected, actual);
    }

    #endregion
}