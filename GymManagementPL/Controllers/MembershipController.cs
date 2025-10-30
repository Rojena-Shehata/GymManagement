using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MembershipViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    public class MembershipController : Controller
    {
        private readonly IMembershipService _membershipService;

        public MembershipController(IMembershipService membershipService)
        {
            _membershipService = membershipService;
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
                ViewBag.Errors = "Check  missed  Data";
                LoadMembersForDroupDown();
                LoadPlansForDroupDown();

                return View(nameof(Create),input);
            }
            var isCreated=_membershipService.Create(input);
            if (!isCreated)

                TempData["ErrorMessage"] = "Failed   to  add membership";
            else
                TempData["SuccessMessage"] = "Membership created successfully";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Cancel(int memberId)
        {
            if (memberId <= 0)
            {
                TempData["ErrorMessage"] = "id must be greater than 0";
                return View();
            }
            var isDeleted = _membershipService.Remove(memberId);
            if (isDeleted)
                TempData["SuccessMessage"] = "Membership Successfully Canceled";
            else
                TempData["ErrorMessage"] = "Membership failed to be canceled";
            return View(nameof(Index));

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
