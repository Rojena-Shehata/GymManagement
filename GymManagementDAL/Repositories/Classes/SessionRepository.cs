using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDbContext _context;

        public SessionRepository(GymDbContext context):base(context) 
        {
            _context = context;
        }

        public IEnumerable<Session> GetAllSessionsWithTrainerAndCategory()
        {
            return _context.Sessions
                .Include(s => s.Trainer)
                .Include(s => s.Category);
                
        }

        public Session? GetSessionWithTrainerAndCategory(int sessionId)
        {
            var session = _context.Sessions.Where(session=>session.Id == sessionId)
                                           .Include(s=>s.Trainer)
                                           .Include(s=>s.Category).FirstOrDefault();
            return session;
        }

        int ISessionRepository.GetCountOfBookingSlots(int sessionId)
        {
            return _context.Bookings.Where(s => s.SessionId == sessionId).Count();
        }
    }
}
