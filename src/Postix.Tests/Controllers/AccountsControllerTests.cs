using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Postix.Controllers;
using Postix.Data;
using Postix.Models;
using Postix.Models.Accounts;

namespace Postix.Tests.Controllers;

public class AccountsControllerTests
{
    private readonly IAccountsRepository _accountsRepository = Substitute.For<IAccountsRepository>();
    private readonly IDomainsRepository _domainsRepository = Substitute.For<IDomainsRepository>();
    private readonly ILogger<AccountsController> _logger = Substitute.For<ILogger<AccountsController>>();
    
    [Fact]
    public async Task Index_Get_ReturnsView()
    {
        var controller = new AccountsController(_accountsRepository, _domainsRepository, _logger);

        var result = await controller.Index() as ViewResult;

        Assert.NotNull(result);
        Assert.Equal("Index", result.ViewName);
    }
    
    [Fact]
    public async Task Details_Get_ReturnsView()
    {
        _accountsRepository.GetAccount(1).Returns(new Account
        {
            Id = 1,
            Domain = "test.lan",
            Username = "unittest",
            Enabled =true,
            Quota = 1024,
            SendOnly = false,
            HashedPassword = "bla"
        });
        var controller = new AccountsController(_accountsRepository, _domainsRepository, _logger);

        var result = await controller.Details(1) as ViewResult;
        
        Assert.NotNull(result);
        Assert.Equal("Details", result.ViewName);
    }

    [Fact]
    public async Task Create_Get_ReturnsView()
    {
        var accountsRepository = Substitute.For<IAccountsRepository>();
        var domainsRepository = Substitute.For<IDomainsRepository>();
        var controller = new AccountsController(accountsRepository, domainsRepository, _logger);

        var result = await controller.Create() as ViewResult;
        
        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task Create_Post_ReturnsRedirectToIndex()
    {
        var accountsRepository = Substitute.For<IAccountsRepository>();
        var domainsRepository = Substitute.For<IDomainsRepository>();
        var controller = new AccountsController(accountsRepository, domainsRepository, _logger);

        var result = await controller.Create(new AccountCreateViewModel()
        {
            Domain = "test.lan",
            Username = "test",
            Quota = 1024,
            SendOnly = false,
            Enabled = true,
            Password = "Test123",
            ConfirmPassword = "Test123"
        })  as RedirectToActionResult;
        
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
    }

    [Fact]
    public async Task Edit_Get_ReturnsView()
    {
        var accountsRepository = Substitute.For<IAccountsRepository>();
        accountsRepository.GetAccount(1).Returns(new Account());
        var domainsRepository = Substitute.For<IDomainsRepository>();
        var controller = new AccountsController(accountsRepository, domainsRepository, _logger);

        var result = await controller.Edit(1) as ViewResult;
        
        Assert.NotNull(result);
        Assert.Equal("Edit", result.ViewName);
    }
    
    [Fact]
    public async Task Edit_Post_ReturnsRedirectToIndex()
    {
        var accountsRepository = Substitute.For<IAccountsRepository>();
        accountsRepository.GetAccount(1).Returns(new Account());
        var domainsRepository = Substitute.For<IDomainsRepository>();
        var controller = new AccountsController(accountsRepository, domainsRepository, _logger);

        var result = await controller.Edit(1, new AccountEditViewModel
        {
            Id = 1, Domain = "edit.me", Quota = 1024, SendOnly = false, Enabled = true, Username = "test" }) as RedirectToActionResult;
        
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
    }
    
    [Fact]
    public async Task Edit_Post_ReturnsViewOnError()
    {
        var accountsRepository = Substitute.For<IAccountsRepository>();
        accountsRepository.GetAccount(1).Returns(new Account());
        accountsRepository.UpdateAccount(default).ThrowsAsyncForAnyArgs(new Exception("Test"));
        var domainsRepository = Substitute.For<IDomainsRepository>();
        var controller = new AccountsController(accountsRepository, domainsRepository, _logger);

        var result = await controller.Edit(1, new AccountEditViewModel
        {
            Id = 1, Domain = "edit.me", Quota = 1024, SendOnly = false, Enabled = true, Username = "test"
            
        }) as ViewResult;
        
        Assert.NotNull(result);
        Assert.Equal("Edit", result.ViewName);
    }
    
    [Fact]
    public async Task Delete_Get_ReturnsView()
    {
        _accountsRepository.GetAccount(1).Returns(new Account());
        var controller = new AccountsController(_accountsRepository, _domainsRepository, _logger);

        var result = await controller.Delete(1) as ViewResult;
        
        Assert.NotNull(result);
        Assert.Equal("Delete", result.ViewName);
    }
    
    [Fact]
    public async Task Delete_Post_ReturnsRedirectToIndex()
    {
        var controller = new AccountsController(_accountsRepository, _domainsRepository, _logger);

        var result = await controller.DeleteConfirmed(1) as RedirectToActionResult;
        
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
    }
}