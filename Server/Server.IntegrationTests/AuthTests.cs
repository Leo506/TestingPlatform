using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Server.IntegrationTests;

public class AuthTests
{
    [Fact]
    public async Task Authorize_CorrectData_Success()
    {
        var application = new WebApplicationFactory<Server.Program>();

        var client = application.CreateClient();
        client.BaseAddress = new Uri("https://localhost:7168");

        var messageDict = new Dictionary<string, string>()
        {
            { "grant_type", "password" },
            { "username", "admin" },
            { "password", "12345678" }
        };
        
        var message = new HttpRequestMessage(new HttpMethod("post"), "/connect/token");
        message.Content = new FormUrlEncodedContent(messageDict);
        var response = await client.SendAsync(message);

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Contains("access_token", responseString);
    }
}