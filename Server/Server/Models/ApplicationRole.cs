using Microsoft.AspNetCore.Identity;

namespace Server.Models;

public class ApplicationRole : IdentityRole
{
    public ApplicationRole(string name) : base(name)
    {
        
    }
}