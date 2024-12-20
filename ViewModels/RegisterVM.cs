using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VillaApp.Web.ViewModels;

public class RegisterVM
{
    [Required]
    public String? Email               { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public String? Password            { get; set; }
    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    [DisplayName("Confirm Password")]
    public String? ConfirmPassword     { get; set; }
    public string? Name                { get; set; }
    [DisplayName("Phone Number")]
    public string? PhoneNumber         { get; set; }
    public String? RedirectURL         { get; set; }
    public string? ApplicationRoles    { get; set; }
    public SelectListItem[]? RolesList { get; set; }
    
}