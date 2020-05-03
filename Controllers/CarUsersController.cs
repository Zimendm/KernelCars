using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KernelCars.Data;
using KernelCars.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

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
            var drivers = _context.CarUsers.Include(c => c.Employee).Where(c => c.CarId == carId).OrderByDescending(c=>c.ID).ToList();

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
            return View(new CarUser() {StartUsingDate = DateAndTime.Now, CarId=(int) carId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Assign(CarUser carUser, string drivers)
        {
            //Поиск водителя в базе, проверка соответствия
            var driversForSearch = (from o in _context.Employees
                                   select o).ToList().Where(e=>!e.IsFirm);
            foreach (var item in driversForSearch)
            {
                if (item.FullName.Trim().ToUpper() == drivers.Trim().ToUpper())
                {
                    carUser.EmployeeId = item.Id;
                    break;
                }
            }

            var curDriver = (from d in _context.CarUsers
                            where d.EndUsingDate == null
                            select d).ToList();

            foreach (var item in curDriver)
            {
                item.EndUsingDate = carUser.StartUsingDate.AddMinutes(-10);
            }
            _context.CarUsers.Add(carUser);
            _context.SaveChanges();

            return RedirectToAction("List","CarUsers",new { carId=carUser.CarId });
        }
    }
}