using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Postix.Controllers;
using Postix.Data;
using Postix.Models;

namespace Postix.Tests.Controllers;

public class DomainsControllerTests
{
    private readonly IDomainsRepository _domainsRepository = Substitute.For<IDomainsRepository>();
    private readonly ILogger<DomainsController> _logger = Substitute.For<ILogger<DomainsController>>();
    
    [Fact]
    public async Task Index_ShouldReturnView()
    {
        var controller = new DomainsController(_domainsRepository, _logger);
        
        var result = await controller.Index() as ViewResult;
        
        Assert.NotNull(result);
    }

    [Fact]
    public async Task Details_Get_ReturnsView()
    {
        _domainsRepository.GetDomain(1).Returns(new Domain { Id = 1, DomainName = "." });
        var controller = new DomainsController(_domainsRepository, _logger);
        
        var result = await controller.Details(1) as ViewResult;
        
        Assert.NotNull(result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(1)]
    public async Task Details_Get_ReturnsNotFound(int? id)
    {
        var controller = new DomainsController(_domainsRepository, _logger);
        
        var result = await controller.Details(id) as NotFoundResult;
        
        Assert.NotNull(result);
        Assert.Equal(404, result.StatusCode);
    }
    
    [Fact]
    public void Create_Get_ReturnsView()
    {
        // Arrange
        var controller = new DomainsController(_domainsRepository, _logger);

        // Act
        var result = controller.Create() as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Create", result.ViewName);
    }

    [Fact]
    public async Task Create_Post_ReturnsRedirectToActionResult()
    {
        var controller = new DomainsController(_domainsRepository, _logger);

        var result = await controller.Create(new Domain { DomainName = "Test" }) as RedirectToActionResult;
        
        Assert.NotNull(result);
    }

    [Fact]
    public async Task Edit_Get_ReturnsView()
    {
        _domainsRepository.GetDomain(1).Returns(new Domain { Id = 1, DomainName = "test.de" });
        var controller = new DomainsController(_domainsRepository, _logger);

        var result = await controller.Edit(1) as ViewResult;

        Assert.NotNull(result);
        Assert.Equal("Edit", result.ViewName);
        var model = result.Model as Domain;
        Assert.NotNull(model);
        Assert.Equal(1,model.Id);
        Assert.Equal("test.de",model.DomainName);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(null)]
    public async Task Edit_Get_ReturnsNotFound_IfDomainDoesNotExist(int? id)
    {
        Substitute.For<IDomainsRepository>();
        var controller = new DomainsController(_domainsRepository, _logger);

        var result = await controller.Edit(id) as NotFoundResult;

        Assert.NotNull(result);
        Assert.Equal(404, result.StatusCode); 
    }

    [Fact]
    public async Task Edit_Post_ReturnsRedirectToActionResult()
    {
        _domainsRepository.GetDomain(1).Returns(new Domain { Id = 1, DomainName = "test.com" });

        var controller = new DomainsController(_domainsRepository, _logger);

        var result = await controller.Edit(1, new Domain { Id = 1, DomainName = "test.de" }) as RedirectToActionResult;

        Assert.NotNull(result);
    }

    [Fact]
    public async Task Delete_Get_ReturnsView()
    {
        _domainsRepository.GetDomain(1).Returns(new Domain { Id = 1, DomainName = "test.de" });
        var controller = new DomainsController(_domainsRepository, _logger);

        var result = await controller.Delete(1) as ViewResult;

        Assert.NotNull(result);
        Assert.Equal("Delete", result.ViewName);
        var model = result.Model as Domain;
        Assert.NotNull(model);
        Assert.Equal(1,model.Id);
        Assert.Equal("test.de",model.DomainName);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(null)] 
    public async Task Delete_Get_ReturnsNotFoundResult_IfDomainDoesNotExist(int? id)
    {
        Substitute.For<IDomainsRepository>();
        var controller = new DomainsController(_domainsRepository, _logger);

        var result = await controller.Delete(id) as NotFoundResult;

        Assert.NotNull(result);
        Assert.Equal(404, result.StatusCode); 
    }
    
    [Fact]
    public async Task Delete_Post_ReturnsRedirectToActionResult()
    {
        Substitute.For<IDomainsRepository>();
        _domainsRepository.GetDomain(1).Returns(new Domain { Id = 1, DomainName = "test.de" });

        var controller = new DomainsController(_domainsRepository, _logger);

        var result = await controller.DeleteConfirmed(1) as RedirectToActionResult;

        Assert.NotNull(result);
    }
}