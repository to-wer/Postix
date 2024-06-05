using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Postix.Models;

[Table("aliases")]
public class Alias
{
    [Column("id")] public int Id { get; set; }

    [Display(Name = "Source username")]
    [Column("source_username")]
    [MaxLength(64)]
    public string SourceUsername { get; set; }

    [Display(Name = "Source domain")]
    [Column("source_domain")]
    [MaxLength(255)]
    public string SourceDomain { get; set; }

    [Display(Name = "Destination username")]
    [Column("destination_username")]
    [MaxLength(64)]
    public string DestinationUsername { get; set; }

    [Display(Name = "Destination domain")]
    [Column("destination_domain")]
    [MaxLength(255)]
    public string DestinationDomain { get; set; }

    [Display(Name = "Enabled")]
    [Column("enabled")]
    public bool Enabled { get; set; }
}