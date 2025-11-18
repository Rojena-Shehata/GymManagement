using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            var sessions= _context.Sessions
                .Include(s => s.Trainer)
                .Include(s => s.Category);
            return sessions;
                
        }

        public IEnumerable<Session> GetAllSessionsWithTrainerAndCategory(Expression<Func<Session, bool>>  condition )
        {
            var session = _context.Sessions.AsNoTracking().Where(condition);
            return session;
        }

        public Session? GetSessionWithTrainerAndCategory(int sessionId)
        {
            var session = _context.Sessions.AsNoTracking().Where(session=>session.Id == sessionId)
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
