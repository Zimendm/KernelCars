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
    public class EditModel : OwnerNamePageModel
    {
        private readonly KernelCars.Data.DataContext _context;

        public EditModel(KernelCars.Data.DataContext context)
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

            Car = await _context.Cars.Include(c=>c.CarOwner).FirstOrDefaultAsync(m => m.Id == id);

            if (Car==null)
            {
                return NotFound();
            }

            PopulateOwnersDropDownList(_context, Car.CarOwnerId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            var carToUpdate = await _context.Cars.FindAsync(id);

            if (carToUpdate==null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<Car>(
                carToUpdate,
                "car",
                c=>c.CarOwnerId,c=>c.CarModelId,c=>c.RegistrationNumber,c=>c.VinNumber))
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            PopulateOwnersDropDownList(_context, carToUpdate.CarOwnerId);
            return Page();
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            //_context.Attach(Car).State = EntityState.Modified;
            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!CarExists(Car.Id))
            //    {
            //        return NotFound();
            //    }

            //    throw;
            //}
            //return RedirectToPage("./Index");

        }

        private bool CarExists(long id)
        {
            return _context.Cars.Any(e => e.Id == id);
        }
    }
}
