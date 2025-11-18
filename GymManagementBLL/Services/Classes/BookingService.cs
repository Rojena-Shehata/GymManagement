using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels;
using GymManagementBLL.ViewModels.BookingViewModels;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Classes;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookingService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        
        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var sessions = _unitOfWork.SessionRepository
                            .GetAllSessionsWithTrainerAndCategory()
                            .OrderByDescending(x => x.StartDate);

            if (sessions == null || !sessions.Any())
                return [];
            //automatic mapping => auto mapper
            var mappedSessions = _mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(sessions);
            foreach (var session in mappedSessions)
            {
                session.AvailableSlots = session.Capacity - _unitOfWork.SessionRepository.GetCountOfBookingSlots(session.Id);
            }
            return mappedSessions;

        }


        public IEnumerable<MembersForOngoingSessionViewModel> GetMembersForOngoingSession(int sessionId)
        {

            var bookings = _unitOfWork.BookingRepository.GetBookingsWithMembersBySessionId(sessionId);
            if (bookings is null || !bookings.Any())
                return [];

            var model = bookings.Select(x => new MembersForOngoingSessionViewModel
            {
                MemberId = x.MemberId,
                SessionId = x.SessionId,
                BookingDate = $"{x.CreatedAt.Date.ToString("MMM dd yyyy")} , {x.CreatedAt.ToString("hh : mm tt")}",
                MemberName = x.Member.Name,
                Phone = x.Member.Phone,
                IsAttended=x.IsAttend

            });
           
            return model;
        }

        public IEnumerable<MembersForUpcomigSessionViewModel> GetMembersForUpcomingSessions(int sessionId)
        {
            var bookings = _unitOfWork.BookingRepository.GetBookingsWithMembersBySessionId(sessionId);
            if (bookings is null || !bookings.Any())
                return [];

            var model = bookings.Select(x => new MembersForUpcomigSessionViewModel
            {
                MemberId = x.MemberId,
                SessionId = x.SessionId,
                BookingDate = $"{x.CreatedAt.Date.ToString("MMM dd yyyy")} , {x.CreatedAt.ToString("hh : mm tt")}",
                MemberName=x.Member.Name,
                Phone=x.Member.Phone

            });
            return model;
        }

        public bool Create(CreateBookingForSessionViewModel input)
        {
            try
            {

                if (input is null)
                    return false;
                if (!HasValidPhoneNumber(input.memberId, input.MemberPhone))
                    return false;
                if (!HasActiveMemberShips(input.memberId))
                    return false;

                var session = _unitOfWork.GetRepository<Session>().GetById(input.SessionId);
                if (session is null)
                    return false;
                //Only future sessions can be booked
                if (session.StartDate < DateTime.UtcNow)
                    return false;
                //Member cannot book the same session twice
                if (IsSessionBookedBefore(input.SessionId, input.memberId))
                    return false;
                if (!HasAvailaleSlots(session))
                    return false;
                Booking newBooking = new Booking
                {
                    MemberId = input.memberId,
                    SessionId = input.SessionId,
                    IsAttend = false,
                    CreatedAt = DateTime.UtcNow
                };
                _unitOfWork.GetRepository<Booking>().Add(newBooking);
                return _unitOfWork.SaveChanges() > 0;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed To create Method in bookingService  CreateBookingForSessionViewModel ");

                return false;
            }
        }

        public bool CreateMemberWithMultipleSeesion(CreateMultipleBookingViewModel input)
        {
            try
            {
                if (input is null)
                    return false;
                if (input.SelectedSessionsIds is null && !input.SelectedSessionsIds.Any())
                    return false;
                if(HasValidPhoneNumber(input.memberId,input.Memberphone))
                    return false;
                if (!HasActiveMemberShips(input.memberId))
                    return false;
                foreach (var sessionId in input.SelectedSessionsIds)
                {
                    var session = _unitOfWork.GetRepository<Session>().GetById(sessionId);
                    if (session is null)
                        return false;
                    //Only future sessions can be booked
                    if (session.StartDate < DateTime.UtcNow)
                        return false;
                    //Member cannot book the same session twice
                    if (!IsSessionBookedBefore(sessionId, input.memberId))
                        return false;
                    if (!HasAvailaleSlots(session))
                        return false;
                    Booking newBooking = new Booking
                    {
                        MemberId = input.memberId,
                        SessionId = sessionId,
                        IsAttend = false,
                        CreatedAt = DateTime.UtcNow
                    };
                    _unitOfWork.GetRepository<Booking>().Add(newBooking);
                    return _unitOfWork.SaveChanges() > 0;

                }
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Failed To create booking Multiple sessions");
            }
            return false;

        }

        public IEnumerable<IdNameViewModelForDropDown> GetMembersForDropDown()
        {

            var members = _unitOfWork.GetRepository<Member>()
                    .GetAll(m => new IdNameViewModelForDropDown
                    {
                        Id = m.Id,
                        Name = m.Name,
                    });

            if (members is null || !members.Any())
                return [];
            return members;
        }

        public IEnumerable<IdNameViewModelForDropDown> GetSessionsToSelect()
        {

            var sessions = _unitOfWork.GetRepository<Session>()
                    .GetAll(m => new IdNameViewModelForDropDown
                    {
                        Id = m.Id,
                        Name = m.Description,
                    }, condition: x => x.StartDate>DateTime.UtcNow);//get upcoming sessions
            return sessions;
        }

        public bool Attendance(int sessionId, int memberId)
        {

            try
            {
                var booking = _unitOfWork.BookingRepository.GetById(x => x.SessionId == sessionId && x.MemberId == memberId && x.Session.StartDate <= DateTime.UtcNow);
                if (booking is null)
                    return false;
                booking.IsAttend = !booking.IsAttend;
                _unitOfWork.BookingRepository.Update(booking);
                return _unitOfWork.SaveChanges() > 0;

            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public bool Cancel(int sessionId, int memberId)
        {
            try
            {
                var booking = _unitOfWork.BookingRepository.GetById(x => x.SessionId == sessionId && x.MemberId == memberId&&x.Session.StartDate>DateTime.UtcNow);
                if (booking is null)
                    return false;
                _unitOfWork.BookingRepository.Delete(booking);
                return _unitOfWork.SaveChanges() > 0;

            }
            catch (Exception ex)
            {
                return false;
            }

        }

        #region Helper Methods  Region

        private bool HasValidPhoneNumber(int memberId,string phoneNumber)
        {
            var IsValid = _unitOfWork.GetRepository<Member>().Any(x => x.Id == memberId && x.Phone==phoneNumber);
            return IsValid;

        }
        private bool HasActiveMemberShips(int memberId)
        {
           var haveActiveMemberShips =_unitOfWork.GetRepository<MemberShip>().Any(x=>x.MemberId == memberId&&x.EndDate>=DateTime.UtcNow);//active Membership
            return haveActiveMemberShips;

        }

        private bool HasAvailaleSlots(Session  session )
        {
            var bookingSlots = _unitOfWork.SessionRepository.GetCountOfBookingSlots(session.Id);
            
            return bookingSlots<session.Capacity;
        }
        private bool IsSessionBookedBefore(int sessionId, int memberId)
        {
          return   _unitOfWork.GetRepository<Booking>().Any(x=>x.SessionId == sessionId && x.MemberId==memberId);
        }

        
        #endregion
    }
}
