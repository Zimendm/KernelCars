using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KernelCars.Models;
using KernelCars.Data;

namespace KernelCars.Controllers
{
    public class CarController : Controller
    {
        private readonly DataContext _context;
        public CarController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Cars.ToList());
        }
    }
}