using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using KernelCars.Models.ViewModels;
using KernelCars.Models;

namespace KernelCars.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        ////////private UserManager<AppUser> userManager;
        ////////private SignInManager<AppUser> signInManager;

        ////////public AccountController(UserManager<AppUser> userMgr, SignInManager<AppUser> signInMgr)
        ////////{
        ////////    userManager = userMgr;
        ////////    signInManager = signInMgr;
        ////////}

        ////////[AllowAnonymous]
        ////////public ViewResult Login(string returnUrl)
        ////////{
        ////////    return View(new LoginModel
        ////////    {
        ////////        ReturnUrl = returnUrl
        ////////    });
        ////////}

        ////////[HttpPost]
        ////////[AllowAnonymous]
        ////////[ValidateAntiForgeryToken]
        ////////public async Task<IActionResult> Login(LoginModel loginModel)
        ////////{
        ////////    ////if (ModelState.IsValid)
        ////////    ////{
        ////////    ////    AppUser user =
        ////////    ////    await userManager.FindByNameAsync(loginModel.Name);
        ////////    ////    if (user != null)
        ////////    ////    {
        ////////    ////        await signInManager.SignOutAsync();
        ////////    ////        Microsoft.AspNetCore.Identity.SignInResult result = 
        ////////    ////            await signInManager.PasswordSignInAsync(user, loginModel.Password, false, false);
                    
                    
        ////////    ////        if (result.Succeeded)
        ////////    ////        {
        ////////    ////            //return Redirect(ReturnUrl ?? "/");
        ////////    ////            //return RedirectToAction("Index", "Car");
        ////////    ////            return Redirect(loginModel?.ReturnUrl ?? "/Home/Index");
        ////////    ////        }
        ////////    ////        //if ((await signInManager.PasswordSignInAsync(user,
        ////////    ////        //loginModel.Password, false, false)).Succeeded)
        ////////    ////        //{
        ////////    ////        //    return Redirect(loginModel?.ReturnUrl ?? "/Home/Index");
        ////////    ////        //}
        ////////    ////    }
        ////////    ////}
        ////////    ////ModelState.AddModelError("", "Invalid name or password");
        ////////    ////return View(loginModel);
        ////////}

        ////////public async Task<RedirectResult> Logout(string returnUrl = "/")
        ////////{
        ////////    ////await signInManager.SignOutAsync();
        ////////    ////return Redirect(returnUrl);
        ////////}
    }
}