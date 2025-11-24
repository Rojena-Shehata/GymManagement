using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;

namespace GymManagementPL.Controllers
{
    [Authorize]
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;
        private readonly IStringLocalizer _stringLocalizer;

        public SessionController(ISessionService sessionService, IStringLocalizer<SessionController> stringLocalizer)
        {
            _sessionService = sessionService;
            _stringLocalizer = stringLocalizer;
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
                ModelState.AddModelError("DataMissed", _stringLocalizer["errors.dataMissing"].Value);
                LoadTrainersForDropDown();
                LoadCategoriesForDropDown();
                return View(nameof(Create));
            }
            var isCreated=_sessionService.CreateSession(input);
            if(isCreated)
            {
                TempData["SuccessMessage"] = string.Format(_stringLocalizer["ActionSuccess"], _stringLocalizer["session"], _stringLocalizer["created"]);
                return RedirectToAction(nameof(Index));

            }

            else
            {
                LoadTrainersForDropDown();
                LoadCategoriesForDropDown();
                ModelState.AddModelError("Failed", string.Format(_stringLocalizer["ActionSuccess"], _stringLocalizer["session"], _stringLocalizer["created"]));

                return View("Create",input);

            }
        }



        public  IActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = string.Format(_stringLocalizer["messages.invalidId"]);
                return RedirectToAction(nameof(Index));
            }
            var   session =_sessionService.GetSessionToUpdate(id);
            if(session is null)
            {
                TempData["ErrorMessage"] = string.Format(_stringLocalizer["messages.notFound"], _stringLocalizer["session"]);
                return RedirectToAction(nameof(Index));

            }
            LoadTrainersForDropDown();
            return View("Edit", session);
        }

        [HttpPost]
        public IActionResult Edit(int id, UpdateSessionViewModel input)
        {
            if (id <= 0)
            {

                TempData["ErrorMessage"] = string.Format(_stringLocalizer["messages.invalidId"].Value);
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {

                ModelState.AddModelError("Failed", string.Format(_stringLocalizer["ActionError"], _stringLocalizer["session"], _stringLocalizer["updated"]));

                return View(nameof(Edit));
            }
            var isUpdated=_sessionService.UpdateSession(id,input);
            if (isUpdated)
            {
                TempData["SuccessMessage"] = string.Format(_stringLocalizer["ActionSuccess"], _stringLocalizer["session"], _stringLocalizer["updated"]); ;

            }
            else
                TempData["ErrorMessage"] = string.Format(_stringLocalizer["ActionError"], _stringLocalizer["session"], _stringLocalizer["updated"]); ;

            return RedirectToAction(nameof(Index));


        }

        public IActionResult Details(int id)
        {
            if(id <= 0)
            {
                TempData["ErrorMessage"] = string.Format(_stringLocalizer["messages.invalidId"]);
                return RedirectToAction(nameof(Index));
            }
            var  session=_sessionService.GetSessionById(id);
            if(session is null)
            {
                TempData["ErrorMessage"] = string.Format(_stringLocalizer["messages.notFound"], _stringLocalizer["session"]);
                return RedirectToAction(nameof(Index));
            }

                return View(session);
        }

        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = string.Format(_stringLocalizer["messages.invalidId"]);
                return RedirectToAction(nameof(Index));
            }

            var session = _sessionService.GetSessionById(id);
            if (session is null)
            {
                TempData["ErrorMessage"] = string.Format(_stringLocalizer["messages.notFound"], _stringLocalizer["session"]);
                return RedirectToAction(nameof(Index));

            }
            ViewBag.SessionId = session.Id;
            return View(nameof(Delete));
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {

            if (id <= 0)
            {
                TempData["ErrorMessage"] = string.Format(_stringLocalizer["messages.invalidId"]);
                return RedirectToAction(nameof(Index));
            }
            var isDeleted = _sessionService.RemoveSession(id);
            if (isDeleted)
                TempData["SuccessMessage"] = string.Format(_stringLocalizer["ActionSuccess"], _stringLocalizer["session"], _stringLocalizer["deleted"]);
            else
                TempData["ErrorMessage"] = string.Format(_stringLocalizer["ActionError"], _stringLocalizer["session"], _stringLocalizer["deleted"]);
            return RedirectToAction(nameof(Index));
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
