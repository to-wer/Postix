using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Postix.Data;
using Postix.Models;

namespace Postix.Controllers;

[Authorize(Roles = "Administrator")]
public class DomainsController(IDomainsRepository domainsRepository, ILogger<DomainsController> logger)
    : Controller
{
    // GET: Domains
    public async Task<IActionResult> Index()
    {
        return View("Index", await domainsRepository.GetDomains());
    }

    // GET: Domains/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var domain = await domainsRepository.GetDomain(id.Value);
        
        if (domain == null) return NotFound();

        return View("Details", domain);
    }

    // GET: Domains/Create
    public IActionResult Create()
    {
        return View("Create");
    }

    // POST: Domains/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,DomainName")] Domain domain)
    {
        if (!ModelState.IsValid) return View("Create", domain);
        await domainsRepository.AddDomain(domain);
        return RedirectToAction(nameof(Index));

    }

    // GET: Domains/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var domain = await domainsRepository.GetDomain(id.Value);
        if (domain == null) return NotFound();
        return View("Edit", domain);
    }

    // POST: Domains/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,DomainName")] Domain domain)
    {
        if (id != domain.Id) return NotFound();

        if (!ModelState.IsValid) return View("Edit", domain);
        try
        {
            await domainsRepository.UpdateDomain(domain);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in Edit alias");
            ModelState.AddModelError("", ex.Message);
            return View("Edit", domain);
        }

        return RedirectToAction(nameof(Index));

    }

    // GET: Domains/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var domain = await domainsRepository.GetDomain(id.Value);
        if (domain == null) return NotFound();

        return View("Delete", domain);
    }

    // POST: Domains/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await domainsRepository.DeleteDomain(id);
        return RedirectToAction(nameof(Index));
    }
}