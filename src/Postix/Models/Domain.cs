using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Postix.Models;

[Table("domains")]
public class Domain
{
    [Column("id")] public int Id { get; set; }

    [Column("domain"),
    Display(Name = "Domain"),
    Required, MaxLength(255)]
    [RegularExpression(@"^(?:(?!-)[A-Za-z0-9-]{1,63}(?<!-)\.)+[A-Za-z]{2,6}$", 
        ErrorMessage = "This is not a valid domain.")]
    public string DomainName { get; set; }
}