using System.ComponentModel.DataAnnotations;
using Client.Services;
using Microsoft.AspNetCore.Components;

namespace Client.Pages;

public class LoginModel : ComponentBase
{
    [Inject] public AuthService AuthService { get; set; }
    public LoginViewModel LoginData { get; set; }

    public LoginModel()
    {
        LoginData = new LoginViewModel();
    }

    protected async Task LoginAsync()
    {
        await AuthService.AuthenticateAsync(LoginData.UserName, LoginData.Password);
    }
}

public class LoginViewModel
{
    [Microsoft.Build.Framework.Required]
    [StringLength(50, ErrorMessage = "Too long username")]
    public string UserName { get; set; }
    
    [Required]
    public string Password { get; set; }
}