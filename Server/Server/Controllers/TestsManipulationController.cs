using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation;
using OpenIddict.Validation.AspNetCore;
using Server.Definitions.Database.Repositories;
using Server.Models;
using Server.ViewModels;

namespace Server.Controllers;

[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme,
    Roles = "Admin, User")]
public class TestsManipulationController : Controller
{
    private readonly IRepository<TestsModel> _repository;

    public TestsManipulationController(IRepository<TestsModel> repository)
    {
        _repository = repository;
    }
    
    
    [HttpGet("/get/tests/all")]
    public async Task GetAllTests()
    {
        await HttpContext.Response.WriteAsJsonAsync(await _repository.GetAllAsync());
    }


    [HttpPost("/create/test")]
    public async Task<IActionResult> CreateTest([FromBody]TestViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var testModel = viewModel.ToTestModel();
        await _repository.CreateAsync(testModel);

        return Ok();
    }
}