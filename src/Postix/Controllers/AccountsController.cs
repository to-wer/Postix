using CryptSharp.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Postix.Data;
using Postix.Models;
using Postix.Models.Accounts;

namespace Postix.Controllers;

[Authorize(Roles = "Administrator")]
public class AccountsController(IAccountsRepository accountsRepository, IDomainsRepository domainsRepository,
    ILogger<AccountsController> logger)
    : Controller
{
    // GET: Accounts
    public async Task<IActionResult> Index()
    {
        return View("Index", await accountsRepository.GetAccounts());
    }

    // GET: Accounts/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var account = await accountsRepository.GetAccount(id.Value);
        if (account == null) return NotFound();

        return View("Details", account);
    }

    // GET: Accounts/Create
    public async Task<IActionResult> Create()
    {
        var viewModel = new AccountCreateViewModel
        {
            Domains = await domainsRepository.GetDomainSelectList(null)
        };
        return View("Create", viewModel);
    }

    // POST: Accounts/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,Username,Domain,Password,ConfirmPassword,Quota,Enabled,SendOnly")]
        AccountCreateViewModel account)
    {
        if (ModelState.IsValid)
        {
            var newAccount = new Account
            {
                Domain = account.Domain,
                Enabled = account.Enabled,
                Quota = account.Quota,
                Username = account.Username,
                HashedPassword = "{SHA512-CRYPT}" + Crypter.Sha512.Crypt(account.Password),
                SendOnly = account.SendOnly
            };

            await accountsRepository.AddAccount(newAccount);
            return RedirectToAction(nameof(Index));
        }

        account.Domains = await domainsRepository.GetDomainSelectList(account.Domain);
        return View("Create", account);
    }

    // GET: Accounts/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var account = await accountsRepository.GetAccount(id.Value);
        if (account == null) return NotFound();
        var viewModel = new AccountEditViewModel
        {
            Domains = await domainsRepository.GetDomainSelectList(account.Domain),
            Domain = account.Domain,
            Enabled = account.Enabled,
            Id = account.Id,
            Quota = account.Quota,
            SendOnly = account.SendOnly,
            Username = account.Username
        };
        return View("Edit", viewModel);
    }

    // POST: Accounts/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
        [Bind("Id,Username,Domain,Quota,Enabled,SendOnly")]
        AccountEditViewModel viewModel)
    {
        if (id != viewModel.Id) return NotFound();

        if (!ModelState.IsValid) return View("Edit", viewModel);
        try
        {
            var dbAccount = await accountsRepository.GetAccount(id);
            if (dbAccount == null) return NotFound();

            dbAccount.Domain = viewModel.Domain;
            dbAccount.Enabled = viewModel.Enabled;
            dbAccount.Quota = viewModel.Quota;
            dbAccount.Username = viewModel.Username;
            dbAccount.SendOnly = viewModel.SendOnly;

            await accountsRepository.UpdateAccount(dbAccount);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in Edit alias");
            ModelState.AddModelError("", ex.Message);
            viewModel.Domains = await domainsRepository.GetDomainSelectList(null);
            return View("Edit", viewModel);
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Accounts/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var account = await accountsRepository.GetAccount(id.Value);
        if (account == null) return NotFound();

        return View("Delete", account);
    }

    /// <summary>
    ///     Deletes an account from the database
    /// </summary>
    /// <param name="id">The ID of the account to delete</param>
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var account = await accountsRepository.GetAccount(id);
        if (account == null) return RedirectToAction(nameof(Index));

        await accountsRepository.DeleteAccount(id);
        return RedirectToAction(nameof(Index));
    }
}