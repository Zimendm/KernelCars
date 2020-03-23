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
    public class DeleteModel : PageModel
    {
        private readonly KernelCars.Data.DataContext _context;
        public DeleteModel(KernelCars.Data.DataContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Car Car { get; set; }
        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id==null)
            {
                return NotFound();
            }

            Car = await _context.Cars.FirstOrDefaultAsync(c => c.Id == id);
            if (Car==null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (id==null)
            {
                return NotFound();
            }

            Car = await _context.Cars.FindAsync(id);

            if (Car != null)
            {
                _context.Cars.Remove(Car);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
