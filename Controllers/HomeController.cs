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

    public IActionResult Privacy()
    {
        return View();
    }
    
    public IActionResult Error()
    {
        return View();
    }
}
