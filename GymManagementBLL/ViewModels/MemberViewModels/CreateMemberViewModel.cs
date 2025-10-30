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
        [Required(ErrorMessage ="Name Is Required.")]
        [RegularExpression(@"^[A-Za-z\s]+$",ErrorMessage = "Only English letters and spaces are allowed.")]
        [StringLength(50,ErrorMessage = "Name cannot exceed 50 characters.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage ="Email is Required.")]
        [EmailAddress(ErrorMessage ="Invalid Email Address!")]
        [RegularExpression(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$",
        ErrorMessage = "That doesn’t look like a valid email. Please use the format name@example.com.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Phone Number is required")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$",ErrorMessage ="Phone Number must be a valid Egyptian Number")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = null!;
        [Required(ErrorMessage ="Date Of Birth is Required")]
        [DataType(DataType.Date,ErrorMessage ="Invalid date!")]
        public DateOnly DateOfBirth { get; set; }

        [Required(ErrorMessage ="Gender Is required")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage ="Building Number is Required")]
        [Range(1,int.MaxValue,ErrorMessage ="Building Number must be greater than 0")]
        public int BuildingNumber { get; set; }
        [Required(ErrorMessage ="Street is required")]
        [StringLength(150,MinimumLength = 2,ErrorMessage ="street must be between 2 and 150 characters")]
        [RegularExpression(@"^[A-Za-z0-9\s]+$",ErrorMessage ="Street Must Only Contain Spaces, Characters and Numbers")]
        public string Street { get; set; } = null!;
        [Required(ErrorMessage = "City is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "City must be between 2 and 100 characters")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "City Must Only Contain Spaces and Characters")]
        public string City { get; set; } = null!;
        [Required(ErrorMessage ="Health record is required")]
        public HealthRecordViewModel HealthRecord { get; set; }= null!;


    }
}
