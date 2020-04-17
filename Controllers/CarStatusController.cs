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
    public class CarStatusController : Controller
    {
        private readonly DataContext _context;
        private int PageSize = 1;

        public CarStatusController(DataContext context)
        {
            _context = context;
        }

        // GET: CarStatus
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.CarStatuses.Include(c => c.Car).Include(c => c.Status).Include(c => c.Unit);
            return View(await dataContext.ToListAsync());
        }

        public ViewResult List(string status, int carPage = 1)
        {
            List<CarStatus> cs;
            if (String.IsNullOrEmpty(status))
            {
                cs= _context.CarStatuses.Include(s => s.Status).Include(s => s.Car)
                    .ToList();
            }
            else
            {
                cs = _context.CarStatuses.Include(s => s.Status).Include(s => s.Car)
                    .Where(s => s.Status.State == status)
                    .ToList();
            }
            

            return View(
                new CarStatusListViewModel
                {
                    CarStatuses = cs
                     .Skip((carPage - 1) * PageSize)
                    .Take(PageSize),
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = carPage,
                        ItemsPerPage = PageSize,
                        TotalItems = cs.Count()
                    },
                    CurrentStatus = status
                }
                ); 
        }

        // GET: CarStatus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carStatus = await _context.CarStatuses
                .Include(c => c.Car)
                .Include(c => c.Status)
                .Include(c => c.Unit)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (carStatus == null)
            {
                return NotFound();
            }

            return View(carStatus);
        }

        // GET: CarStatus/Create
        public IActionResult Create()
        {
            ViewData["CarId"] = new SelectList(_context.Cars, "Id", "Id");
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusID", "StatusID");
            ViewData["UnitId"] = new SelectList(_context.Units, "UnitId", "UnitId");
            return View();
        }

        // POST: CarStatus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,BeginDate,EndDate,CarId,StatusId,UnitId,Comment")] CarStatus carStatus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(carStatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarId"] = new SelectList(_context.Cars, "Id", "Id", carStatus.CarId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusID", "StatusID", carStatus.StatusId);
            ViewData["UnitId"] = new SelectList(_context.Units, "UnitId", "UnitId", carStatus.UnitId);
            return View(carStatus);
        }

        // GET: CarStatus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carStatus = await _context.CarStatuses.FindAsync(id);
            if (carStatus == null)
            {
                return NotFound();
            }
            ViewData["CarId"] = new SelectList(_context.Cars, "Id", "Id", carStatus.CarId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusID", "StatusID", carStatus.StatusId);
            ViewData["UnitId"] = new SelectList(_context.Units, "UnitId", "UnitId", carStatus.UnitId);
            return View(carStatus);
        }

        // POST: CarStatus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,BeginDate,EndDate,CarId,StatusId,UnitId,Comment")] CarStatus carStatus)
        {
            if (id != carStatus.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carStatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarStatusExists(carStatus.ID))
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
            ViewData["CarId"] = new SelectList(_context.Cars, "Id", "Id", carStatus.CarId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusID", "StatusID", carStatus.StatusId);
            ViewData["UnitId"] = new SelectList(_context.Units, "UnitId", "UnitId", carStatus.UnitId);
            return View(carStatus);
        }

        // GET: CarStatus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carStatus = await _context.CarStatuses
                .Include(c => c.Car)
                .Include(c => c.Status)
                .Include(c => c.Unit)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (carStatus == null)
            {
                return NotFound();
            }

            return View(carStatus);
        }

        // POST: CarStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carStatus = await _context.CarStatuses.FindAsync(id);
            _context.CarStatuses.Remove(carStatus);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarStatusExists(int id)
        {
            return _context.CarStatuses.Any(e => e.ID == id);
        }
    }
}
