using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementBLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace GymManagementPL.Controllers
{
    [Authorize(Roles ="SuperAdmin")]
    public class TrainerController : Controller
    {
        private readonly ITrainerService _trainerService;
        private readonly IStringLocalizer<TrainerController> _stringLocalizer;

        public TrainerController(ITrainerService trainerService, IStringLocalizer<TrainerController> stringLocalizer)
        {
            _trainerService = trainerService;
            _stringLocalizer = stringLocalizer;
        }

        public IActionResult Index()
        {
            var trainers=_trainerService.GetAllTrainers();
            
            return View(nameof(Index),trainers);
        }
        public IActionResult TrainerDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = string.Format(_stringLocalizer["messages.notFound"], _stringLocalizer["theTrainer"]);
                return RedirectToAction(nameof(Index));
            }
            var trainer=_trainerService.GetTrainerDetails(id);
            if(trainer is null)
            {
                TempData["ErrorMessage"] = string.Format(_stringLocalizer["messages.notFound"], _stringLocalizer["theTrainer"]);
                return RedirectToAction(nameof(Index));
            }
            return View(nameof(TrainerDetails), trainer);
        }

        public IActionResult Create()
        {
            return View("Create");
        }
        [HttpPost]
        public IActionResult Create(CreateTrainerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataMissed", _stringLocalizer["errors.dataMissing"]);
                return View(nameof(Create), model);
            }
            var IsCreated = _trainerService.CreatTrainer(model);
            if (IsCreated)
            {

                TempData["SuccessMessage"] = string.Format(_stringLocalizer["ActionSuccess"], _stringLocalizer["theTrainer"], _stringLocalizer["created"]);
                return RedirectToAction(nameof(Index), model);
            }
            else
            {
                ModelState.AddModelError("EmailOrPhoneExistError", _stringLocalizer["errors.emailOrPhoneExists"]);
                return View(nameof(Create), model);
            }
        }

        public IActionResult Edit(int id)
        {
            if (id <= 0)
            {
                ModelState.AddModelError("DataMissed", _stringLocalizer["errors.dataMissing"]);
                return RedirectToAction(nameof(Index));
            }
            var trainer=_trainerService.GetTrainerModelToUpdate(id);
            if(trainer is null)
            {
                ModelState.AddModelError("DataMissed", _stringLocalizer["errors.dataMissing"]);
                return RedirectToAction(nameof(Index));

            }
            return View(nameof(Edit), trainer);

        }
        [HttpPost]
        public IActionResult Edit([FromRoute] int id, TrainerToUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Edit), model);
            }

            var IsUpdated = _trainerService.UpdateTrainerData(id, model);
            if (IsUpdated)
            {
                TempData["SuccessMessage"] = string.Format(_stringLocalizer["ActionSuccess"], _stringLocalizer["theTrainer"], _stringLocalizer["updated"]); ;
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = string.Format(_stringLocalizer["ActionError"], _stringLocalizer["theTrainer"], _stringLocalizer["updated"]);
                return RedirectToAction(nameof(Index), model);
            }

        }

        public IActionResult Delete([FromRoute] int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = string.Format(_stringLocalizer["messages.invalidId"]);
                return RedirectToAction(nameof(Index));
            }
            var trainer=_trainerService.GetTrainerDetails(id);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = string.Format(_stringLocalizer["messages.notFound"], _stringLocalizer["theTrainer"]);
                return RedirectToAction(nameof(Index));

            }
            ViewBag.TrainerId = trainer.Id;
            return View(nameof(Delete));
        }
        [HttpPost]
       public IActionResult DeleteConfirmed([FromForm]int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = string.Format(_stringLocalizer["messages.invalidId"]);
                return RedirectToAction(nameof(Index));
            }
            var isDeleted = _trainerService.RemoveTrainer(id);
            if (isDeleted)
                TempData["SuccessMessage"] = string.Format(_stringLocalizer["ActionSuccess"], _stringLocalizer["theTrainer"], _stringLocalizer["deleted"]);
            else
                TempData["ErrorMessage"] = string.Format(_stringLocalizer["ActionError"], _stringLocalizer["theTrainer"], _stringLocalizer["deleted"]);
            

            return RedirectToAction(nameof(Index));
        }

       
    }
}
