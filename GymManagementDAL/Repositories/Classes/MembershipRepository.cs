using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
    public class MembershipRepository:IMembershipRepository
    {
        private readonly GymDbContext _context;

        public MembershipRepository(GymDbContext context)
        {
            _context = context;
        }

        public int Add(MemberShip planMember)
        {
            if (planMember is null)
                return 0;
            _context.Add(planMember);
            return _context.SaveChanges();
        }

        public int Delete(int memberId, int planId)
        {
            MemberShip planMember =GetById(memberId, planId);
            if (planMember is null) return 0;

            _context.Remove(planMember);
            return _context.SaveChanges();
        }

        public IEnumerable<MemberShip> GetAll()=>_context.MemberShips.ToList();




        public MemberShip? GetById(int memberId, int planId) => _context.MemberShips.Find(planId, memberId);
        
        

        public int Update(MemberShip planMember)
        {
            if (planMember is null) return 0;
            _context.Update(planMember);
            return _context.SaveChanges();
        }
    }
}
