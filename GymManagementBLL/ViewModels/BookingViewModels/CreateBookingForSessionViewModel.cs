using GymManagementBLL.ViewModels.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.BookingViewModels
{
    public class CreateBookingForSessionViewModel
    {
        [Display(Name = "member"), Required(ErrorMessage = "required")]
        public int memberId { get; set; }
        [Display(Name = "session"), Required(ErrorMessage = "required")]
        public int SessionId { get; set; }
        [Display(Name = "phone"), Required(ErrorMessage = "required")]
        [Phone(ErrorMessage = "invalidPhone")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "regexPhone")]
        [DataType(DataType.PhoneNumber)]
        public string MemberPhone { get; set; } = null!;
    }
}
