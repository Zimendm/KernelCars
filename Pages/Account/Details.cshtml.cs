using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KernelCars.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KernelCars.Pages.Account
{
    public class DetailsModel : PageModel
    {
        private UserManager<AppUser> userManager;
        public DetailsModel(UserManager<AppUser> manager)
        {
            userManager = manager;
        }
        public AppUser IdentityUser { get; set; }
        public async Task OnGetAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                IdentityUser = await userManager.FindByNameAsync(User.Identity.Name);
            }
        }
    }
}
