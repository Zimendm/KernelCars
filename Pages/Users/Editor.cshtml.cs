using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using KernelCars.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KernelCars.Pages.Users
{
    public class EditorModel : AdminPageModel
    {
        public UserManager<AppUser> UserManager;
        public EditorModel(UserManager<AppUser> usrManager)
        {
            UserManager = usrManager;
        }
        [BindProperty]
        [Required]
        public string Id { get; set; }
        [BindProperty]
        [Required]
        public string UserName { get; set; }
        [BindProperty]
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [BindProperty]
        public string Password { get; set; }
        public async Task OnGetAsync(string id)
        {
            AppUser user = await UserManager.FindByIdAsync(id);
            Id = user.Id; UserName = user.UserName; Email = user.Email;
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                AppUser user = await UserManager.FindByIdAsync(Id);
                user.UserName = UserName;
                user.Email = Email;
                IdentityResult result = await UserManager.UpdateAsync(user);
                if (result.Succeeded && !String.IsNullOrEmpty(Password))
                {
                    await UserManager.RemovePasswordAsync(user);
                    result = await UserManager.AddPasswordAsync(user, Password);
                }
                if (result.Succeeded)
                {
                    return RedirectToPage("List");
                }
                foreach (IdentityError err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }
            return Page();
        }
    }
}
