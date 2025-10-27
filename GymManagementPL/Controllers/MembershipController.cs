using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class MembershipController : Controller
    {
        public MembershipController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
