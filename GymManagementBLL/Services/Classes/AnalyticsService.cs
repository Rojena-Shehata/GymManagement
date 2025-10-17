using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.AnalyticsViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnalyticsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public AnalyticsViewModel GetAnalyticsData()
        {
            
                var sessions = _unitOfWork.GetRepository<Session>().GetAll();

            return new AnalyticsViewModel
            { 
                TotalMembers = _unitOfWork.GetRepository<Member>().GetCount(),
                ActiveMembers = _unitOfWork.GetRepository<MemberShip>().GetCount(x=>x.Status=="Active"),
                Trainers = _unitOfWork.GetRepository<Trainer>().GetCount(),
                UpcomingSessions =sessions.Count(x=>x.StartDate>DateTime.UtcNow),
                OngoingSessions = sessions.Count(x=>x.StartDate<=DateTime.UtcNow&&x.EndDate>=DateTime.UtcNow),
                CompletedSessions = sessions.Count(x=>x.EndDate<DateTime.UtcNow),

            };
        }
    }
}
