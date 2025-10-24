using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface ISessionService
    {
        IEnumerable<SessionViewModel> GetAllSessions();
        SessionViewModel? GetSessionById(int sessionId);
        bool CreateSession(CreateSessionViewModel sessionModel);
        bool UpdateSession(int sessionId,UpdateSessionViewModel sessionModel);
        bool RemoveSession(int sessionId);
        UpdateSessionViewModel? GetSessionToUpdate(int sessionId);
        IEnumerable<CategorySelectViewModel> GetCategoriesForDropDown();
        IEnumerable<TrainerSelectViewModel> GetTrainersForDropDown();

    }
}
