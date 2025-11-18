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
        [Display(Name = "member"), Required(ErrorMessage = "required")]
        public int MemberId { get; set; }

        [Display(Name = "plan"), Required(ErrorMessage = "required")]
        public int PlanId { get; set; }
        public DateTime StartDate { get; set; }= DateTime.UtcNow;
        public DateTime EndDate { get; set; }


    }
}
