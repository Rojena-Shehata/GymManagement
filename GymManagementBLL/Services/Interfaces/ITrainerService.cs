using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface ITrainerService
    {
        IEnumerable<TrainerViewModel> GetAllTrainers();
        bool CreatTrainer(CreateTrainerViewModel model);
        TrainerViewModel? GetTrainerDetails(int trainerId);
        bool UpdateTrainerData(int trainerId, TrainerToUpdateViewModel model);
         TrainerToUpdateViewModel? GetTrainerModelToUpdate(int trainerId);
        bool RemoveTrainer(int trainerId);
    }
}
