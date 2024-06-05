using System.Net;
using Postix.Tests.Utils;

namespace Postix.Tests.Controllers;

public class HomeControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
    : IClassFixture<CustomWebApplicationFactory<Program>>
{
    [Fact]
    public async Task Index_ShouldReturnView()
    {
        var client = factory.CreateClient();

        var response = await client.GetAsync("/");
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("<img src=\"logo-05.jpeg\" class=\"img-fluid\" alt=\"Postix Logo\"/>", content);
    }
}