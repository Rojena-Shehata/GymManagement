using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.BookingViewModels;
using GymManagementBLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public IActionResult Index()
        {
            var sessions = _bookingService.GetAllSessions();
            return View(sessions);
        }

        public IActionResult GetMembersForUpcomingSession([FromQuery]int sessionId)
        {
            if (sessionId <= 0)
            {
                return View();
            }
                
            var members=_bookingService.GetMembersForUpcomingSessions(sessionId);
            return View(members);
        }

        public IActionResult GetMembersForOngoingSessions([FromQuery]int sessionId)
        {
            if (sessionId <= 0)
            {
                return View();
            }
                
            var members=_bookingService.GetMembersForOngoingSession(sessionId);
            return View(members);
        }
        [HttpPost]
        public IActionResult Cancel(int sessionId, int memberId)
        {
            if (sessionId <= 0||memberId<=0)
            {
                TempData["ErrorMessage"] = "Id must be greater than 0";
                return RedirectToAction(nameof(GetMembersForUpcomingSession), new { sessionId = sessionId });
            }
            var isCanceled = _bookingService.Cancel(sessionId, memberId);
            if (isCanceled)
                TempData["SuccessMessage"] = " Successfully Canceled";
            else
                TempData["Error"] = "Failed to Cancel";

            return RedirectToAction(nameof(GetMembersForUpcomingSession), new { sessionId =sessionId });
        }
        public IActionResult CreateWithMultipleSessions()
        {
            LoadMembersForDropDown();
            LoadSessionsForDropDown();
           
            return View();
        }
        
        //[Route("Booking/Create/{sessionId}")]
        public IActionResult Create([FromQuery]int sessionId)
        {

            LoadMembersForDropDown();
            ViewBag.SessionId = sessionId;
            return View();
        }
        [HttpPost]
        public IActionResult Create( CreateBookingForSessionViewModel  input)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataMissed", "Check missing data");
                LoadMembersForDropDown();
                ViewBag.SessionId = input.SessionId;
                return View(nameof(Create),input);
            }
            var isCreated = _bookingService.Create(input);
            if (isCreated)
                TempData["SuccessMessage"] = "New Booking Added Successfullty";
            else
                TempData["ErrorMessage"]="Failed  To Add Booking";
            return RedirectToAction(nameof(GetMembersForUpcomingSession), new { sessionId = input.SessionId });
        }

        [HttpPost]
        public  IActionResult Attendance(int sessionId,int memberId)
        {
            if (sessionId <= 0 || memberId <= 0)
            {
                TempData["ErrorMessage"] = "Id must be greater than 0";
                return RedirectToAction(nameof(GetMembersForUpcomingSession), new { sessionId = sessionId });
            }
            var isCanceled = _bookingService.Attendance(sessionId, memberId);
            if (isCanceled)
                TempData["SuccessMessage"] = " Successfully Attendance Status Changed";
            else
                TempData["Error"] = "Failed To Change Attendance Status";
            return RedirectToAction(nameof(GetMembersForOngoingSessions), new { sessionId = sessionId });
        }



        #region Helper Methods Region

        private void LoadMembersForDropDown()
        {
            var members=_bookingService.GetMembersForDropDown();
            ViewBag.Members = new SelectList(members, "Id", "Name");
        }
        private void LoadSessionsForDropDown()
        {
            var sessions = _bookingService.GetSessionsToSelect();
            ViewBag.Sessions = new SelectList(sessions, "Id", "Name");
        }

        #endregion
    }
}
