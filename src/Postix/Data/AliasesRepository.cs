using Microsoft.EntityFrameworkCore;
using Postix.Models;

namespace Postix.Data;

public class AliasesRepository(VmailDbContext dbContext) : IAliasesRepository
{
    public async Task<IEnumerable<Alias>> GetAliases()
    {
        return await dbContext.Aliases.ToListAsync();
    }

    public async Task<Alias> GetAlias(int id)
    {
        return await dbContext.Aliases.FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task AddAlias(Alias alias)
    {
        await dbContext.Aliases.AddAsync(alias);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAlias(Alias alias)
    {
        dbContext.Update(alias);
        dbContext.Entry(alias).State = EntityState.Modified;
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAlias(int id)
    {
        dbContext.Aliases.Remove(await dbContext.Aliases.FindAsync(id)?? throw new InvalidOperationException());
        await dbContext.SaveChangesAsync();
    }
}