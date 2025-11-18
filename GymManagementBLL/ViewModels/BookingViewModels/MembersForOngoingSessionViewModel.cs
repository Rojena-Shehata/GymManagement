using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.BookingViewModels
{
    public class MembersForOngoingSessionViewModel
    {

        public string MemberName { get; set; } = default!;
        public string BookingDate { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public int MemberId { get; set; }
        public int SessionId { get; set; }
        public bool IsAttended { get; set; }
    }
}
