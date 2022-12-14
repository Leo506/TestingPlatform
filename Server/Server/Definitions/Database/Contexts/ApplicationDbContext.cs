using Microsoft.EntityFrameworkCore;

namespace Server.Definitions.Database.Contexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options) 
        { 
            Database.EnsureCreated();
        }

    protected override void OnModelCreating(ModelBuilder modelBuilder) { }
}