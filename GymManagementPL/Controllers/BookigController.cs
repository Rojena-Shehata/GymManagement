using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    [Authorize]
    public class BookigController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
