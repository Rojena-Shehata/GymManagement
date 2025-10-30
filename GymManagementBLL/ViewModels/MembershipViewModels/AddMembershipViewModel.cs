using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.MembershipViewModels
{
    public class AddMembershipViewModel
    {
        [Required(ErrorMessage = "Please select a member.")]
        [Display(Name = "Member")]
        public int MemberId { get; set; }

        [Required(ErrorMessage = "Please select a plan.")]
        [Display(Name = "Member")]
        public int PlanId { get; set; }

        public DateTime StartDate { get; set; }= DateTime.UtcNow;
        public DateTime EndDate { get; set; }


    }
}
