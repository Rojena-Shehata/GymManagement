using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        Booking? GetById(int memberId, int sessionId);
        IEnumerable<Booking> GetAll();
        int Add(Booking booking);
        int Update(Booking booking);
        int Delete(int memberId, int sessionId);
    }
}
