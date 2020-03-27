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
    public class IndexModel : PageModel
    {
        private readonly KernelCars.Data.DataContext _context;

        public IndexModel(KernelCars.Data.DataContext context)
        {
            _context = context;
        }

        public string NumberSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        public PaginatedList<Car> Cars { get; set; }

        public async Task OnGetAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex)
        {
            CurrentSort = sortOrder;
            NumberSort = String.IsNullOrEmpty(sortOrder) ? "number_desc" : "";
            if (searchString!=null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;

            IQueryable<Car> carIQ = from c in _context.Cars
                                    select c;
            
            if (!String.IsNullOrEmpty(searchString))
            {
                carIQ = carIQ.Where(c=>c.RegistrationNumber.Contains(searchString));
            }
            switch (sortOrder)
            { 
                case "number_desc":
                    carIQ = carIQ.OrderByDescending(c => c.RegistrationNumber);
                    break;
                default:
                    break;
            }
            int pageSize = 3;
            Cars = await PaginatedList<Car>.CreateAsync(carIQ.Include(c=>c.CarOwner).AsNoTracking(),pageIndex??1,pageSize);
        }
    }
}
