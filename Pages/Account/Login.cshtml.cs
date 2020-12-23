using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using KernelCars.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KernelCars.Pages.Account
{
    public class LoginModel : PageModel
    {
        private SignInManager<AppUser> signInManager;
        public LoginModel(SignInManager<AppUser> signinMgr)
        {
            signInManager = signinMgr;
        }
        [BindProperty]
        [Required]
        public string UserName { get; set; }
        [BindProperty]
        [Required]
        public string Password { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result =
                await signInManager.PasswordSignInAsync(UserName, Password,
                false, false);
                if (result.Succeeded)
                {
                    return Redirect(ReturnUrl ?? "/");
                }
                ModelState.AddModelError("", "Invalid username or password");
            }
            return Page();
        }
    }
}
