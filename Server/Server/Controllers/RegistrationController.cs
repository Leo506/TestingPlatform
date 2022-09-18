using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using Server.Models;
using Server.ViewModels;

namespace Server.Controllers;

public class RegistrationController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public RegistrationController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost("/connect/registration"), Produces("application/json")]
    public async Task<IActionResult> Register(UserViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var existUser = await _userManager.FindByNameAsync(viewModel.UserName);
        if (existUser != null)
            return BadRequest("There is user with same username");

        var newUser = new ApplicationUser()
        {
            Email = viewModel.Email,
            UserName = viewModel.UserName
        };

        var addingResult = await _userManager.CreateAsync(newUser, viewModel.Password);
        if (!addingResult.Succeeded)
            return StatusCode(503, "Service is not available");  // TODO remove magic values
        
        var addingRoleResult = await _userManager.AddToRoleAsync(newUser, "User");
        if (!addingRoleResult.Succeeded)
        {
            await _userManager.DeleteAsync(newUser);
            return StatusCode(503, "Service is not available"); // TODO remove magic values
        }

        return Ok();
    }
}