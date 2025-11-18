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
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        private readonly GymDbContext _context;

        public BookingRepository(GymDbContext context):base(context) 
        {
            _context = context;
        }

        public Booking? GetBookingIncludeSession(int sessionId, int memberId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Booking> GetBookingsWithMembersBySessionId(int sessionId)
        {
            var result = _context.Bookings.AsNoTracking().Where(x => x.SessionId == sessionId)
                                                         .Include(x => x.Member);
            return result;
        }

        public bool IsMemberValidPhoneNumberAndActiveMembership(int memberId, string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;
            return false;


        }
    }
}
