using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KernelCars.Data;
using KernelCars.Models;

namespace KernelCars.Controllers
{
    public class FirmDepartmentsController : Controller
    {
        private readonly DataContext _context;

        public FirmDepartmentsController(DataContext context)
        {
            _context = context;
        }

        // GET: FirmDepartments
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.FirmDepartment.Include(f => f.Department).Include(f => f.Firm);
            return View(await dataContext.ToListAsync());
        }

        // GET: FirmDepartments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var firmDepartment = await _context.FirmDepartment
                .Include(f => f.Department)
                .Include(f => f.Firm)
                .FirstOrDefaultAsync(m => m.FirmId == id);
            if (firmDepartment == null)
            {
                return NotFound();
            }

            return View(firmDepartment);
        }

        // GET: FirmDepartments/Create
        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentId");
            ViewData["FirmId"] = new SelectList(_context.Firms, "FirmId", "FirmId");
            return View();
        }

        // POST: FirmDepartments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirmId,DepartmentId")] FirmDepartment firmDepartment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(firmDepartment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentId", firmDepartment.DepartmentId);
            ViewData["FirmId"] = new SelectList(_context.Firms, "FirmId", "FirmId", firmDepartment.FirmId);
            return View(firmDepartment);
        }

        // GET: FirmDepartments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var firmDepartment = await _context.FirmDepartment.FindAsync(id);
            if (firmDepartment == null)
            {
                return NotFound();
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentId", firmDepartment.DepartmentId);
            ViewData["FirmId"] = new SelectList(_context.Firms, "FirmId", "FirmId", firmDepartment.FirmId);
            return View(firmDepartment);
        }

        // POST: FirmDepartments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FirmId,DepartmentId")] FirmDepartment firmDepartment)
        {
            if (id != firmDepartment.FirmId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(firmDepartment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FirmDepartmentExists(firmDepartment.FirmId))
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
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentId", firmDepartment.DepartmentId);
            ViewData["FirmId"] = new SelectList(_context.Firms, "FirmId", "FirmId", firmDepartment.FirmId);
            return View(firmDepartment);
        }

        // GET: FirmDepartments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var firmDepartment = await _context.FirmDepartment
                .Include(f => f.Department)
                .Include(f => f.Firm)
                .FirstOrDefaultAsync(m => m.FirmId == id);
            if (firmDepartment == null)
            {
                return NotFound();
            }

            return View(firmDepartment);
        }

        // POST: FirmDepartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var firmDepartment = await _context.FirmDepartment.FindAsync(id);
            _context.FirmDepartment.Remove(firmDepartment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FirmDepartmentExists(int id)
        {
            return _context.FirmDepartment.Any(e => e.FirmId == id);
        }
    }
}
