using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Linq;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;
using Server.Data;
using Server.Definitions.Database.Contexts;
using Server.Models;
using Server.ViewModels;

namespace Server.Controllers;

[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme,
    Roles = ServerConstants.Roles.Admin + ", " + ServerConstants.Roles.User)]
public class ResultsController : Controller
{
    private ResultsDbContext _dbContext;

    public ResultsController(ResultsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost("result/new")]
    public async Task SaveResult([FromBody] ResultViewModel viewModel)
    {
        var tmp = await _dbContext.Results.FirstOrDefaultAsync(result =>
            result.IdempotencyKey == Guid.Parse(viewModel.IdempotencyKey));
        if (tmp != null)
            return;
        
        var model = new ResultModel()
        {
            UserId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == OpenIddictConstants.Claims.Subject)?.Value ??
                     string.Empty,
            IdempotencyKey = Guid.Parse(viewModel.IdempotencyKey),
            Result = (double)viewModel.CorrectAnswersCount / viewModel.QuestionCount,
            TestId = viewModel.TestId
        };

        _dbContext.Results.Add(model);
        await _dbContext.SaveChangesAsync();
    }
}