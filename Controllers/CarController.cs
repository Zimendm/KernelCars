using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KernelCars.Models;
using KernelCars.Data;
using Microsoft.EntityFrameworkCore;
using KernelCars.Models.ViewModels;

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
    }
}