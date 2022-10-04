using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Client;
using Client.LocalStorage;
using Client.Pages;
using Client.Services;
using Client.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Toolbelt.Blazor.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthenticationStateProvider, TokenAuthProvider>();

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<AuthService>();
builder.Services.AddHttpClient<IRefreshTokenService, RefreshTokenService>(client => client.BaseAddress = new Uri("https://localhost:7168"));
builder.Services.AddHttpClient<AuthService>(client => client.BaseAddress = new Uri("https://localhost:7168"));
builder.Services.AddHttpClient<TestsService>((provider, client) =>
{
    client.BaseAddress = new Uri("https://localhost:7168");
    client.EnableIntercept(provider);
});
builder.Services.AddHttpClient<ResultService>((provider, client) =>
{
    client.BaseAddress = new Uri("https://localhost:7168");
    client.EnableIntercept(provider);
});
builder.Services.AddHttpClient<RegistrationService>(client => client.BaseAddress = new Uri("https://localhost:7168"));
builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
builder.Services.AddHttpClientInterceptor();
builder.Services.AddScoped<IInterceptorService, InterceptorService>();

await builder.Build().RunAsync();