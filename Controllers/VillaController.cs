using Microsoft.AspNetCore.Mvc;
using VillaApp.Application.Common.Interfaces;
using VillaApp.Domains.Entities;
using VillaApp.Infrastructure.Data;

namespace VillaApp.Controllers
{
    public class VillaController(ApplicationDbContext context, IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment) : Controller
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        public IActionResult Index()
        {
            var villas = _unitOfWork.villaRepo.GetVillas(x => x.IsActive == true);
            return View(villas.OrderBy(x => x.Name).ToList());
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
                TempData["warning"] = "Name and Description shouldn't be same!";
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (obj.Image is not null)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
                        String imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"Images/VillaImages");
                        Console.WriteLine("Image path: " + imagePath);
                        //If directory is not being created.
                        if (!Directory.Exists(imagePath))
                        {
                            Directory.CreateDirectory(imagePath);
                            Console.WriteLine("Directory created at: " + imagePath);
                        }
                        using var filestream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
                        obj.Image.CopyTo(filestream);
                        // Add the URL to ImageURL property
                        obj.ImageUrl = Path.Combine("Images", "VillaImages", fileName).Replace("\\", "/");
                    }
                    else
                    {
                        obj.ImageUrl = "";
                    }
                    _unitOfWork.villaRepo.Add(obj!);
                    _unitOfWork.CommitToDb();
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
            var villa = _unitOfWork.villaRepo.GetVilla(x => x.Id == villaId);
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
                    if (input.Image is not null)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(input.Image.FileName);
                        string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"Images/VillaImages");
                        Console.WriteLine("Image path: " + imagePath);
                        //If directory is not being created.
                        if (!Directory.Exists(imagePath))
                        {
                            Directory.CreateDirectory(imagePath);
                            Console.WriteLine("Directory created at: " + imagePath);
                        }
                        if (!string.IsNullOrEmpty(model!.ImageUrl))
                        {
                            var oldImageUrl = Path.Combine(_webHostEnvironment.WebRootPath, model.ImageUrl.TrimStart('\\'));
                            if (System.IO.File.Exists(oldImageUrl))
                            {
                                System.IO.File.Delete(oldImageUrl);
                            }
                        }
                        var filestream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
                        input.Image.CopyTo(filestream);
                        // Add the URL to ImageURL property
                        model!.ImageUrl = Path.Combine("Images", "VillaImages", fileName).Replace("\\", "/");
                    }
                    model!.Name = input.Name;
                    model.Description = input.Description;
                    model.IsActive = true;
                    model.Price = input.Price;
                    model.Sqft = input.Sqft;
                    model.Occupancy = input.Occupancy;
                    //model.ImageUrl = model.ImageUrl;

                    _context.Tbl_Villa.Update(model);
                    _unitOfWork.CommitToDb();
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
                if (!string.IsNullOrEmpty(villaToDelete!.ImageUrl))
                {
                    var oldImageUrl = Path.Combine(_webHostEnvironment.WebRootPath, villaToDelete.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImageUrl))
                    {
                        System.IO.File.Delete(oldImageUrl);
                    }
                }
                _unitOfWork.CommitToDb();
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
}
