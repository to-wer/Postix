using System.Net;
using Postix.Tests.Utils;

namespace Postix.Tests.Controllers;

public class DomainsControllerIntegrationTests(CustomWebApplicationFactory<Program> factory) :IClassFixture<CustomWebApplicationFactory<Program>>
{
    [Fact]
    public async Task Index_ShouldReturnList()
    {
        var client = factory.CreateClient();

        await HtmlHelpers.DoLogin(client, "admin@example.com", "P@ssword1");
        
        var response = await client.GetAsync("/Domains");
        var content = await HtmlHelpers.GetDocumentAsync(response);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("http://localhost/Domains", content.BaseUri);
    }
    
    [Fact]
    public async Task Details_ShouldReturnDetailPage()
    {
        var client = factory.CreateClient();

        await HtmlHelpers.DoLogin(client, "admin@example.com", "P@ssword1");
        
        var response = await client.GetAsync("/Domains/Details/1");
        var content = await HtmlHelpers.GetDocumentAsync(response);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("http://localhost/Domains/Details/1", content.BaseUri);
    }
    
    [Fact]
    public async Task Edit_Get_ShouldReturnDetailPage()
    {
        var client = factory.CreateClient();

        await HtmlHelpers.DoLogin(client, "admin@example.com", "P@ssword1");
        
        var response = await client.GetAsync("/Domains/Edit/1");
        var content = await HtmlHelpers.GetDocumentAsync(response);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("http://localhost/Domains/Edit/1", content.BaseUri);
    }
}