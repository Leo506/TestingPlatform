using System.ComponentModel.DataAnnotations;
using Client.Services;
using Microsoft.AspNetCore.Components;

namespace Client.Pages;

public class RegistrationModel : ComponentBase
{
    public RegistrationViewModel RegistrationData { get; set; }
    
    [Inject] public RegistrationService RegistrationService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }

    public RegistrationModel()
    {
        RegistrationData = new RegistrationViewModel();
    }
    
    
    public async Task RegisterAsync()
    {
        var result = await RegistrationService.RegisterAsync(RegistrationData);

        if (result)
        {
            NavigationManager.NavigateTo("/login");
        }
    }
}

public class RegistrationViewModel
{
    [Required] [EmailAddress] public string Email { get; set; }

    [Required] public string UserName { get; set; }

    [Required] public string Password { get; set; }
}