using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Postix.Controllers;
using Postix.Data;
using Postix.Models;
using Postix.Models.Aliases;

namespace Postix.Tests.Controllers;

public class AliasesControllerTests
{
    private readonly IAliasesRepository _aliasesRepository = Substitute.For<IAliasesRepository>();
    private readonly IDomainsRepository _domainsRepository = Substitute.For<IDomainsRepository>();
    private readonly ILogger<AliasesController> _logger = Substitute.For<ILogger<AliasesController>>();

    [Fact]
    public async Task Index_Get_ReturnsView()
    {
        var controller = new AliasesController(_aliasesRepository, _domainsRepository, _logger);

        var result = await controller.Index() as ViewResult;

        Assert.NotNull(result);
        Assert.Equal("Index", result.ViewName);
    }
    
    [Fact]
    public async Task Details_Get_ReturnsNotFound()
    {
        var controller = new AliasesController(_aliasesRepository, _domainsRepository, _logger);

        var result = await controller.Details(1) as NotFoundResult;
        
        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task Details_Get_ReturnsView()
    {
        _aliasesRepository.GetAlias(1).Returns(new Alias()
        {
            Id = 1,
            SourceDomain = "source.lan",
            SourceUsername = "unittest",
            Enabled =true,
            DestinationDomain = "destination.lan",
            DestinationUsername = "unittest"
        });
        var controller = new AliasesController(_aliasesRepository, _domainsRepository, _logger);

        var result = await controller.Details(1) as ViewResult;
        
        Assert.NotNull(result);
        Assert.Equal("Details", result.ViewName);
    }
    
    [Fact]
    public async Task Create_Get_ReturnsView()
    {
        var controller = new AliasesController(_aliasesRepository, _domainsRepository, _logger);

        var result = await controller.Create() as ViewResult;
        
        Assert.NotNull(result);
        Assert.Equal("Create", result.ViewName);
    }

    [Fact]
    public async Task Create_Post_ReturnsRedirectToIndex()
    {
        var controller = new AliasesController(_aliasesRepository, _domainsRepository, _logger);

        var result = await controller.Create(new AliasCreateViewModel()
        {
            DestinationDomain = "destination.lan",
            DestinationUsername = "unittest",
            Enabled = true,
            SourceDomain = "source.lan",
            SourceUsername = "unittest"
        }) as RedirectToActionResult;
        
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
    }
    
    [Fact]
    public async Task Edit_Get_ReturnsView()
    {
        _aliasesRepository.GetAlias(1).Returns(new Alias()
        {
            Id = 1,
            SourceDomain = "source.lan",
            SourceUsername = "unittest",
            Enabled =true,
            DestinationDomain = "destination.lan",
            DestinationUsername = "unittest"
        });
        var controller = new AliasesController(_aliasesRepository, _domainsRepository, _logger);

        var result = await controller.Edit(1) as ViewResult;
        
        Assert.NotNull(result);
        Assert.Equal("Edit", result.ViewName);
    }
    
    [Fact]
    public async Task Edit_Post_ReturnsRedirectToIndex()
    {
        _aliasesRepository.GetAlias(1).Returns(new Alias()
        {
            Id = 1,
            SourceDomain = "source.lan",
            SourceUsername = "unittest",
            Enabled =true,
            DestinationDomain = "destination.lan",
            DestinationUsername = "unittest"
        });
        var controller = new AliasesController(_aliasesRepository, _domainsRepository, _logger);

        var result = await controller.Edit(1, new AliasEditViewModel()
        {
            Id = 1,
            DestinationDomain = "destination.lan",
            DestinationUsername = "unittest",
            Enabled = true,
            SourceDomain = "source.lan",
            SourceUsername = "unittest"
        }) as RedirectToActionResult;
        
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
    }
    
    [Fact]
    public async Task Edit_Post_ReturnsViewOnError()
    {
        _aliasesRepository.GetAlias(1).Returns(new Alias()
        {
            Id = 1,
            SourceDomain = "source.lan",
            SourceUsername = "unittest",
            Enabled =true,
            DestinationDomain = "destination.lan",
            DestinationUsername = "unittest"
        });
        _aliasesRepository.UpdateAlias(default).ThrowsAsyncForAnyArgs(new Exception("Test"));
        var controller = new AliasesController(_aliasesRepository, _domainsRepository, _logger);

        var result = await controller.Edit(1, new AliasEditViewModel()
        {
            Id = 1,
            DestinationDomain = "destination.lan",
            DestinationUsername = "unittest",
            Enabled = true,
            SourceDomain = "source.lan",
            SourceUsername = "unittest"
        }) as ViewResult;
        
        Assert.NotNull(result);
        Assert.Equal("Edit", result.ViewName);
    }
    
    [Fact]
    public async Task Delete_Get_ReturnsView()
    {
        _aliasesRepository.GetAlias(1).Returns(new Alias()
        {
            Id = 1,
            SourceDomain = "source.lan",
            SourceUsername = "unittest",
            Enabled =true,
            DestinationDomain = "destination.lan",
            DestinationUsername = "unittest"
        });
        var controller = new AliasesController(_aliasesRepository, _domainsRepository, _logger);

        var result = await controller.Delete(1) as ViewResult;
        
        Assert.NotNull(result);
        Assert.Equal("Delete", result.ViewName);
    }
    
    [Fact]
    public async Task Delete_Post_ReturnsRedirectToIndex()
    {
        var controller = new AliasesController(_aliasesRepository, _domainsRepository, _logger);

        var result = await controller.DeleteConfirmed(1) as RedirectToActionResult;
        
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
    }
}