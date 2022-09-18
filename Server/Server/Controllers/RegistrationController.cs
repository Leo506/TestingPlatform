using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Server.Data;
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
            return BadRequest(ServerConstants.ExceptionTexts.UserAlreadyExist);

        var newUser = new ApplicationUser()
        {
            Email = viewModel.Email,
            UserName = viewModel.UserName
        };

        var addingResult = await _userManager.CreateAsync(newUser, viewModel.Password);
        if (!addingResult.Succeeded)
        {
            return this.StatusCode(ServerConstants.ServerResponses.ServiceNotAvailable);
        }
        
        var addingRoleResult = await _userManager.AddToRoleAsync(newUser, ServerConstants.Roles.User);
        
        if (addingRoleResult.Succeeded) return Ok();
        
        await _userManager.DeleteAsync(newUser);
        return this.StatusCode(ServerConstants.ServerResponses.ServiceNotAvailable);

    }
}