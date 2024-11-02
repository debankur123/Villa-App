using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VillaApp.Application.Common.Interfaces;
using VillaApp.Infrastructure.Data;
using VillaApp.Web.ViewModels;

namespace VillaApp.Controllers;
public class VillaNumberController(ApplicationDbContext context, IUnitOfWork unitOfWork) : Controller
{
    private readonly ApplicationDbContext _context = context;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public IActionResult Index()
    {
        var villaNumber = _unitOfWork.villaNumberRepo.GetVillas(includeProperties: "Villa");
        return View(villaNumber);
    }
    [HttpGet]
    public IActionResult AddVillaNumber()
    {
        VillaNumberVM villaNumberVM = new()
        {
            VillaList = _unitOfWork.villaRepo.GetVillas(x => x.IsActive == true)
                .Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.Name
                })
        };
        //ViewData["VillaNames"] = villaNames; // This is used to pass values from controller to view and of type dictionary!
        // ViewBag.VillaNames = villaNames; // This is used to pass values from controller to view and type is dynamic!
        return View(villaNumberVM);
    }
    [HttpPost]
    public IActionResult AddVillaNumber(VillaNumberVM obj)
    {
        var roomExists = _unitOfWork.villaNumberRepo.Any(u => u.Villa_Number == obj.VillaNumber!.Villa_Number);
        obj.VillaList = _unitOfWork.villaRepo.GetVillas().Select(u => new SelectListItem
        {
            Text = u.Name.ToString(),
            Value = u.Id.ToString()
        });
        switch (ModelState.IsValid, roomExists)
        {
            case (true, false):
                _unitOfWork.villaNumberRepo.Add(obj.VillaNumber!);
                _unitOfWork.CommitToDb();
                TempData["success"] = "Villa Number added successfully!";
                return RedirectToAction("Index");
            case (_, true):
                TempData["warning"] = "Villa Number already taken!";
                break;
            case (false, _):
                break;
        }
        return View(obj);
    }

    public IActionResult EditVillaNumber(int villaNumberId)
    {
        VillaNumberVM villaNumberVM = new()
        {
            VillaList = _unitOfWork.villaRepo.GetVillas()
                .Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.Name
                }),
            VillaNumber = _unitOfWork.villaNumberRepo.GetVilla(u => u.Villa_Number == villaNumberId)
        };
        if (villaNumberVM.VillaNumber is null)
        {
            return RedirectToAction("Error", "Home");
        }
        return View(villaNumberVM);
    }
    [HttpPost]
    public IActionResult EditVillaNumber(VillaNumberVM input)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "");
            TempData["error"] = "Something went wrong! Please try again";
            //return RedirectToAction("Error", "Home");
        }
        try
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.villaNumberRepo.Update(input.VillaNumber!);
                _unitOfWork.CommitToDb();
                TempData["success"] = "Request modified successfully";
                return RedirectToAction("Index");
            }
            input.VillaList = _unitOfWork.villaRepo.GetVillas()
                .Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.Name
                });
            return View(input);
        }
        catch (Exception)
        {
            return View("Error", "Home");
        }
    }
    //[HttpPost]
    public IActionResult DeleteVillaNumber(int villaNumberId)
    {
        var villaNumberToDelete = _unitOfWork.villaNumberRepo.GetVilla(u => u.Villa_Number == villaNumberId);
        if (villaNumberToDelete == null)
        {
            TempData["error"] = "Something went wrong!";
            return RedirectToAction("Index");
        }
        try
        {
            _context.Tbl_VillaNumber.Remove(villaNumberToDelete);
            _unitOfWork.CommitToDb();
            TempData["success"] = "Villa removed successfully!";
            return RedirectToAction("Index", "VillaNumber");
        }
        catch (Exception)
        {
            TempData["error"] = "Something went wrong!";
            return RedirectToAction("Index");
        }
    }

}
