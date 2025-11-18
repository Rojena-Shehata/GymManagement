using GymManagementDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class Trainer:GymUser
    {
        //public DateOnly HireDate { get; set; }
        public Specialties Specialties { get; set; }
        public ICollection<Session> Sessions { get; set; } = null;

    }
}
