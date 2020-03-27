using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KernelCars.Models;
using KernelCars.Data;
using Microsoft.EntityFrameworkCore;
using KernelCars.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;

namespace KernelCars.Controllers
{
    public class CarController : Controller
    {
        private readonly DataContext _context;

        public int PageSize = 4;

        public CarController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index(int carPage = 1)
        {
            return View(
                new CarsListViewModel {
                    Cars =_context.Cars
                    .Include(c => c.CarModel)
                    .ThenInclude(c => c.Manufacturer)
                    .Include(c=>c.CarOwner)
                    .Skip((carPage - 1) * PageSize)
                    .Take(PageSize)
                    .ToList(),
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage=carPage,
                        ItemsPerPage=PageSize,
                        TotalItems=_context.Cars.Count()
                    }
                }

                //_context.Cars
                //.Include(c=>c.CarModel)
                //.ThenInclude(c=>c.Manufacturer)
                //.Skip((productPage-1)*PageSize)
                //.Take(PageSize)
                //.ToList())


                );
        }

        public IActionResult Create()
        {
            var manufacturerQuery = from m in _context.Manufacturers
                                    orderby m.Name
                                    select m;
            ViewBag.ManufacturerID = new SelectList(manufacturerQuery.AsNoTracking(), "ManufacturerId", "Name",null);

            return View();
        }

        public IActionResult Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = _context.Cars.Include(c => c.CarModel).ThenInclude(c => c.Manufacturer).Include(c=>c.CarOwner)
                .AsNoTracking()
                .FirstOrDefault(m => m.Id == id);

            if (car == null)
            {
                return NotFound();
            }

            ViewBag.Manufacturers = _context.Manufacturers;
            //ViewBag.CarTypes = context.CarTypes.Include(c=>c.Manufacturer);
            //return View("Editor", repository.GetProduct(id));
            var manufacturersQuery = from m in _context.Manufacturers
                                     orderby m.Name
                                     select m;
            ViewBag.ManufacturerId = new SelectList(manufacturersQuery.AsNoTracking(), "ManufacturerId", "Name");

            var carModelsQuery = from m in _context.CarModels
                                     orderby m.Model
                                     select m;
            ViewBag.CarModelId = new SelectList(carModelsQuery.AsNoTracking(), "CarModelId", "Model");


            return View(car);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult EditPost(Car car, string owners)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            var str = owners;

            Car c = _context.Cars.Find(car.Id);
            c.CarModelId = car.CarModelId;
            //c.CarTypeId = car.CarTypeId;

            var owner = (from o in _context.Employees
                        where o.FirstName == owners
                        select o).First();

            if (owner != null)
            {
                c.CarOwnerId = owner.Id;
            }


            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
            //return View();
            //var courseToUpdate = await _context.Courses
            //    .FirstOrDefaultAsync(c => c.CourseID == id);

            //if (await TryUpdateModelAsync<Course>(courseToUpdate,
            //    "",
            //    c => c.Credits, c => c.DepartmentID, c => c.Title))
            //{
            //    try
            //    {
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateException /* ex */)
            //    {
            //        //Log the error (uncomment ex variable name and write a log.)
            //        ModelState.AddModelError("", "Unable to save changes. " +
            //            "Try again, and if the problem persists, " +
            //            "see your system administrator.");
            //    }
            //    return RedirectToAction(nameof(Index));
            //}
            //PopulateDepartmentsDropDownList(courseToUpdate.DepartmentID);
            //return View(courseToUpdate);
        }


        private void PopulateOwnersDropDownList(object selectedOwner = null)
        {
            var ownersQuery = from o in _context.Employees
                              orderby o.FirstName
                              select o;
            ViewBag.OwnerID = new SelectList(ownersQuery.AsNoTracking(), "Id", "FirstName", selectedOwner);
        }

        [HttpGet]
        public JsonResult GetCarTypeList(int ManufacturerId)
        {
            var carTypelist = new SelectList(_context.CarModels.Where(c => c.ManufacturerId == ManufacturerId), "CarModelId", "Model");
            return Json(carTypelist);
        }

        [HttpGet]
        public JsonResult GetCarOwnerList(int ManufacturerId)
        {
            var carTypelist = new SelectList(_context.Employees, "Id", "FirstName");
            return Json(carTypelist);
        }

        //[HttpGet]
        public FileResult DownloadFile()
        {
            int z = 0;

            var testFile = (from f in _context.Cars
                            where f.ImagePage1 != null
                            select f).First();

            //if (testFile ==null)
            //{
            //    return NotFound();
            //}
            return File(testFile.ImagePage1, "image/jpg", "testfile.jpg");
        }
        //public ActionResult GetImage()
        //{
        //    /MemoryStream ms = new MemoryStream();

        //    return null;
        //}
    }
}