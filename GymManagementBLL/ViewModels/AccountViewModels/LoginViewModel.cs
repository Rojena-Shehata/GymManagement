using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.AccountViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "email"), Required(ErrorMessage = "required")]
        public string Email { get; set; } = null!;
        [Display(Name = "password"), Required(ErrorMessage = "required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }=null!;
        public bool RememberMe { get; set; }
        
    }
}
