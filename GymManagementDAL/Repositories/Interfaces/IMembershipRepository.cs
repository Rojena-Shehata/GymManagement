using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface IMembershipRepository
    {

        MemberShip? GetById(int memberId, int planId);
        IEnumerable<MemberShip> GetAll();
        int Add(MemberShip planMember);
        int Update(MemberShip planMember);
        int Delete(int memberId, int planId);
    }
}
