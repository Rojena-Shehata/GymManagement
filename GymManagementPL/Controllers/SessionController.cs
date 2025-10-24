using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        public IActionResult Index()
        {
            var Sessions=_sessionService.GetAllSessions();
            return View(Sessions);
        }

        public  IActionResult Create()
        {
            LoadCategoriesForDropDown();
            LoadTrainersForDropDown();
            return View();
        }
        [HttpPost]
        public  IActionResult Create(CreateSessionViewModel input)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataMissed", "Check Missing Data");
                LoadTrainersForDropDown();
                LoadCategoriesForDropDown();
                return View(nameof(Create));
            }
            var isCreated=_sessionService.CreateSession(input);
            if(isCreated)
            {
                TempData["SuccessMessage"] = "Session CreatedSuccessfully";
                return RedirectToAction(nameof(Index));

            }

            else
            {
                LoadTrainersForDropDown();
                LoadCategoriesForDropDown();
                ModelState.AddModelError("Failed", "Failed to create session");
                return View("Create",input);

            }
        }


        #region Helper methods

        public  void LoadCategoriesForDropDown()
        {
            var categories=_sessionService.GetCategoriesForDropDown();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
        }

        public  void LoadTrainersForDropDown()
        {
            var trainers = _sessionService.GetTrainersForDropDown();
            ViewBag.Trainers = new SelectList(trainers, "Id", "Name");
        }

        #endregion
    }
}
