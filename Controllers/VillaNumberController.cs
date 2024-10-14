using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VillaApp.Infrastructure.Data;
using VillaApp.Web.ViewModels;

namespace VillaApp.Controllers;
public class VillaNumberController : Controller
{
    private readonly ApplicationDbContext _context;
    public VillaNumberController(ApplicationDbContext context)
    {
        _context = context;
    }
    public IActionResult Index()
    {
        var villaNumber = _context.Tbl_VillaNumber
            .Include(u => u.Villa)
            .ToList();
        return View(villaNumber);
    }
    [HttpGet]
    public IActionResult AddVillaNumber()
    {
        VillaNumberVM villaNumberVM = new()
        {
            VillaList = _context.Tbl_Villa.ToList()
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
        var roomExists = _context.Tbl_VillaNumber.Any(u => u.Villa_Number == obj.VillaNumber!.Villa_Number);
        obj.VillaList = _context.Tbl_Villa.ToList().Select(u => new SelectListItem
        {
            Text = u.Name.ToString(),
            Value = u.Id.ToString()
        });
        switch (ModelState.IsValid, roomExists)
        {
            case (true, false):
                _context.Tbl_VillaNumber.Add(obj.VillaNumber!);
                _context.SaveChanges();
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
            VillaList = _context.Tbl_Villa.ToList()
                .Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.Name
                }),
            VillaNumber = _context.Tbl_VillaNumber.FirstOrDefault(u => u.Villa_Number == villaNumberId)
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
            ModelState!.AddModelError("", "");
            TempData["error"] = "Something went wrong! Please try again";
            //return RedirectToAction("Error", "Home");
        }
        try
        {
            if (ModelState.IsValid)
            {
                _context.Tbl_VillaNumber.Update(input.VillaNumber!);
                _context.SaveChanges();
                TempData["success"] = "Request modified successfully";
                return RedirectToAction("Index");
            }
            input.VillaList = _context.Tbl_Villa.ToList()
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
        var villaNumberToDelete = _context.Tbl_VillaNumber.FirstOrDefault(u => u.Villa_Number == villaNumberId);
        if (villaNumberToDelete == null)
        {
            TempData["error"] = "Something went wrong!";
            return View();
        }
        try
        {
            _context.Tbl_VillaNumber.Remove(villaNumberToDelete);
            _context.SaveChanges();
            TempData["success"] = "Villa removed successfully!";
        }
        catch (Exception)
        {
            TempData["error"] = "Something went wrong!";
        }
        TempData["error"] = "Something went wrong while processing!";
        return View();
    }

}
