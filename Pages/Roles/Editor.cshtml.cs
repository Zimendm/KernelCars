using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KernelCars.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KernelCars.Pages.Roles
{
    public class EditorModel : PageModel
    {
        public UserManager<AppUser> UserManager;
        public RoleManager<IdentityRole> RoleManager;
        public EditorModel(UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }
        public IdentityRole Role { get; set; }
        public Task<IList<AppUser>> Members() => UserManager.GetUsersInRoleAsync(Role.Name);
        public async Task<IEnumerable<AppUser>> NonMembers() => UserManager.Users.ToList().Except(await Members());
        public async Task OnGetAsync(string id)
        {
            Role = await RoleManager.FindByIdAsync(id);
        }
        public async Task<IActionResult> OnPostAsync(string userid, string rolename)
        {
            Role = await RoleManager.FindByNameAsync(rolename);
            AppUser user = await UserManager.FindByIdAsync(userid);
            IdentityResult result;
            if (await UserManager.IsInRoleAsync(user, rolename))
            {
                result = await UserManager.RemoveFromRoleAsync(user, rolename);
            }
            else
            {
                result = await UserManager.AddToRoleAsync(user, rolename);
            }
            if (result.Succeeded)
            {
                return RedirectToPage();
            }
            else
            {
                foreach (IdentityError err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
                return Page();
            }
        }
    }
}
