using System.ComponentModel.DataAnnotations;

namespace Server.ViewModels;

public class UserViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
    
    [Required]
    public string UserName { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    [Required]
    public string Role { get; set; } = null!;
}