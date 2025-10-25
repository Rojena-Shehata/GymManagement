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
        [Required(ErrorMessage ="Name is required")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only English letters and spaces are allowed.")]
        [MaxLength(50,ErrorMessage ="Name length can't exceed 50 characters")]
        public string PlanName { get; set; } = null!;
        [Required(ErrorMessage ="Description is Required")]
        [MaxLength(200, ErrorMessage = "Name length can't exceed 200 characters")]
        public string Description { get; set; } = null!;
        [Required(ErrorMessage = "Duration days is Required")]
        [Range(1,365,ErrorMessage ="Please, enter days between 1 and 365")]
        public int DurationDays { get; set; }
        [Required(ErrorMessage ="Price is required")]      
        public decimal Price { get; set; }
        
        public bool IsActive =>true;
    }
}
