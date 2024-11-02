using System.ComponentModel.DataAnnotations;

namespace VillaApp.Web.ViewModels;

public class LoginVM
{
    [Required]
    public String? Email       { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public String? Password    { get; set; }
    public Boolean RememberMe  { get; set; }
    public String? RedirectURL { get; set; }
}