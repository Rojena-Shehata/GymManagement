using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities.Enums
{
    public enum Gender
    {
        [Display(Name = "male")]
        Male,
        [Display(Name = "female")]
        Female
    }
}
