using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Postix.Data;

namespace Postix;

public class Program
{
    public static async Task Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                               throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        if (builder.Environment.EnvironmentName == "Local")
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(connectionString));
        else
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));

        var vmailConnectionString = builder.Configuration.GetConnectionString("VmailConnection");
        if (!string.IsNullOrEmpty(vmailConnectionString))
        {
            var serverVersion = ServerVersion.AutoDetect(vmailConnectionString);
            builder.Services.AddDbContext<VmailDbContext>(o =>
                o.UseMySql(vmailConnectionString, serverVersion)
            );
        }

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddScoped<IDomainsRepository, DomainsRepository>();
        builder.Services.AddScoped<IAccountsRepository, AccountsRepository>();
        builder.Services.AddScoped<IAliasesRepository, AliasesRepository>();

        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        builder.Services.AddControllersWithViews();

        builder.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders =
                ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        });

        builder.Services.AddHealthChecks();

        var app = builder.Build();

        app.UseForwardedHeaders();

// Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        else
        {
            app.UseMigrationsEndPoint();
            app.UseDeveloperExceptionPage();
        }

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<ApplicationDbContext>();
            await context.Database.EnsureCreatedAsync();

            var adminUser = builder.Configuration.GetValue<string>("AdminUsername");
            var adminPassword = builder.Configuration.GetValue<string>("AdminPassword");

            await SeedData.Initialize(services, adminUser, adminPassword);
        }

        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            "default",
            "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        app.MapHealthChecks("/healthz");

        await app.RunAsync();

    }
}