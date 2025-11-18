using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities.Enums
{
    public enum Specialties
    {
        [Display(Name = "generalFitness")]
        GeneralFitness = 1,
        [Display(Name = "yoga")]
        Yoga,
        [Display(Name = "boxing")]
        Boxing,
        [Display(Name = "crossFit")]
        CrossFit
    }
}
