using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VillaApp.Domains.Entities;
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
        bool roomExists = _context.Tbl_VillaNumber.Any(u => u.Villa_Number == obj.VillaNumber!.Villa_Number);
        obj.VillaList = _context.Tbl_Villa.ToList().Select(u => new SelectListItem
        {
            Text = u.Name.ToString(),
            Value = u.Id.ToString()
        });
        switch (ModelState.IsValid , roomExists)
        {
            case (true,false):
                _context.Tbl_VillaNumber.Add(obj.VillaNumber!);
                _context.SaveChanges();
                TempData["success"] = "Villa Number added successfully!";
                RedirectToAction("Index");
                break;
            case (_,true):
                TempData["warning"] = "Villa Number already taken!";
                break;
            case (false,_):
                break;
        }
        return View(obj);
    }
    
    public IActionResult EditVilla(int villaNumberId)
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
    public IActionResult EditVilla(int villaId, Villa input)
    {
        var model = _context.Tbl_Villa.FirstOrDefault(val => val.Id == villaId && val.IsActive == true);
        if (model is null)
        {
            ModelState.AddModelError("", "");
            TempData["error"] = "Something went wrong! Please try again";
            //return RedirectToAction("Error", "Home");
        }
        try
        {
            if (ModelState.IsValid)
            {
                model!.Name = input.Name;
                model.Description = input.Description;
                model.IsActive = true;
                model.Price = input.Price;
                model.Sqft = input.Sqft;
                model.Occupancy = input.Occupancy;
                model.ImageUrl = model.ImageUrl;
                _context.Update(model);
                _context.SaveChanges();
            }
            TempData["success"] = "Villa edited successfully!";
            return RedirectToAction("Index", "Villa");
        }
        catch (Exception)
        {
            return View(model);
        }
    }

    public IActionResult DeleteVilla(int villaId)
    {
        var requestId = _context.Tbl_Villa.FirstOrDefault(u => u.Id == villaId);
        if (requestId is null)
        {
            return View("Error", "Home");
        }
        return View(requestId);
    }
    [HttpPost]
    public IActionResult DeleteVilla(int Id, Villa obj)
    {
        var villaToDelete = _context.Tbl_Villa.FirstOrDefault(u => u.Id == Id && u.IsActive == true);
        if (villaToDelete is null)
        {
            TempData["error"] = "Something went wrong!";
        }
        try
        {
            if (villaToDelete is not null)
            {
                villaToDelete.IsActive = false;
            }
            _context.SaveChanges();
            TempData["success"] = "Villa removed successfully!";
            return RedirectToAction("Index");
        }
        catch (Exception)
        {
            TempData["error"] = "Something went wrong!";
        }
        return View("Index");
    }
}
