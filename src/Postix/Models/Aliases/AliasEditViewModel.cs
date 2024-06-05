using Microsoft.AspNetCore.Mvc.Rendering;

namespace Postix.Models.Aliases;

public class AliasEditViewModel
{
    public int Id { get; set; }
    public string SourceUsername { get; set; }
    public string SourceDomain { get; set; }
    public string DestinationUsername { get; set; }
    public string DestinationDomain { get; set; }
    public bool Enabled { get; set; }
    
    public IEnumerable<SelectListItem> Domains { get; set; }
}