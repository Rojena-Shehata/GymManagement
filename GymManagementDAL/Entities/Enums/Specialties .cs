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
        [Display(Name ="General Fitness")]
        GeneralFitness = 1,
        Yoga,
        Boxing,
        CrossFit
    }
}
