using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Postix.Data;
using Postix.Models;
using Postix.Models.Aliases;

namespace Postix.Controllers;

[Authorize(Roles = "Administrator")]
public class AliasesController(IAliasesRepository aliasesRepository, IDomainsRepository domainsRepository, 
    ILogger<AliasesController> logger) : Controller
{
    // GET: Aliases
    public async Task<IActionResult> Index()
    {
        return View("Index", await aliasesRepository.GetAliases());
    }

    // GET: Aliases/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var alias = await aliasesRepository.GetAlias(id.Value);
        if (alias == null) return NotFound();

        return View("Details", alias);
    }

    // GET: Aliases/Create
    public async Task<IActionResult> Create()
    {
        var viewModel = new AliasCreateViewModel()
        {
            Domains = await domainsRepository.GetDomainSelectList(null)
        };
        return View("Create", viewModel);
    }

    // POST: Aliases/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("SourceUsername,SourceDomain,DestinationUsername,DestinationDomain,Enabled")]
        AliasCreateViewModel alias)
    {
        if (ModelState.IsValid)
        {
            await aliasesRepository.AddAlias(new Alias()
            {
                SourceUsername = alias.SourceUsername,
                Enabled = alias.Enabled,
                DestinationDomain = alias.DestinationDomain,
                SourceDomain = alias.SourceDomain,
                DestinationUsername = alias.DestinationUsername
            });
            
            return RedirectToAction(nameof(Index));
        }

        alias.Domains = await domainsRepository.GetDomainSelectList(null);
        return View(alias);
    }

    // GET: Aliases/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var alias = await aliasesRepository.GetAlias(id.Value);
        if (alias == null) return NotFound();
        return View("Edit", new AliasEditViewModel()
        {
            Id = alias.Id,
            Domains = await domainsRepository.GetDomainSelectList(null),
            Enabled = alias.Enabled,
            DestinationDomain = alias.DestinationDomain,
            DestinationUsername = alias.DestinationUsername,
            SourceDomain = alias.SourceDomain,
            SourceUsername = alias.SourceUsername
        });
    }

    // POST: Aliases/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
        [Bind("Id,SourceUsername,SourceDomain,DestinationUsername,DestinationDomain,Enabled")]
        AliasEditViewModel alias)
    {
        if (id != alias.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                var dbAlias = await aliasesRepository.GetAlias(id);
                dbAlias.Enabled = alias.Enabled;
                dbAlias.SourceDomain = alias.SourceDomain;
                dbAlias.SourceUsername = alias.SourceUsername;
                dbAlias.DestinationDomain= alias.DestinationDomain;
                dbAlias.DestinationUsername = alias.DestinationUsername;
                await aliasesRepository.UpdateAlias(dbAlias);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in Edit alias");
                ModelState.AddModelError("", ex.Message);
                alias.Domains = await domainsRepository.GetDomainSelectList(null);
                return View("Edit", alias);
            }

            return RedirectToAction(nameof(Index));
        }

        alias.Domains = await domainsRepository.GetDomainSelectList(null);
        return View("Edit", alias);
    }

    // GET: Aliases/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var alias = await aliasesRepository.GetAlias(id.Value);
        if (alias == null) return NotFound();

        return View("Delete", alias);
    }

    // POST: Aliases/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await aliasesRepository.DeleteAlias(id);

        return RedirectToAction(nameof(Index));
    }
}