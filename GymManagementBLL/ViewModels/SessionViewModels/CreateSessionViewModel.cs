using System.ComponentModel.DataAnnotations;

namespace GymManagementBLL.ViewModels.SessionViewModels
{
	public class CreateSessionViewModel
	{
        [Display(Name = "description"), Required(ErrorMessage = "required")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "stringLength")]
        public string Description { get; set; } = null!;
        [Display(Name = "capacity"), Required(ErrorMessage = "required")]
        [Range(0, 25, ErrorMessage = "range", ConvertValueInInvariantCulture = true)]
        public int Capacity { get; set; }

        [Display(Name = "startDateTime"), Required(ErrorMessage = "required")]
		public DateTime StartDate { get; set; }

        [Display(Name = "endDateTime"), Required(ErrorMessage = "required")]
		public DateTime EndDate { get; set; }
        [Display(Name = "theTrainer"), Required(ErrorMessage = "required")]
        public int TrainerId { get; set; }
        [Display(Name = "category"), Required(ErrorMessage = "required")]
        public int CategoryId { get; set; }
	}
}
