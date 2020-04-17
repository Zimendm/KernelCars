using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KernelCars.Data;
using KernelCars.Models;
using KernelCars.Models.ViewModels;

namespace KernelCars.Controllers
{
    public class CarServicesController : Controller
    {
        private readonly DataContext _context;

        public CarServicesController(DataContext context)
        {
            _context = context;
        }

        // GET: CarServices
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.CarServices;//.Include(c => c.Car);
            return View(await dataContext.ToListAsync());
            //return View();

        }

        // GET: CarServices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carService = await _context.CarServices
                .Include(c => c.Car)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (carService == null)
            {
                return NotFound();
            }

            return View(carService);
        }

        // GET: CarServices/Create
        public IActionResult Create()
        {
            //rService carservice = _context.CarServices.Include(c => c.WorkAssigments).ThenInclude(c => c.TypeOfService);


            //Todo Вынести в отдельную процедуру

            var allServices = _context.TypeOfServices;


            //var workServices = new HashSet<int>( carservice..w instructor.CourseAssignments.Select(c => c.CourseID));
            
            var viewModel = new List<AssignedTypeOfService>();
            
            foreach (var service in allServices)
            {
                viewModel.Add(new AssignedTypeOfService
                {
                    TypeOfServiceID = service.ID,
                    Service=service.Service
                });
            }
            ViewData["Services"] = viewModel;

            CarService s = new CarService()
            {
                OpenDate = DateTime.Now
            };
            //ViewData["CarId"] = new SelectList(_context.Cars, "Id", "Id");
            return View(s);
        }

        // POST: CarServices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,OpenDate,CompleteDate,ServiceDescription,CarId")] CarService carService)
        {
            if (ModelState.IsValid)
            {
                _context.Add(carService);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarId"] = new SelectList(_context.Cars, "Id", "Id", carService.CarId);


            return View(carService);
        }

        // GET: CarServices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carService = await _context.CarServices.FindAsync(id);
            if (carService == null)
            {
                return NotFound();
            }
            ViewData["CarId"] = new SelectList(_context.Cars, "Id", "Id", carService.CarId);
            return View(carService);
        }

        // POST: CarServices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,OpenDate,CompleteDate,ServiceDescription,CarId")] CarService carService)
        {
            if (id != carService.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carService);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarServiceExists(carService.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarId"] = new SelectList(_context.Cars, "Id", "Id", carService.CarId);
            return View(carService);
        }

        // GET: CarServices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carService = await _context.CarServices
                .Include(c => c.Car)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (carService == null)
            {
                return NotFound();
            }

            return View(carService);
        }

        // POST: CarServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carService = await _context.CarServices.FindAsync(id);
            _context.CarServices.Remove(carService);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarServiceExists(int id)
        {
            return _context.CarServices.Any(e => e.ID == id);
        }
    }
}
