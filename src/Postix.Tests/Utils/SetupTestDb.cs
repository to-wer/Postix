using Postix.Data;

namespace Postix.Tests.Utils;

public static class SetupTestDb
{
    public static void InitializeApplicationDbContext(ApplicationDbContext applicationDbContext)
    {
        applicationDbContext.Database.EnsureDeleted();
        applicationDbContext.Database.EnsureCreated();
    }

    public static void InitializeVmailDbContext(VmailDbContext vmailDbContext)
    {
        vmailDbContext.Database.EnsureDeleted();
        vmailDbContext.Database.EnsureCreated();
    }
}