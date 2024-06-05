using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Postix.Authorization;
using Postix.Models;

namespace Postix.Data;

public static class SeedData
{

    public static async Task Initialize(IServiceProvider serviceProvider, string adminUser, string adminPassword)
    {
        var adminId = await EnsureUser(serviceProvider, adminPassword, adminUser); 
        await EnsureRole(serviceProvider, adminId, Constants.AdministratorsRole);

        await using var vmailDbContext =
            new VmailDbContext(serviceProvider.GetRequiredService<DbContextOptions<VmailDbContext>>());
        if (vmailDbContext.Database.ProviderName is "Microsoft.EntityFrameworkCore.InMemory"
            or "Microsoft.EntityFrameworkCore.Sqlite")
        {
            await SetupTestDbAsync(vmailDbContext, CancellationToken.None);
        }
    }

    private static async Task<string> EnsureUser(IServiceProvider serviceProvider,
        string userPassword, string userName)
    {
        var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

        var user = await userManager.FindByNameAsync(userName);
        if (user == null)
        {
            user = new IdentityUser
            {
                UserName = userName,
                Email = userName,
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(user, userPassword);
            if (!result.Succeeded) throw new ArgumentException("The password is probably not strong enough!");
        }
        return user.Id;
    }

    private static async Task EnsureRole(IServiceProvider serviceProvider,
        string uid, string role)
    {
        var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
        
        if (!await roleManager.RoleExistsAsync(role)) await roleManager.CreateAsync(new IdentityRole(role));

        var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

        var user = await userManager.FindByIdAsync(uid);

        if (user == null) throw new ArgumentException("The testUserPw password was probably not strong enough!");

        await userManager.AddToRoleAsync(user, role);
    }

    private static async Task SetupTestDbAsync(VmailDbContext vmailDbContext, CancellationToken cancellationToken = default)
    {
        if (vmailDbContext.Database.ProviderName is "Microsoft.EntityFrameworkCore.InMemory")
        {
            await vmailDbContext.Database.EnsureDeletedAsync(cancellationToken);
            await vmailDbContext.Database.EnsureCreatedAsync(cancellationToken);
        }
        else
        {
            await vmailDbContext.Database.MigrateAsync(cancellationToken);
        }

        await InsertTestDataAsync(vmailDbContext, cancellationToken);
    }

    private static async Task InsertTestDataAsync(VmailDbContext vmailDbContext, CancellationToken cancellationToken)
    {
        const string testDomain = "test.me";
        if (!vmailDbContext.Domains.Any())
            await vmailDbContext.AddAsync(new Domain { Id = 1, DomainName = testDomain }, cancellationToken);    
        if (!vmailDbContext.Accounts.Any())
            await vmailDbContext.AddAsync(new Account { Id = 1, Domain = testDomain, Enabled = true, HashedPassword = "1234567890123456789012345", Quota = 1024, SendOnly = false, Username = "test"}, cancellationToken);
        if (!vmailDbContext.Aliases.Any())
            await vmailDbContext.AddAsync(new Alias { Id = 1, DestinationDomain = testDomain, Enabled = true, DestinationUsername = "test", SourceDomain = testDomain, SourceUsername = ""}, cancellationToken);
        await vmailDbContext.SaveChangesAsync(cancellationToken);
    }
}