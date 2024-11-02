
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using VillaApp.Domains.Entities;

namespace VillaApp.Web.ViewModels;

public class AmenityVM
{
    public Amenity? Amenity { get; set; }
    [ValidateNever]
    public IEnumerable<SelectListItem>? VillaList { get; set; }
}
