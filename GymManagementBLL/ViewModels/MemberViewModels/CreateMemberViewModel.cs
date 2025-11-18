using GymManagementDAL.Entities.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace GymManagementBLL.ViewModels.MemberViewModels
{
    public class CreateMemberViewModel
    {
       // [Required(ErrorMessage ="Profile Photo is Required")]
        [Display(Name="Profile Photo")]
        public IFormFile? Photo { get; set; }
        [Display(Name = "name"), Required(ErrorMessage = "required")]

        [RegularExpression(@"^([A-Za-z\s]+|[\u0600-\u06FF\s]+)$", ErrorMessage = "nameRegex")]
        [StringLength(50,ErrorMessage = "stringLengthMax")]
        public string Name { get; set; } = null!;

        [Display(Name = "email"), Required(ErrorMessage = "required")]
        [EmailAddress(ErrorMessage = "invalidEmail")]
        [RegularExpression(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$",
        ErrorMessage = "regexEmail")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
        [Display(Name="phone"),Required(ErrorMessage = "required")]
        [Phone(ErrorMessage = "invalidPhone")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$",ErrorMessage = "regexPhone")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = null!;
        [Display(Name = "dateOfBirth"), Required(ErrorMessage ="required")]
        [DataType(DataType.Date,ErrorMessage = "regex")]
        public DateOnly DateOfBirth { get; set; }

        [Display(Name = "gender"),Required(ErrorMessage ="required")]
        public Gender Gender { get; set; }

        [Display(Name = "buildingNumber"),Required(ErrorMessage ="required")]
        [Range(1,int.MaxValue,ErrorMessage = "range",ConvertValueInInvariantCulture =true)]
        public int BuildingNumber { get; set; }
        [Display(Name = "street"),Required(ErrorMessage ="required")]
        [StringLength(150,MinimumLength = 2,ErrorMessage = "stringLength")]
        [RegularExpression(@"^[A-Za-z0-9\s]+$",ErrorMessage = "regexStreet")]
        public string Street { get; set; } = null!;
        [Display(Name = "city"), Required(ErrorMessage = "required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "stringLength")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "regexCity")]
        public string City { get; set; } = null!;
        [Display(Name = "healthRecord"), Required(ErrorMessage = "required")]
        public HealthRecordViewModel HealthRecord { get; set; }= null!;


    }
}
