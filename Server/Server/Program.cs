using Server.Definitions.Base;

namespace Server;

public partial class Program
{

    public static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddDefinitions(builder, typeof(Program));

        var app = builder.Build();
        app.UseDefinitions();

        app.UseHttpsRedirection();

        app.Run();
    }
}