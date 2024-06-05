using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Postix.Models.Aliases;

public class AliasCreateViewModel
{
    [Required] public string SourceUsername { get; set; }
    [Required] public string SourceDomain { get; set; }
    [Required] public string DestinationUsername { get; set; }
    [Required] public string DestinationDomain { get; set; }

    public bool Enabled { get; set; }
    
    public IEnumerable<SelectListItem> Domains { get; set; }
}