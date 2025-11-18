using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MembershipViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;

namespace GymManagementPL.Controllers
{
    public class MembershipController : Controller
    {
        private readonly IMembershipService _membershipService;
        private readonly IStringLocalizer<MembershipController> _stringLocalizer;

        public MembershipController(IMembershipService membershipService,IStringLocalizer<MembershipController>stringLocalizer)
        {
            _membershipService = membershipService;
            _stringLocalizer = stringLocalizer;
        }
        public IActionResult Index()
        {
            var memberships = _membershipService.GetAll();
            return View(memberships);
        }
        public IActionResult Create()
        {
            LoadMembersForDroupDown();
            LoadPlansForDroupDown();
            return View();
        }
        [HttpPost]
        public IActionResult Create(AddMembershipViewModel  input )
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = _stringLocalizer["errors.dataMissing"].Value;
                LoadMembersForDroupDown();
                LoadPlansForDroupDown();

                return View(nameof(Create),input);
            }
            var isCreated=_membershipService.Create(input);
            if (!isCreated)

                TempData["ErrorMessage"] = string.Format(_stringLocalizer["ActionError"], _stringLocalizer["membership"], _stringLocalizer["created"]);
            else
                TempData["SuccessMessage"] = string.Format(_stringLocalizer["ActionSuccess"], _stringLocalizer["membership"], _stringLocalizer["created"]);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Cancel(int memberId)
        {
            if (memberId <= 0)
            {
                TempData["ErrorMessage"] = _stringLocalizer["messages.invalidId"].Value;
                return View();
            }
            var isDeleted = _membershipService.Remove(memberId);
            if (isDeleted)
                TempData["SuccessMessage"] = _stringLocalizer["membershipCanceledSuccess"].Value;
            else
                TempData["ErrorMessage"] = _stringLocalizer["membershipCanceledError"].Value;

            return RedirectToAction(nameof(Index));
        }



        #region Helper Methods

        void LoadMembersForDroupDown()
        {
           var members= _membershipService.GetMembersForDropDown();
            ViewBag.Members = new SelectList(members, "Id", "Name");
        }

        void LoadPlansForDroupDown()
        {
           var plans= _membershipService.GetPlansForDropDown();
            ViewBag.Plans = new SelectList(plans, "Id", "Name");
        }
        #endregion
    }
}
