using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementBLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class TrainerController : Controller
    {
        private readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
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
                TempData["ErrorMessage"] = "Trainer id must be greater than 0";
                return RedirectToAction(nameof(Index));
            }
            var trainer=_trainerService.GetTrainerDetails(id);
            if(trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer not found";
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
                ModelState.AddModelError("DataMissed", "Check Missing Data");
                return View(nameof(Create), model);
            }
            var IsCreated = _trainerService.CreatTrainer(model);
            if (IsCreated)
            {

                TempData["SuccessMessage"] = "Trainer is Created, Successfully";
                return RedirectToAction(nameof(Index), model);
            }
            else
            {
                ModelState.AddModelError("EmailOrPhoneExistError", "Email or phone number already exist");
                return View(nameof(Create), model);
            }
        }

        public IActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Trainer id must be greater than 0";
                return RedirectToAction(nameof(Index));
            }
            var trainer=_trainerService.GetTrainerModelToUpdate(id);
            if(trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer not found";
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
                TempData["SuccessMessage"] = "Trainer Data Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Trainer failed to be Updated";
                return RedirectToAction(nameof(Index), model);
            }

        }

        public IActionResult Delete([FromRoute] int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Trainer id must be greater than 0";
                return RedirectToAction(nameof(Index));
            }
            var trainer=_trainerService.GetTrainerDetails(id);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
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
                TempData["ErrorMessage"] = "Trainer id must be greater than 0";
                return RedirectToAction(nameof(Index));
            }
            var isDeleted = _trainerService.RemoveTrainer(id);
            if (isDeleted)
                TempData["ErrorMessage"] = "Trainer Deleted Sucessfully";
            else
                TempData["ErrorMessage"] = "Trainer Faild To Be Deleted";

            return View(nameof(Index));
        }

       
    }
}
