using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public bool CreateSession(CreateSessionViewModel sessionModel)
        {
            if(sessionModel is null) 
                return false;
            if(!IsTrainerExist(sessionModel.TrainerId))
                return false;
            if(!IsCategoryExist(sessionModel.CategoryId))
                return false;
            if(IsValidDateRange(sessionModel.StartDate, sessionModel.EndDate)) 
                return false;

            var session=_mapper.Map<CreateSessionViewModel,Session>(sessionModel);
            _unitOfWork.GetRepository<Session>().Add(session);
            return _unitOfWork.SaveChanges()>0;

        }




        //specification pattern
        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var sessions = _unitOfWork.SessionRepository
                            .GetAllSessionsWithTrainerAndCategory()
                            .OrderByDescending(x=>x.StartDate);

            if (sessions == null || sessions.Any())
                return [];
            //automatic mapping => auto mapper
            var mappedSessions=_mapper.Map<IEnumerable<Session>,IEnumerable<SessionViewModel>>(sessions);
            foreach (var session in mappedSessions)
            {
                session.AvailableSlots =session.Capacity - _unitOfWork.SessionRepository.GetCountOfBookingSlots(session.Id);
            }
            return mappedSessions;
        }

        public SessionViewModel? GetSessionById(int sessionId)
        {
            var session=_unitOfWork.SessionRepository.GetSessionWithTrainerAndCategory(sessionId);
            if(session is null)
                return null;

           var mappedModel=_mapper.Map<Session,SessionViewModel>(session);

            mappedModel.AvailableSlots = session.Capacity - _unitOfWork.SessionRepository.GetCountOfBookingSlots(session.Id);

            return mappedModel;
        }



        public UpdateSessionViewModel? GetSessionToUpdate(int sessionId)
        {
            var session=_unitOfWork.GetRepository<Session>().GetById(sessionId);
            if(session is null) 
                return null;
            //if(!IsSessionAvailapleForUpdate(session))
            //    return null;
            return _mapper.Map<Session, UpdateSessionViewModel>(session) ;


        }

        public bool UpdateSession(int sessionId, UpdateSessionViewModel sessionModel) 
        {
            var session=_unitOfWork.GetRepository<Session>().GetById(sessionId);
            if (session is null) 
                return false;
            if( ! IsSessionAvailapleForUpdate(session))
                return false;
            if(!IsTrainerExist(sessionModel.TrainerId))
                return false;
            if(IsValidDateRange(sessionModel.StartDate,sessionModel.EndDate))
                return false;
          
            session=  _mapper.Map<UpdateSessionViewModel, Session>(sessionModel);
            session.UpdatedAt=DateTime.UtcNow;
            _unitOfWork.GetRepository<Session>().Update(session);
           
            return _unitOfWork.SaveChanges()>0;

        }



        public bool RemoveSession(int sessionId)
        {
            var session=_unitOfWork.GetRepository<Session>().GetById(sessionId);
            if (session is null) 
                return false;
            if (!ISessionAvailableForRemove(session))
                return false;            
            
            _unitOfWork.GetRepository<Session>().Delete(session);
            return _unitOfWork.SaveChanges()>0;
        }



        #region Helper Methods
        private bool IsTrainerExist(int trainerId)
        {
            return _unitOfWork.GetRepository<Trainer>().Any(t=>t.Id== trainerId);
        }
        private bool IsCategoryExist(int categoryId)
        {
            return _unitOfWork.GetRepository<Category>().Any(t=>t.Id== categoryId);
        }
        private bool IsValidDateRange(DateTime startDate, DateTime endDate)
        {
            return endDate > startDate && startDate > DateTime.UtcNow;
        }
        private bool IsSessionAvailapleForUpdate(Session session)
        {
            if(session.EndDate<DateTime.UtcNow)
                return false;
            if(session.StartDate<=DateTime.UtcNow)
                return false;
            bool hasActiveBookings=_unitOfWork.SessionRepository
                .GetCountOfBookingSlots(session.Id) > 0;
            if(hasActiveBookings)
                return false;
            return true;
        }

        private bool ISessionAvailableForRemove(Session session)
        {
            if(session.StartDate<=DateTime.UtcNow && session.EndDate>DateTime.UtcNow)
                return false;
            if (session.StartDate > DateTime.UtcNow) //upcoming
                return false;
            var hasActiveBookings=_unitOfWork.GetRepository<Booking>()
                                             .Any(x=>x.SessionId==session.Id);
            if(hasActiveBookings)
                return false;

            return true;
        }

        #endregion
    }
}
