using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.MembershipViewModels
{
    public class MembershipViewModel
    {
        public int Id { get; set; }

        //  Member info
        public int MemberId { get; set; }
        public string MemberName { get; set; }

        // Plan info
        public int PlanId { get; set; }
        public string PlanName { get; set; }

        // Dates
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Status
        public bool IsActive { get; set; }
    }
}
