using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlanService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool Activate(int planId)
        {
            var plan=_unitOfWork.GetRepository<Plan>().GetById(planId);
            if (plan is null ||HasActiveMemberShips(planId)) 
                return false;

            plan.IsActive = !plan.IsActive;

            plan.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.GetRepository<Plan>().Update(plan);
            return _unitOfWork.SaveChanges()>0;
        }

        public bool CreatePlan(CreatePlanViewModel model)
        {
            if(model is null) 
                return false;
            try
            {


                var plan = new Plan
                {
                    Name = model.PlanName,
                    DurationDays = model.DurationDays,
                    Description = model.Description,
                    Price = model.Price,
                    IsActive = model.IsActive
                };
                _unitOfWork.GetRepository<Plan>().Add(plan);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var plans=_unitOfWork.GetRepository<Plan>().GetAll().OrderByDescending(x=>x.IsActive).ThenBy(x=>x.Price);
            if (plans is null || !plans.Any())
                return [];
            var model = plans.Select(plan => new PlanViewModel
            {
                Id= plan.Id,
                Name= plan.Name,
                Description= plan.Description,
                DurationDays= plan.DurationDays,
                Price= plan.Price,
                IsActive= plan.IsActive,

            });
            return model;
        }

        
        public PlanViewModel? GetPlanById(int planId)
        {
            var plan=_unitOfWork.GetRepository<Plan>().GetById(planId);
            if (plan is null)
                return null;

            var model = new PlanViewModel
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                Price = plan.Price,
                IsActive = plan.IsActive,
            };
            return model;
            
        }

        public UpdatePlanViewModel? GetPlanToUpdate(int planId)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
            if (plan is null || plan.IsActive==false)
                return null;
            return new UpdatePlanViewModel
            {
                PlanName = plan.Name,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                Price = plan.Price,

            };

        }

        public bool UpdatePlan(int planId, UpdatePlanViewModel model)
        {
            try
            {
                var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
                if (plan is null || HasActiveMemberShips(planId))
                    return false;
                plan.Name = model.PlanName;
                plan.Description = model.Description;
                plan.DurationDays = model.DurationDays;
                plan.Price = model.Price;

                _unitOfWork.GetRepository<Plan>().Update(plan);

                return _unitOfWork.SaveChanges()>0;
            }
            catch
            {
                return false;
            }
        }

        #region Helper Methods
        private bool HasActiveMemberShips(int planId)
        {
            return _unitOfWork.GetRepository<MemberShip>()
                .Any(p => p.PlanId == planId&& p.EndDate>=DateTime.UtcNow);
          //  return _unitOfWork.GetRepository<MemberShip>()
              //  .Any(p => p.PlanId == planId&& p.Status=="Active" );
        }

        #endregion
    }
}
