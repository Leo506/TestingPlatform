using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Definitions.Database.Contexts;

public sealed partial class UsersDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<ApplicationUser> Users { get; set; }
    public DbSet<ApplicationRole> Roles { get; set; }

    public UsersDbContext(DbContextOptions<UsersDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
}