using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KernelCars.Models;
using KernelCars.Data;
using KernelCars.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace KernelCars.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _context;

        public HomeController(DataContext context)
        {
            _context = context;
        }

        public ViewResult Index()
        {
            //var manufacturersQuery = from m in _context.Manufacturers
            //                         orderby m.Name
            //                         select m;
            //ViewBag.ManufacturerId = new SelectList(manufacturersQuery.AsNoTracking(), "Id", "Name");



            //ViewBag.Manufacturers = _context.Manufacturers.ToList();
            //=> "Hello From Kernel Cars";
            //var dat = _context.Manufacturers.Include(m => m.CarModels).ToList();//.ToArray();
            //ViewBag.Manufacturers = _context.Manufacturers.ToList();
            ////return View(new List<string> {"красный","синий","зелёный" });
            //return View(_context.Manufacturers.Include(m => m.CarModels).ToList());
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Car car, List<IFormFile> images)
        {
            var car1 = _context.Cars.Where(c => c.RegistrationNumber.IndexOf("1938") > -1).FirstOrDefault();

            foreach (var item in images)
            {
                if (item.Length>0)
                {
                    using (var stream = new MemoryStream())
                    {
                        await item.CopyToAsync(stream);
                        car1.ImagePage1 = stream.ToArray();
                    }

                }
            }

            //car.CarModelId = 2;
            //_context.Cars.Add(car);
            _context.SaveChanges();
            int z = 0;
            return RedirectToAction("Index");
        }

        public IActionResult Blazor()
        {
            return View("_Host");
        }
    }
}