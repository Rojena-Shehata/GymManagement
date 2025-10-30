using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    [Authorize]
    public class PlanController : Controller
    {
        private readonly IPlanService _planService;

        public PlanController(IPlanService planService)
        {
            _planService = planService;
        }

        public IActionResult Index()
        {
            var plans=_planService.GetAllPlans();
           
            return View(plans);
        }

        public IActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id must be greater than 0";
                return RedirectToAction(nameof(Index));
            }
            var plan=_planService.GetPlanById(id);
            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan not found";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }

        public IActionResult Edit(int id)
        {

            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id must be greater than 0";
                return RedirectToAction(nameof(Index));
            }
            var plan = _planService.GetPlanToUpdate(id);
            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan not found";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }
        [HttpPost]
        public IActionResult Edit(int id,UpdatePlanViewModel input)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id must be greater than 0";
                return RedirectToAction(nameof(Index));
            }
            if(!ModelState.IsValid )
            {
                ModelState.AddModelError("WrongData", "Check wrong data");
                return View(nameof(Edit), input);

            }
            var isUpdated = _planService.UpdatePlan(id, input);
            if (isUpdated)
                TempData["SuccessMessage"] = "Plan is Updated successfully";
            else
                TempData["ErrorMessage"] = "Plan failed to be updated";
            
                return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Activate([FromRoute]int id)
        {
            if(id <= 0)
            {
                TempData["ErrorMessage"] = "Id must be greater than 0";
                return RedirectToAction(nameof(Index));
            }
            var IsActivated=_planService.Activate(id);
            if (IsActivated)
                TempData["SuccessMessage"] = "Plan Status Changed Successfullty";
            else
                TempData["ErrorMessage"] = "Plan  Status failed to be Changed";

            return RedirectToAction(nameof(Index));
            
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreatePlanViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataMissed", "Check Missing Data");
                return View(nameof(Create),model);
            }
            var isCreated= _planService.CreatePlan(model);
            if (isCreated)           
                TempData["SuccessMessage"] = "Plan Created  Successfully";
            
            else           
                TempData["ErrorMessage"] = "Plan Failed to  be  Created";


            return RedirectToAction(nameof(Index));

        }

    }
}
