using Microsoft.EntityFrameworkCore;
using Postix.Models;

namespace Postix.Data;

public class VmailDbContext(DbContextOptions<VmailDbContext> options) : DbContext(options)
{
    public DbSet<Domain> Domains { get; set; }
    public DbSet<Alias> Aliases { get; set; }
    public DbSet<Account> Accounts { get; set; }
}