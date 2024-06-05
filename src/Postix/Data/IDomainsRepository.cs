using Microsoft.AspNetCore.Mvc.Rendering;
using Postix.Models;

namespace Postix.Data;

public interface IDomainsRepository
{
    Task<IEnumerable<Domain>> GetDomains();
    Task<SelectList> GetDomainSelectList(string selectedDomain);
    Task<Domain> GetDomain(int id);

    Task AddDomain(Domain domain);

    Task UpdateDomain(Domain domain);

    Task DeleteDomain(int id);
}