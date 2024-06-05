using Postix.Models;

namespace Postix.Data;

public interface IAccountsRepository
{
    Task<IEnumerable<Account>> GetAccounts();
    Task<Account> GetAccount(int id);
    Task AddAccount(Account account);
    Task UpdateAccount(Account account);
    Task DeleteAccount(int id);
}