using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KernelCars.Data;
using Microsoft.EntityFrameworkCore;

namespace KernelCars.Pages.Cars
{
    public class OwnerNamePageModel: PageModel
    {
        public SelectList OwnerNameSL { get; set; }

        public void PopulateOwnersDropDownList(DataContext _context, object selectedOwner=null)
        {
            var ownerQuery = from d in _context.Employees
                             orderby d.FirstName
                             select d;
            OwnerNameSL = new SelectList(ownerQuery.AsNoTracking(), "Id", "FirstName", selectedOwner);
        }
    }
}
