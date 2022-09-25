using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Client;
using Client.LocalStorage;
using Client.Pages;
using Client.Services;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthenticationStateProvider, TokenAuthProvider>();

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<AuthService>();
builder.Services.AddHttpClient<AuthService>(client => client.BaseAddress = new Uri("https://localhost:7168"));
builder.Services.AddHttpClient<TestsService>(client => client.BaseAddress = new Uri("https://localhost:7168"));
builder.Services.AddHttpClient<RegistrationService>(client => client.BaseAddress = new Uri("https://localhost:7168"));
builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();

await builder.Build().RunAsync();