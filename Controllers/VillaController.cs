using Microsoft.AspNetCore.Mvc;
using VillaApp.Application.Common.Interfaces;
using VillaApp.Domains.Entities;
using VillaApp.Infrastructure.Data;

namespace VillaApp.Controllers;
public class VillaController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IVillaRepository _repo;
    public VillaController(ApplicationDbContext context, IVillaRepository repo)
    {
        _context = context;
        _repo = repo;
    }
    public IActionResult Index()
    {
        var villas = _repo.GetVillas(x => x.IsActive == true);
        return View(villas.ToList());
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
            ModelState.AddModelError("", "Name and Description shouldn't be the same!");
            TempData["warning"] = "Name and Description shouldn't be the same!";
        }
        if (ModelState.IsValid)
        {
            try
            {
                _repo.Add(obj!);
                _repo.SaveToDb();
                TempData["success"] = "Villa added successfully!";
                return RedirectToAction("Index", "Villa");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                TempData["error"] = "An error occurred while saving the villa.";
            }
        }
        // If we reach here, ModelState is not valid, or something else went wrong
        TempData["error"] = "There was an issue with your submission!";
        return View(obj); 
    }


    public IActionResult EditVilla(int villaId)
    {
        var villa = _repo.GetVilla(x => x.Id == villaId);
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
                _context.Tbl_Villa.Update(model);
                _context.SaveChanges();
                TempData["success"] = "Villa edited successfully!";
                return RedirectToAction("Index", "Villa");
            }
        }
        catch (Exception)
        {
            return View(model);
        }
        TempData["error"] = "Something went wrong! Please try again";
        return View(model);
    }
    public IActionResult DeleteVilla(int Id, Villa obj)
    {
        var villaToDelete = _context.Tbl_Villa.FirstOrDefault(u => u.Id == Id && u.IsActive == true);
        if (villaToDelete is null)
        {
            TempData["error"] = "Something went wrong!";
            return View();
        }
        try
        {
            villaToDelete.IsActive = false;
            _context.SaveChanges();
            TempData["success"] = "Villa removed successfully!";
            return RedirectToAction("Index", "Villa");
        }
        catch (Exception)
        {
            TempData["error"] = "Something went wrong!";
        }
        return View(obj);
    }
}
