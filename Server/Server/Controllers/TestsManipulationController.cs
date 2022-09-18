using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation;
using OpenIddict.Validation.AspNetCore;
using Server.Definitions.Database.Repositories;
using Server.Models;

namespace Server.Controllers;

public class TestsManipulationController : Controller
{
    private readonly IRepository<TestsModel> _repository;

    public TestsManipulationController(IRepository<TestsModel> repository)
    {
        _repository = repository;
    }

    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme,
        Roles = "Admin")]
    [HttpGet("/get/tests/all")]
    public async Task GetAllTests()
    {
        await HttpContext.Response.WriteAsJsonAsync(await _repository.GetAllAsync());
    }
}