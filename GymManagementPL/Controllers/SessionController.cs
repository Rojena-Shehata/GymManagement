using GymManagementBLL.Services.Classes;
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
                TempData["SuccessMessage"] = "Session Created Successfully";
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



        public  IActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "id must be greater thann 0";
                return RedirectToAction(nameof(Index));
            }
            var   session =_sessionService.GetSessionToUpdate(id);
            if(session is null)
            {
                TempData["ErrorMessage"] = "Session not found";
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

                TempData["ErrorMessage"] = "Trainer id must be greater than 0";
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {

                ModelState.AddModelError("Failed", "Session failed to be Updated");
                return View(nameof(Edit));
            }
            var isUpdated=_sessionService.UpdateSession(id,input);
            if (isUpdated)
            {
                TempData["SuccessMessage"] = "Session Updated Successfully";

            }
            else
                TempData["Errormessage"] = "Session failed to be Updated";

            return RedirectToAction(nameof(Index));


        }

        public IActionResult Details(int id)
        {
            if(id <= 0)
            {
                TempData["ErrprMessage"] = "Id must  ne greater than 0";
                return RedirectToAction(nameof(Index));
            }
            var  session=_sessionService.GetSessionById(id);
            if(session is null)
            {
                TempData["ErrorMessage"] = "Session not Found";
                return RedirectToAction(nameof(Index));
            }

                return View(session);
        }

        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "id must be greater than 0";
                return RedirectToAction(nameof(Index));
            }

            var session = _sessionService.GetSessionById(id);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));

            }
            ViewBag.SessionId = session.Id;
            return View(nameof(Delete));
        }

        public IActionResult DeleteConfirmed(int id)
        {

            if (id <= 0)
            {
                TempData["ErrorMessage"] = "id must be greater than 0";
                return RedirectToAction(nameof(Index));
            }
            var isDeleted = _sessionService.RemoveSession(id);
            if (isDeleted)
                TempData["SuccessMessage"] = "Session Deleted Successfully";
            else
                TempData["ErrorMessage"] = "Session failed to be deleted";
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
