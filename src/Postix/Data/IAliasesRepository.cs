using Postix.Models;

namespace Postix.Data;

public interface IAliasesRepository
{
    Task<IEnumerable<Alias>> GetAliases();
    Task<Alias> GetAlias(int id);
    Task AddAlias(Alias alias);
    Task UpdateAlias(Alias alias);
    Task DeleteAlias(int id);
}