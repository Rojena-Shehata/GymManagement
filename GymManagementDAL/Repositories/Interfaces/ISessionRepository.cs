using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface ISessionRepository:IGenericRepository<Session>
    {
        IEnumerable<Session> GetAllSessionsWithTrainerAndCategory();
        IEnumerable<Session> GetAllSessionsWithTrainerAndCategory(Expression<Func<Session,bool>> condition);
        int GetCountOfBookingSlots(int sessionId);
        Session? GetSessionWithTrainerAndCategory(int sessionId);
    }
}
