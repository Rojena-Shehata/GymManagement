using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace GymManagementPL.Controllers
{
    [Authorize(Roles ="SuperAdmin")]
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;
        private readonly IStringLocalizer _stringLocalizer;

        public MemberController(IMemberService memberService,IStringLocalizer<MemberController> stringLocalizer)
        {
            _memberService = memberService;
            _stringLocalizer = stringLocalizer;
        }

        public IActionResult Index()
        {
            var members=_memberService.GetAllMembers();
            return View(members);
        }


        public IActionResult MemberDetails(int id)
        {
            var member=_memberService.GetMemberDetails(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = string.Format(_stringLocalizer["messages.notFound"], _stringLocalizer["member"]);
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        public IActionResult HealthRecordDetails(int id)
        {
            var member= _memberService.GetMemberHealthRecord(id);
            if (member is null)
            {
                TempData["ErrorMessage"]= string.Format(_stringLocalizer["messages.notFound"], _stringLocalizer["member"]);             
                    return RedirectToAction(nameof(Index));
            }
            return View(member);
        }
       
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateMemberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataMissed", _stringLocalizer["errors.dataMissing"]);
                return View(nameof(Create),model);
            }
            bool IsCreated = _memberService.createMember(model);
            if (IsCreated)
            {
                TempData["SuccessMessage"] = string.Format(_stringLocalizer["ActionSuccess"], _stringLocalizer["member"], _stringLocalizer["created"]);
                return RedirectToAction(nameof(Index), model);
            }
            else
            {
                ModelState.AddModelError("EmailOrPhoneExistError", _stringLocalizer["errors.emailOrPhoneExists"]);
                return View(nameof(Create), model);
            }
        }

        public IActionResult EditMember(int id)
        {
            var member = _memberService.GetMemberToUpdate(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = string.Format(_stringLocalizer["messages.notFound"], _stringLocalizer["member"]);
                return RedirectToAction(nameof(Index));
            }
            return View(nameof(EditMember),member);
        }
        [HttpPost]
        public IActionResult EditMember([FromRoute]int id,MemberToUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataMissed", _stringLocalizer["errors.dataMissing"]);

                return View(nameof(EditMember),model);
            }
            bool isMemberUpdated = _memberService.UpdateMemberData(id,model);
            if (isMemberUpdated)
            { 
                TempData["SuccessMessage"] = string.Format(_stringLocalizer["ActionSuccess"], _stringLocalizer["member"], _stringLocalizer["updated"]); ;
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("error", _stringLocalizer["errors.emailOrPhoneExists"]);
                return View(nameof(EditMember),model);
            }

        }

        [HttpGet]
        public IActionResult Delete([FromRoute]int id)
        {
            if(id<=0)
            {
                TempData["ErrorMessage"] = string.Format(_stringLocalizer["messages.invalidId"]);
                return RedirectToAction(nameof(Index));
            }
            var member=_memberService.GetMemberDetails(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = string.Format(_stringLocalizer["messages.notFound"], _stringLocalizer["member"]);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MembrId=member.Id;
            return View("Delete");
        }

        [HttpPost]
        public IActionResult DeleteConfirmed([FromForm]int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = string.Format(_stringLocalizer["messages.invalidId"]);
                return RedirectToAction(nameof(Index));
            }
            var isDeleted=_memberService.RemoveMember(id);
            if(! isDeleted)
                TempData["ErrorMessage"] = string.Format(_stringLocalizer["ActionError"], _stringLocalizer["member"], _stringLocalizer["deleted"]); 
            else
                TempData["SuccessMessage"] = string.Format(_stringLocalizer["ActionSuccess"], _stringLocalizer["member"], _stringLocalizer["deleted"]);
            return RedirectToAction(nameof(Index));
        }
    }
}
