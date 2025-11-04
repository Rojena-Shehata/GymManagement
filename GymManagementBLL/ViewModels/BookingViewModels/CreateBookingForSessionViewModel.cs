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
        [Required(ErrorMessage = "Member is Required")]
        [Display(Name = "Member")]
        public int memberId { get; set; }
        [Required(ErrorMessage = "Session is Required")]
        [Display(Name = "UpComing Sessions")]
        public int SessionId { get; set; } 
        [Required(ErrorMessage = "Phone Number is required")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Phone Number must be a valid Egyptian Number")]
        [DataType(DataType.PhoneNumber)]
        public string Memberphone { get; set; }
    }
}
