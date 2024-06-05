using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Postix.Models;

namespace Postix.Data;

public class DomainsRepository(VmailDbContext dbContext) : IDomainsRepository
{
    public async Task<IEnumerable<Domain>> GetDomains()
    {
        return await dbContext.Domains.ToListAsync();
    }

    public async Task<SelectList> GetDomainSelectList(string selectedDomain)
    {
        var domains = await GetDomains();

        return new SelectList(domains, selectedValue: selectedDomain, dataValueField: nameof(Domain.DomainName), dataTextField: nameof(Domain.DomainName));
    }
    
    public async Task<Domain> GetDomain(int id)
    {
        return await dbContext.Domains.FindAsync(id);
    }

    public async Task AddDomain(Domain domain)
    {
        dbContext.Add(domain);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateDomain(Domain domain)
    {
        dbContext.Update(domain);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteDomain(int id)
    {
        dbContext.Domains.Remove(await dbContext.Domains.FindAsync(id) ?? throw new InvalidOperationException());
        await dbContext.SaveChangesAsync();
    }

    public Task<bool> DomainExists(int id)
    {
        return Task.FromResult(dbContext.Domains.Any(e => e.Id == id));
    }
}