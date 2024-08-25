using Microsoft.AspNetCore.Mvc;
using VillaApp.Domains.Entities;
using VillaApp.Infrastructure.Data;

namespace VillaApp.Controllers;
public class VillaController : Controller
{
    private readonly ApplicationDbContext _context;
    public VillaController(ApplicationDbContext context)
    {
        _context = context;
    }
    public IActionResult Index()
    {
        var villas = _context.Tbl_Villa.Where(x => x.IsActive == true).ToList();
        return View(villas);
    }

    public IActionResult AddVilla()
    {
        return View();
    }
    [HttpPost]
    public IActionResult AddVilla(Villa obj)
    {
        if (obj.Name == obj.Description)
        {
            ModelState.AddModelError("", "");
            TempData["warning"] = "Name and Description shouldn't be same!";
        }
        if (!ModelState.IsValid) return View();
        _context.Tbl_Villa.Add(obj);
        _context.SaveChanges();
        TempData["success"] = "Villa added successfully!";
        return RedirectToAction("Index", "Villa");
    }

    public IActionResult EditVilla(int villaId)
    {
        var villa = _context.Tbl_Villa.FirstOrDefault(x => x.Id == villaId);
        if (villa == null)
        {
            return RedirectToAction("Error", "Home");
        }
        return View(villa);
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
