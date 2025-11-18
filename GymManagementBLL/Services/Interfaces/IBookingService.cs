using GymManagementBLL.ViewModels;
using GymManagementBLL.ViewModels.BookingViewModels;
using GymManagementBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IBookingService
    {
        bool Create(CreateBookingForSessionViewModel input);
        IEnumerable<SessionViewModel> GetAllSessions();
        IEnumerable<MembersForOngoingSessionViewModel> GetMembersForOngoingSession(int sessionId);
        bool Attendance(int sessionId,int memberId);

        IEnumerable<MembersForUpcomigSessionViewModel> GetMembersForUpcomingSessions(int sessionId);
        bool Cancel(int sessionId, int memberId);

         bool CreateMemberWithMultipleSeesion(CreateMultipleBookingViewModel input);
        IEnumerable<IdNameViewModelForDropDown> GetSessionsToSelect();
        IEnumerable<IdNameViewModelForDropDown> GetMembersForDropDown();
        
    }
}
