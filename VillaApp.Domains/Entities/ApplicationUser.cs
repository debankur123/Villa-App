using Microsoft.AspNetCore.Identity;

namespace VillaApp.Domains.Entities;

public class ApplicationUser : IdentityUser
{
    public String? Name       { get; set; }
    public DateTime CreatedAt { get; set; }
}