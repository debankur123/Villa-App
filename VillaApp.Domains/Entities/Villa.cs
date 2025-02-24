using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace VillaApp.Domains.Entities;

public class Villa
{
    public int Id { get; set; }
    [Display(Name = "Villa Name")]
    [MaxLength(50)]
    public required string Name { get; set; }
    [Display(Name = "Villa Description")]
    [Required(ErrorMessage = "Villa Description is required")]
    public string? Description { get; set; }
    [Display(Name = "Price per night")]
    [Required(ErrorMessage = "Price per night is required")]
    [Range(10, 10000)]
    public double? Price { get; set; }
    [Required(ErrorMessage = "Square ft is required")]
    [Range(1, 2000)]
    public int? Sqft { get; set; }
    [Range(1, 10)]
    public int? Occupancy { get; set; }
    [Display(Name = "Image URL")]
    public string? ImageUrl { get; set; }
    [Column(TypeName = "datetime(6)")]
    public DateTime? CreatedDate { get; set; } 
    [Column(TypeName = "datetime(6)")]
    public DateTime? UpdatedDate { get; set; }
    public bool IsActive { get; set; } = true;
    [NotMapped]
    public IFormFile? Image { get; set; }
    [ValidateNever]
    public IEnumerable<Amenity>? AmenityList { get; set; }
    [NotMapped]
    public bool IsAvailable { get; set; } = true;
}
