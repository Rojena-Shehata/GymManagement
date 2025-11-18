using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.MemberViewModels
{
    public class HealthRecordViewModel
    {
        [Display(Name = "height"),Required(ErrorMessage ="required")]
        [Range(0.1,300,ErrorMessage = "range", ConvertValueInInvariantCulture=true)]
        public decimal Height { get; set; }
        [Display(Name = "weight"), Required(ErrorMessage = "required")]
        [Range(0.1,500,ErrorMessage = "range", ConvertValueInInvariantCulture=true)]
        public decimal Weight { get; set; }
        [Display(Name = "bloodType"), Required(ErrorMessage ="required")]
        [StringLength(3,ErrorMessage = "stringLengthMax")]
        public string BloodType { get; set; } = null!;
        public string? Note { get; set; }
    }
}
