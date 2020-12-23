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
    public class LogoutModel : PageModel
    {
        private SignInManager<AppUser> signInManager;
        public LogoutModel(SignInManager<AppUser> signInMgr)
        {
            signInManager = signInMgr;
        }

        public async Task OnGetAsync()
        {
            await signInManager.SignOutAsync();
        }
    }
}
