using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Postix.Models;

public class AccountEditViewModel
{
    [Required] public int Id { get; set; }
    [Required] public string Username { get; set; }
    [Required] public string Domain { get; set; }
    public int Quota { get; set; }
    public bool Enabled { get; set; }
    public bool SendOnly { get; set; }
    
    public IEnumerable<SelectListItem> Domains { get; set; }
}