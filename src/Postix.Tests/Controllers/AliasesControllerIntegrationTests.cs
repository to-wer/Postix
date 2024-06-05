using System.Net;
using Postix.Tests.Utils;

namespace Postix.Tests.Controllers;

public class AliasesControllerIntegrationTests(CustomWebApplicationFactory<Program> factory) 
    :IClassFixture<CustomWebApplicationFactory<Program>>
{
    [Fact]
    public async Task Index_ShouldReturnList()
    {
        var client = factory.CreateClient();

        await HtmlHelpers.DoLogin(client, "admin@example.com", "P@ssword1");
        
        var response = await client.GetAsync("/Aliases");
        var content = await HtmlHelpers.GetDocumentAsync(response);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("http://localhost/Aliases", content.BaseUri);
    }
    
    [Fact]
    public async Task Details_ShouldReturnNotFound()
    {
        var client = factory.CreateClient();

        await HtmlHelpers.DoLogin(client, "admin@example.com", "P@ssword1");
        
        var response = await client.GetAsync("/Aliases/Details/99");
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task Details_ShouldReturnView()
    {
        var client = factory.CreateClient();

        await HtmlHelpers.DoLogin(client, "admin@example.com", "P@ssword1");
        
        var response = await client.GetAsync("/Aliases/Details/1");
         var content = await HtmlHelpers.GetDocumentAsync(response);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("http://localhost/Aliases/Details/1", content.BaseUri);
    }
}