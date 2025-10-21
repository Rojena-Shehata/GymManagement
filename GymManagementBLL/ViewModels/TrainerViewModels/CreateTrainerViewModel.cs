using GymManagementDAL.Entities;
using GymManagementDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.TrainerViewModels
{
    public class CreateTrainerViewModel
    {
        [Required(ErrorMessage ="Name Is Required")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only English letters and spaces are allowed.")]
        [StringLength(50,ErrorMessage ="Name mustn't Exceed 50 Characters")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Specializations Section Is Required")]
        public Specialties Specialization { get; set; }

        [Required(ErrorMessage ="Email Is Required")]
        [EmailAddress(ErrorMessage ="Invalid Email Address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage ="Phone Number Is Required")]
        [Phone(ErrorMessage ="Invalid Phone Number")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Phone Number must be a valid Egyptian Number")]
        public string Phone { get; set; } = null!;

        [DataType(DataType.Date,ErrorMessage ="Invalid Date")]
        public DateOnly DateOfBirth { get; set; }

        [Required(ErrorMessage ="Gender is Required")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Building Number is Required")]
        [Range(1, int.MaxValue, ErrorMessage = "Building Number must be greater than 0")]
        public int BuildingNumber { get; set; }

        [Required(ErrorMessage = "Street is required")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "street must be between 2 and 150 characters")]
        [RegularExpression(@"^[A-Za-z0-9\s]+$", ErrorMessage = "Street Must Only Contain Spaces, Characters and Numbers")]
        public string Street { get; set; } = null!;

        [Required(ErrorMessage = "City is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "City must be between 2 and 100 characters")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "City Must Only Contain Characters and Spaces")]
        public string City { get; set; } = null!;
    }
}
