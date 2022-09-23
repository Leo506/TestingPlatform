using Calabonga.OperationResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Validation;
using OpenIddict.Validation.AspNetCore;
using Server.Data;
using Server.Definitions.Database.Repositories;
using Server.Models;
using Server.ViewModels;

namespace Server.Controllers;

[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme,
    Roles = ServerConstants.Roles.Admin + ", " + ServerConstants.Roles.User)]
public class TestsManipulationController : Controller
{
    private readonly IRepository<TestsModel> _repository;

    public TestsManipulationController(IRepository<TestsModel> repository)
    {
        _repository = repository;
    }
    
    
    [HttpGet("/get/tests/all")]
    public async Task<IActionResult> GetAllTests()
    {
        var result = new OperationResult<IEnumerable<TestsModel>>();
        
        if (HttpContext.User.IsInRole(ServerConstants.Roles.Admin))
            result = await _repository.GetAllAsync();
        else
        {
            var userId =
                HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == OpenIddictConstants.Claims.Subject);
            result = await _repository.GetAllAsync(userId?.Value);
        }
        if (!result.Ok)
            return BadRequest();

        var toReturn = result.Result.Select(x => x.ToTestViewModel());

        return Ok(toReturn);
    }


    [HttpGet("/get/tests/{id}")]
    public async Task<IActionResult> GetTest(string id)
    {
        var result = await _repository.GetAsync(id);
        if (!result.Ok)
            return BadRequest();

        return Ok(result.Result);
    }


    [HttpPost("/create/test")]
    public async Task<IActionResult> CreateTest([FromBody]TestViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var testModel = viewModel.ToTestModel();
        var userId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == OpenIddictConstants.Claims.Subject);
        testModel.UserId = userId.Value;
        await _repository.CreateAsync(testModel);

        return Ok();
    }

    [HttpPut("/update/test/{id}")]
    public async Task<IActionResult> UpdateTest(string id, [FromBody] TestViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var testModel = viewModel.ToTestModel();
        testModel.Id = id;
        var userId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == OpenIddictConstants.Claims.Subject);
        testModel.UserId = userId.Value;
        var result = await _repository.UpdateAsync(testModel);
        return result.Ok ? Ok() : this.StatusCode(ServerConstants.ServerResponses.ServiceNotAvailable);
    }

    [HttpDelete("/delete/test/{id}")]
    public async Task<IActionResult> DeleteTest(string id)
    {
        var result = await _repository.DeleteAsync(id);
        return result.Ok ? Ok() : this.StatusCode(ServerConstants.ServerResponses.ServiceNotAvailable);
    }
}