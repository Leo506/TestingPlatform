using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Definitions.Database.Contexts;

public class ResultsDbContext : DbContext
{
    public DbSet<ResultModel> Results { get; set; }

    public ResultsDbContext(DbContextOptions<ResultsDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}