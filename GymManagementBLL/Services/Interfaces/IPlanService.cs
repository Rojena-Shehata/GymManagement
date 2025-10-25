using GymManagementBLL.ViewModels.PlanViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IPlanService
    {
        bool CreatePlan(CreatePlanViewModel model);
        bool UpdatePlan(int planId,UpdatePlanViewModel model);
        UpdatePlanViewModel? GetPlanToUpdate(int planId);
        IEnumerable<PlanViewModel> GetAllPlans();
        PlanViewModel? GetPlanById(int planId);
        bool Activate(int planId);
    }
}
