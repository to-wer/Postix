using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Postix.Models.Accounts;

public class AccountCreateViewModel
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Domain { get; set; }

    [Required]
    [DataType(DataType.Password)] public string Password { get; set; }
    [DataType(DataType.Password)] public string ConfirmPassword { get; set; }

    public int Quota { get; set; }
    public bool Enabled { get; set; }
    public bool SendOnly { get; set; }
    
    public IEnumerable<SelectListItem> Domains { get; set; }
}