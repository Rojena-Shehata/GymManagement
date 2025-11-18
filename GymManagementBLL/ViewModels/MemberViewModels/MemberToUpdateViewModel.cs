using GymManagementDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.MemberViewModels
{
    public class MemberToUpdateViewModel
    {
        public string Name { get; set; } = null!;
        public string? Photo { get; set; }

        [Display(Name = "email"), Required(ErrorMessage = "required")]
        [EmailAddress(ErrorMessage = "invalidEmail")]
        [RegularExpression(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$",
                                                ErrorMessage = "regexEmail")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
        [Display(Name = "phone"), Required(ErrorMessage = "required")]
        [Phone(ErrorMessage = "invalidPhone")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "regexPhone")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = null!;

        [Display(Name = "dateOfBirth"), Required(ErrorMessage ="required")]
        [DataType(DataType.Date,ErrorMessage = "regex")]
        public DateOnly DateOfBirth { get; set; }
        [Display(Name = "gender"), Required(ErrorMessage = "required")]
        public Gender Gender { get; set; }
        [Display(Name = "buildingNumber"), Required(ErrorMessage = "required")]
        [Range(1, int.MaxValue, ErrorMessage = "range", ConvertValueInInvariantCulture = true)]
        public int BuildingNumber { get; set; }
        [Display(Name = "city"), Required(ErrorMessage = "required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "stringLength")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "regexCity")]
        public string Street { get; set; } = null!;
        [Required(ErrorMessage = "City is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "City must be between 2 and 100 characters")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "City Must Only Contain Spaces and Characters")]
        public string City { get; set; } = null!;

    }
}
