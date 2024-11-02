using VillaApp.Domains.Entities;

namespace VillaApp.Web.ViewModels;

public class HomeVM
{
    public Villa[]? VillaList     { get; set; }
    public DateOnly? CheckInDate  { get; set; }
    public DateOnly? CheckOutDate { get; set; }
    public int  NoOfNights        { get; set; }
}