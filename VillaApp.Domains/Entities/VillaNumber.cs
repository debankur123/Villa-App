using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace VillaApp.Domains.Entities;

public class VillaNumber
{
    [Key,DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "Villa Number")]
    [Required(ErrorMessage = "Villa Number is required")]
    public required int Villa_Number { get; set; }
    [ForeignKey("Villa")]
    [Required(ErrorMessage = "Villa Id is required")]
    public required int VillaId      { get; set; }
    [ValidateNever]
    public Villa? Villa              { get; set; }
    public string? SpecialDetails    { get; set; }
}