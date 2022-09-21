using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;

namespace Client.Pages;

public class LoginModel : ComponentBase
{
    public LoginViewModel LoginData { get; set; }

    public LoginModel()
    {
        LoginData = new LoginViewModel();
    }

    protected Task LoginAsync()
    {
        return Task.CompletedTask;
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