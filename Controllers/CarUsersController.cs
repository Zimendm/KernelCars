using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KernelCars.Data;
using KernelCars.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KernelCars.Controllers
{
    public class CarUsersController : Controller
    {
        private DataContext _context;
        public CarUsersController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List(long? carId)
        {
            var drivers = _context.CarUsers.Include(c => c.Employee).Where(c => c.CarId == carId).ToList();

            string text = "";
            foreach (var item in drivers)
            {
                text += "\n" + item.Employee.FullName;
            }

            var car = _context.Cars.Include(c=>c.CarModel).ThenInclude(c=>c.Manufacturer).Where(c => c.Id == carId).FirstOrDefault();
            if (car!=null)
            {
                ViewData["RegistrationNumber"] = car.RegistrationNumber;
                ViewData["Model"] = car.CarModel.Model;
                ViewData["Manufacturer"] = car.CarModel.Manufacturer.Name;
                ViewData["carId"] = car.Id;
            }


            //ViewData["RegistrationNumber"] = _context.Cars.Where(c => c.Id == carId).Select(c => c.RegistrationNumber).FirstOrDefault();


            return View(drivers);
        }

        public IActionResult Assign(long? carId)
        {
            var drivers = _context.Employees.ToList().Where(d=>!d.IsFirm);
            return View(drivers);
        }

    }
}