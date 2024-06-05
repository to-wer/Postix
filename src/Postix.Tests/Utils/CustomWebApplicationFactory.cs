using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Postix.Data;

namespace Postix.Tests.Utils;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private readonly string _testRunId = Guid.NewGuid().ToString();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            ConfigureApplicationDbContext(services);
            
            ConfigureVmailDbContext(services);
        });

        builder.UseEnvironment("IntegrationTests");
    }

    private void ConfigureVmailDbContext(IServiceCollection services)
    {
        var dbContextDescriptor = services.SingleOrDefault(
            d => d.ServiceType ==
                 typeof(DbContextOptions<VmailDbContext>));

        services.Remove(dbContextDescriptor);
        services.AddDbContext<VmailDbContext>(options =>
        {
            options.UseInMemoryDatabase($"IT_VmailDbContext_{_testRunId}");
        });
    }
    
    private void ConfigureApplicationDbContext(IServiceCollection services)
    {
        var dbContextDescriptor = services.SingleOrDefault(
            d => d.ServiceType ==
                 typeof(DbContextOptions<ApplicationDbContext>));

        services.Remove(dbContextDescriptor);
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseInMemoryDatabase($"IT_ApplicationDbContext_{_testRunId}");
        });
    }
}