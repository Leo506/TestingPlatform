using Microsoft.EntityFrameworkCore;
using BindingFlags = System.Reflection.BindingFlags;

namespace Server.UnitTests.Helpers;

public static class DatabaseHelper
{
    public static T GetDbContext<T>(string dbName) where T : DbContext
    {
        var options = new DbContextOptionsBuilder<T>()
            .UseInMemoryDatabase(dbName)
            .Options;

        var prop = typeof(T).GetConstructor(new[] { typeof(DbContextOptions<T>) });
        return (T)prop.Invoke(new [] {options});
    }
}