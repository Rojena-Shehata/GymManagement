using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.AccountViewModels;
using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.Threading.Tasks;

namespace GymManagementPL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IStringLocalizer<AccountController> _stringLocalizer;

        public AccountController(IAccountService accountService,SignInManager<ApplicationUser> signInManager
                                                               ,IStringLocalizer<AccountController> stringLocalizer)
        {
            _accountService = accountService;
            _signInManager = signInManager;
            _stringLocalizer = stringLocalizer;
        }

        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );
            return LocalRedirect(returnUrl);
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
            var  user= await _accountService.ValidateUserAsync(input);
            if (user is  null)
            {
                ModelState.AddModelError("InvalidLogin", _stringLocalizer["invalidLogin"].Value);
                return View(nameof(Login),input);
            }
            var result=await _signInManager.PasswordSignInAsync(user,input.Password,input.RememberMe,false);
            if(result.IsNotAllowed)
                ModelState.AddModelError("InvalidLogin", _stringLocalizer["loginNotAllowed"].Value);
            if(result.IsLockedOut)
                ModelState.AddModelError("InvalidLogin", _stringLocalizer["loginLockedOut"]);
            if(result.Succeeded)
                return RedirectToAction("Index", "Home");

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

        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Register()
        {
            return View();
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> Register(CreateNewUser input)
        {
            if (!ModelState.IsValid)
            {

                ModelState.AddModelError("DataMissed", _stringLocalizer["errors.dataMissing"]);
                return View(nameof(Register), input);
            }
            var result=await _accountService.RegisterAsync(input);
            
                if (result.Succeeded)
            {
                TempData["SuccessMessage"] = string.Format(_stringLocalizer["ActionSuccess"], _stringLocalizer["theAdmin"], _stringLocalizer["created"]);
                return RedirectToAction(nameof(Index));
                }
                
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(nameof(Register), input);
                
          
        }
        [Authorize(Roles = "SuperAdmin")]

        public async Task<IActionResult> Index()
        {
            var Users= await _accountService.GetUsersAsync();
            return View(Users);

        }
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var isDeleted= await _accountService.Delete(id);
            if (isDeleted)
                TempData["SuccessMessage"] = string.Format(_stringLocalizer["ActionSuccess"], _stringLocalizer["theAdmin"], _stringLocalizer["deleted"]);
            else
                TempData["ErrorMessage"] = string.Format(_stringLocalizer["ActionError"], _stringLocalizer["theAdmin"], _stringLocalizer["deleted"]);
            

          return  RedirectToAction(nameof(Index));          

        }

    }
}
