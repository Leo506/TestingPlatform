using Server.Definitions.Base;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDefinitions(builder, typeof(Program));

var app = builder.Build();
app.UseDefinitions();

app.UseHttpsRedirection();

app.Run();