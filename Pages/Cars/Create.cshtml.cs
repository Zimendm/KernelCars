using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KernelCars.Models;

namespace KernelCars.Pages.Cars
{
    public class CreateModel : OwnerNamePageModel
    {
        private readonly KernelCars.Data.DataContext _context;
        public CreateModel(KernelCars.Data.DataContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            PopulateOwnersDropDownList(_context);
            return Page();
        }

        [BindProperty]
        public Car Car { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            var emptyCar = new Car();
            if (await TryUpdateModelAsync<Car>(
                emptyCar,"car",s=>s.CarOwnerId,s=>s.CarModelId))
            {
                _context.Cars.Add(Car);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            PopulateOwnersDropDownList(_context, emptyCar.CarOwnerId);
            return Page();
        }
    }
}
