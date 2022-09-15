using System.Net;
using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Server.IntegrationTests;

public class RegistrationTests
{
    [Fact]
    public async Task Registration_NewUser_Success()
    {
        var application = new WebApplicationFactory<Program>();
        
        var client = application.CreateClient();
        client.BaseAddress = new Uri("https://localhost:7168");

        var message = new HttpRequestMessage(new HttpMethod("post"), "/connect/registration");
        var messageDict = new Dictionary<string, string>()
        {
            ["username"] = "TestUser",
            ["password"] = "12345678",
            ["email"] = "some@some.com"
        };
        message.Content = new FormUrlEncodedContent(messageDict);

        var response = await client.SendAsync(message);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}