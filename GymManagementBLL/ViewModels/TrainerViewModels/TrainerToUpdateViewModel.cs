using GymManagementDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.TrainerViewModels
{
    public class TrainerToUpdateViewModel
    {
        public string Name { get; set; } = null!;
        [Display(Name = "specialization"), Required(ErrorMessage = "required")]
        public Specialties Specialization { get; set; }

        [Display(Name = "email"), Required(ErrorMessage = "required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Display(Name = "phone"), Required(ErrorMessage = "required")]
        [Phone(ErrorMessage = "invalidPhone")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "regexPhone")]
        public string Phone { get; set; } = null!;


        [Display(Name = "buildingNumber"), Required(ErrorMessage = "required")]
        [Range(1, int.MaxValue, ErrorMessage = "range", ConvertValueInInvariantCulture = true)]
        public int BuildingNumber { get; set; }

        [Display(Name = "street"), Required(ErrorMessage = "required")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "stringLength")]
        [RegularExpression(@"^[A-Za-z0-9\s]+$", ErrorMessage = "regexStreet")]
        public string Street { get; set; } = null!;

        [Display(Name = "city"), Required(ErrorMessage = "required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "stringLength")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "regexCity")]
        public string City { get; set; } = null!;
    }
}
