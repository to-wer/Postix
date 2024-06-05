using Microsoft.EntityFrameworkCore;
using Postix.Models;

namespace Postix.Data;

public class AccountsRepository(VmailDbContext dbContext) : IAccountsRepository
{
    public async Task<IEnumerable<Account>> GetAccounts()
    {
        return await dbContext.Accounts.ToListAsync(); 
    }

    public async Task<Account> GetAccount(int id)
    {
        return await dbContext.Accounts.FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task AddAccount(Account account)
    {
        await dbContext.Accounts.AddAsync(account);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAccount(Account account)
    {
        dbContext.Update(account);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAccount(int id)
    {
        dbContext.Accounts.Remove(await dbContext.Accounts.FindAsync(id)?? throw new InvalidOperationException());
        await dbContext.SaveChangesAsync();
    }

    public Task<bool> AccountExists(int id)
    {
        return Task.FromResult(dbContext.Accounts.Any(e => e.Id == id));    
    }
}