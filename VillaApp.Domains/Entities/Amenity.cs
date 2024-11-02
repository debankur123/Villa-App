
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace VillaApp.Domains.Entities;

public class Amenity
{
    [Key]
    public Int32 Id             { get; set; }
    public required String Name { get; set; }
    public String? Description  { get; set; }
    [ForeignKey("Villa")]
    public Int32 VillaId        { get; set; }
    [ValidateNever]
    public Villa? Villa         { get; set; }
}
