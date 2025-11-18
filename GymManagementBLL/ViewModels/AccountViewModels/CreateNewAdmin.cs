using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.AccountViewModels
{
    public class CreateNewAdmin
    {

        [Required(ErrorMessage = "Name is Required")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters and spaces is valid for name. ")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Userame is Required")]
        [RegularExpression(@"^[A-Za-z0-9_-]+$", ErrorMessage = "Username can contain letters, numbers, underscores (_) and dashes (-) only.")]
        public string Username { get; set; } = null!;
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        [MinLength( 6, ErrorMessage = "The password length must be at least 6 letters")]
        [Compare("ConfirmPassword", ErrorMessage = "Password does not match")]
        public string Password { get; set; } = null!;
        [Required(ErrorMessage = "Confirm Password is Required")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
