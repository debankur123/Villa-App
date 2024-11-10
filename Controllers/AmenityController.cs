using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VillaApp.Application.Common.Interfaces;
using VillaApp.Application.Common.Utilities;
using VillaApp.Infrastructure.Data;
using VillaApp.Web.ViewModels;

namespace VillaApp.Controllers
{
    [Authorize(Roles = StaticDetails.Role_Admin)]
    public class AmenityController(ApplicationDbContext context, IUnitOfWork unitOfWork) : Controller
    {
        internal IUnitOfWork UnitOfWork => unitOfWork;
        public IActionResult Index()
        {
            var amenities = UnitOfWork.amenityRepo.GetVillas(includeProperties: "Villa");
            return View(amenities);
        }
        [HttpGet]
        public IActionResult AddAmenity()
        {
            AmenityVM amenityVM = new()
            {
                VillaList = UnitOfWork.villaRepo.GetVillas(x => x.IsActive == true)
                    .Select(u => new SelectListItem
                    {
                        Value = u.Id.ToString(),
                        Text = u.Name
                    })
            };
            //ViewData["VillaNames"] = villaNames; // This is used to pass values from controller to view and of type dictionary!
            // ViewBag.VillaNames = villaNames; // This is used to pass values from controller to view and type is dynamic!
            return View(amenityVM);
        }
        [HttpPost]
        public IActionResult AddAmenity(AmenityVM obj)
        {
            //var roomExists = _unitOfWork.AmenityRepo.Any(u => u.Villa_Number == obj.Amenity!.Villa_Number);
            if(ModelState.IsValid){
                UnitOfWork.amenityRepo.Add(obj.Amenity!);
                UnitOfWork.CommitToDb();
                TempData["success"] = "Amenity added successfully!";
                return RedirectToAction("Index");
            }
            obj.VillaList = UnitOfWork.villaRepo
                .GetVillas(x => x.IsActive == true)
                .Select(u => new SelectListItem{
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            return View(obj);
        }

        public IActionResult EditAmenity(int AmenityId)
        {
            AmenityVM AmenityVM = new()
            {
                VillaList = UnitOfWork.villaRepo.GetVillas(x => x.IsActive == true)
                    .Select(u => new SelectListItem
                    {
                        Value = u.Id.ToString(),
                        Text = u.Name
                    }),
                Amenity = UnitOfWork.amenityRepo.GetVilla(u => u.Id == AmenityId)
            };
            if (AmenityVM.Amenity is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(AmenityVM);
        }
        [HttpPost]
        public IActionResult EditAmenity(AmenityVM input)
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
                    UnitOfWork.amenityRepo.Update(input.Amenity!);
                    UnitOfWork.CommitToDb();
                    TempData["success"] = "Request modified successfully";
                    return RedirectToAction("Index");
                }
                input.VillaList = UnitOfWork.villaRepo.GetVillas(x => x.IsActive == true)
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
        public IActionResult DeleteAmenity(int AmenityId)
        {
            var AmenityToDelete = UnitOfWork.amenityRepo.GetVilla(u => u.Id== AmenityId);
            if (AmenityToDelete is  null)
            {
                TempData["error"] = "Something went wrong!";
                return RedirectToAction("Index");
            }
            try
            {
                context.Tbl_Amenities.Remove(AmenityToDelete);
                UnitOfWork.CommitToDb();
                TempData["success"] = "Amenities removed successfully!";
                return RedirectToAction("Index", "Amenity");
            }
            catch (Exception)
            {
                TempData["error"] = "Something went wrong!";
                return RedirectToAction("Index");
            }
        }

    }
}