using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.PlanViewModels
{
    public class CreatePlanViewModel
    {
        [Display(Name = "planName"), Required(ErrorMessage = "required")]
        //[RegularExpression(@"^([A-Za-z\s]+|[\u0600-\u06FF\s]+)$", ErrorMessage = "nameRegex")]
        [StringLength(50, ErrorMessage = "stringLengthMax")]
        public string PlanName { get; set; } = null!;
        [Display(Name = "description"), Required(ErrorMessage = "required")]
        [StringLength(200, ErrorMessage = "stringLengthMax")]
        public string Description { get; set; } = null!;
        [Display(Name = "durationDays"), Required(ErrorMessage = "required")]
        [Range(1,365, ErrorMessage = "range", ConvertValueInInvariantCulture = true)]
        public int DurationDays { get; set; }
        [Display(Name = "price"), Required(ErrorMessage = "required")]
        public decimal Price { get; set; }
        
        public bool IsActive =>true;
    }
}
