using Microsoft.AspNetCore.Mvc;
using VillaApp.Application.Common.Interfaces;
using VillaApp.Web.ViewModels;

namespace VillaApp.Controllers;

public class HomeController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public HomeController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        var obj = new HomeVM
        {
            VillaList = [.. _unitOfWork.villaRepo.GetVillas(includeProperties:"AmenityList")],
            NoOfNights = 1,
            CheckInDate = DateOnly.FromDateTime(DateTime.Now)
        };
        return View(obj);
    }
    [HttpPost]
    public IActionResult Index(HomeVM obj)
    {
        obj.VillaList = [.. _unitOfWork.villaRepo.GetVillas(includeProperties:"AmenityList")];
        foreach (var item in obj.VillaList)
        {
            if(item.Id % 2 == 0)
                item.IsAvailable = false;
        }
        return View(obj);
    }
    //[HttpGet]
    [HttpPost]
    public IActionResult GetVillasByDate(int NoOfNights,DateOnly CheckInDate)
    {
        var villaArray =  _unitOfWork.villaRepo.GetVillas(includeProperties:"AmenityList").ToArray();
        foreach (var items in villaArray)
        {
            if(items.Id % 2 == 0)
                items.IsAvailable = false;
        }
        HomeVM obj = new()
        {
            CheckInDate = CheckInDate,
            NoOfNights = NoOfNights,
            VillaList = villaArray
        };
        // ReSharper disable once Mvc.ViewNotResolved
        return PartialView("_VillaList", obj);
    }

    public IActionResult Privacy()
    {
        return View();
    }
    
    public IActionResult Error()
    {
        return View();
    }
}
