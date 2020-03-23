using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KernelCars.Models;
using Microsoft.EntityFrameworkCore;

namespace KernelCars.Pages.Cars
{
    public class DetailsModel : PageModel
    {
        private readonly KernelCars.Data.DataContext _context;
        public DetailsModel(KernelCars.Data.DataContext context)
        {
            _context = context;
        }

        public Car Car  { get; set; }
        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id==null)
            {
                return NotFound();
            }

            Car = await _context.Cars
                .Include(c=>c.CarModel)
                .ThenInclude(c=>c.Manufacturer)
                .Include(c=>c.CarOwner)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (Car==null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
