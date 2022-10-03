using Microsoft.EntityFrameworkCore;
using BindingFlags = System.Reflection.BindingFlags;

namespace Server.UnitTests.Helpers;

public static class DatabaseHelper
{
    public static T GetDbContext<T>() where T : DbContext
    {
        var options = new DbContextOptionsBuilder<T>()
            .UseInMemoryDatabase(nameof(T))
            .Options;

        var prop = typeof(T).GetConstructor(BindingFlags.Public, new[] { typeof(DbContextOptions<T>) });
        return (T)prop.Invoke(new [] {options});
    }
}