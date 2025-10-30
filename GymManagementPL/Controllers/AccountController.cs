using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.AccountViewModels;
using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GymManagementPL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(IAccountService accountService,SignInManager<ApplicationUser> signInManager)
        {
            _accountService = accountService;
            _signInManager = signInManager;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel  input)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Login),input);
            }
            var  user= await _accountService.ValidateUser(input);
            if (user is  null)
            {
                ModelState.AddModelError("InvalidLogin", "Invalid Email or Password");
                return View(nameof(Login),input);
            }
            var result=await _signInManager.PasswordSignInAsync(user,input.Password,input.RememberMe,false);
            if(result.IsNotAllowed)
                ModelState.AddModelError("InvalidLogin", "Your account is not allowed");
            if(result.IsLockedOut)
                ModelState.AddModelError("InvalidLogin", "Your account is  locked out");
            if(result.Succeeded)
                return RedirectToAction(nameof(Index), "Home");

            return View(nameof(Login), input);


        }

        public  async Task<IActionResult> Logout()
        {
            
             await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
