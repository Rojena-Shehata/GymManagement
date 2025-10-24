using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
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
                TempData["ErrorMessage"] = "Member is not found";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        public IActionResult HealthRecordDetails(int id)
        {
            var member= _memberService.GetMemberHealthRecord(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member is not found";
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
                ModelState.AddModelError("DataMissed", "Check Missing Data");
                return View(nameof(Create),model);
            }
            bool IsCreated = _memberService.createMember(model);
            if (IsCreated)
            {
                TempData["SuccessMessage"] = "Member is Created, Successfully";
                return RedirectToAction(nameof(Index), model);
            }
            else
            {
                ModelState.AddModelError("EmailOrPhoneExistError", "Email or phone number already exist");
                return View(nameof(Create), model);
            }
        }

        public IActionResult EditMember(int id)
        {
            var member = _memberService.GetMemberToUpdate(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member is Not Found";
                return RedirectToAction(nameof(EditMember));
            }
            return View(nameof(EditMember),member);
        }
        [HttpPost]
        public IActionResult EditMember([FromRoute]int id,MemberToUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(EditMember),model);
            }
            bool isMemberUpdated = _memberService.UpdateMemberData(id,model);
            if (isMemberUpdated)
            { 
                TempData["SuccessMessage"] = "Member Data Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Member failed to be Updated";
                return RedirectToAction(nameof(EditMember),model);
            }

        }

        [HttpGet]
        public IActionResult Delete([FromRoute]int id)
        {
            if(id<=0)
            {
                TempData["ErrorMessage"] = "Member id must be greater than 0";
                return RedirectToAction(nameof(Index));
            }
            var member=_memberService.GetMemberDetails(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member not found";
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
                TempData["ErrorMessage"] = "Member id must be greater than 0";
                return RedirectToAction(nameof(Index));
            }
            var isDeleted=_memberService.RemoveMember(id);
            if(! isDeleted)
                TempData["ErrorMessage"] = "Member can't deleted";
            else
                TempData["SuccessMessage"] = "Member deleted Successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
