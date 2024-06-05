using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Postix.Models;

[Table("accounts")]
public class Account
{
    [Column("id")] public int Id { get; set; }

    [Display(Name = "Username")]
    [Column("username")]
    [MaxLength(64)]
    public string Username { get; set; }

    [Display(Name = "Domain")]
    [Column("domain")]
    [MaxLength(255)]
    public string Domain { get; set; }

    [Display(Name = "Password hash")]
    [Column("password")]
    [MaxLength(255)]
    public string HashedPassword { get; set; }

    [Display(Name = "Quota")]
    [Column("quota")]
    public int Quota { get; set; }

    [Display(Name = "Enabled")]
    [Column("enabled")]
    public bool Enabled { get; set; }

    [Display(Name = "Send only")]
    [Column("sendonly")]
    public bool SendOnly { get; set; }
}