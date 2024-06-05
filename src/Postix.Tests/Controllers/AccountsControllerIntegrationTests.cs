using System.Net;
using Postix.Tests.Utils;

namespace Postix.Tests.Controllers;

public class AccountsControllerIntegrationTests(CustomWebApplicationFactory<Program> factory) 
    :IClassFixture<CustomWebApplicationFactory<Program>>
{
    [Fact]
    public async Task Index_ShouldReturnList()
    {
        var client = factory.CreateClient();

        await HtmlHelpers.DoLogin(client, "admin@example.com", "P@ssword1");
        
        var response = await client.GetAsync("/Accounts");
        var content = await HtmlHelpers.GetDocumentAsync(response);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("http://localhost/Accounts", content.BaseUri);
    }
    
    [Fact]
    public async Task Details_ShouldReturnView()
    {
        var client = factory.CreateClient();

        await HtmlHelpers.DoLogin(client, "admin@example.com", "P@ssword1");
        
        var response = await client.GetAsync("/Accounts/Details/1");
        var content = await HtmlHelpers.GetDocumentAsync(response);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("http://localhost/Accounts/Details/1", content.BaseUri);
    }    
    
    [Fact]
    public async Task Create_ShouldReturnView()
    {
        var client = factory.CreateClient();

        await HtmlHelpers.DoLogin(client, "admin@example.com", "P@ssword1");
        
        var response = await client.GetAsync("/Accounts/Create");
        var content = await HtmlHelpers.GetDocumentAsync(response);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("http://localhost/Accounts/Create", content.BaseUri);
    }    
}