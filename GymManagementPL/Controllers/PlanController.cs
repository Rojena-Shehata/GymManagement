using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace GymManagementPL.Controllers
{
    [Authorize]
    public class PlanController : Controller
    {
        private readonly IPlanService _planService;
        private readonly IStringLocalizer<PlanController> _stringLocalizer;

        public PlanController(IPlanService planService, IStringLocalizer<PlanController> stringLocalizer)
        {
            _planService = planService;
            _stringLocalizer = stringLocalizer;
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
                TempData["ErrorMessage"] = string.Format(_stringLocalizer["messages.invalidId"]);
                return RedirectToAction(nameof(Index));
            }
            var plan=_planService.GetPlanById(id);
            if (plan is null)
            {
                TempData["ErrorMessage"] = string.Format(_stringLocalizer["messages.notFound"], _stringLocalizer["plan"]);
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }

        public IActionResult Edit(int id)
        {

            if (id <= 0)
            {
                TempData["ErrorMessage"] = string.Format(_stringLocalizer["messages.invalidId"]);
                return RedirectToAction(nameof(Index));
            }
            var plan = _planService.GetPlanToUpdate(id);
            if (plan is null)
            {
                TempData["ErrorMessage"] = string.Format(_stringLocalizer["messages.notFound"], _stringLocalizer["plan"]);
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }
        [HttpPost]
        public IActionResult Edit(int id,UpdatePlanViewModel input)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = string.Format(_stringLocalizer["messages.invalidId"]);
                return RedirectToAction(nameof(Index));
            }
            if(!ModelState.IsValid )
            {
                ModelState.AddModelError("DataMissed", _stringLocalizer["errors.dataMissing"]);
                return View(nameof(Edit), input);

            }
            var isUpdated = _planService.UpdatePlan(id, input);
            if (isUpdated)
                TempData["SuccessMessage"] = string.Format(_stringLocalizer["ActionSuccess"], _stringLocalizer["session"], _stringLocalizer["updated"]); 
            else
                TempData["ErrorMessage"] = string.Format(_stringLocalizer["ActionError"], _stringLocalizer["session"], _stringLocalizer["updated"]); ;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Activate([FromRoute]int id)
        {
            if(id <= 0)
            {
                TempData["ErrorMessage"] = string.Format(_stringLocalizer["messages.invalidId"]);
                return RedirectToAction(nameof(Index));
            }
            var IsActivated=_planService.Activate(id);
            if (IsActivated)
                TempData["SuccessMessage"] = _stringLocalizer["successMessagePlanStatus"];
            else
                TempData["ErrorMessage"] = _stringLocalizer["errorMessagePlanStatus"];

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
                ModelState.AddModelError("DataMissed", _stringLocalizer["errors.dataMissing"]);
                return View(nameof(Create),model);
            }
            var isCreated= _planService.CreatePlan(model);
            if (isCreated)
                TempData["SuccessMessage"] = string.Format(_stringLocalizer["ActionSuccess"], _stringLocalizer["plan"], _stringLocalizer["created"]);

            else
                TempData["ErrorMessage"] = string.Format(_stringLocalizer["ActionError"], _stringLocalizer["plan"], _stringLocalizer["created"]);


            return RedirectToAction(nameof(Index));

        }

    }
}
