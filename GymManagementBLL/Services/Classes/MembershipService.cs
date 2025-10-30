using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels;
using GymManagementBLL.ViewModels.MembershipViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class MembershipService : IMembershipService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MembershipService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool Create(AddMembershipViewModel model)
        {
            try
            {


                if (model is null)
                    return false;
                if (!IsMemberExist(model.MemberId))
                    return false;
                //Member cannot have duplicate active memberships 
                if (HasActiveMemberShips(model.MemberId))
                    return false;
                var plan = GetPlan(model.PlanId);
                if (plan is null)
                    return false;
                if (!plan.IsActive)
                    return false;
                var membership = new MemberShip
                {
                    MemberId = model.MemberId,
                    PlanId = model.PlanId,
                    CreatedAt = model.StartDate,
                    EndDate = model.StartDate.AddDays(plan.DurationDays)
                };

                _unitOfWork.GetRepository<MemberShip>().Add(membership);
                return _unitOfWork.SaveChanges()>0;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public IEnumerable<MembershipViewModel> GetAll()
        {
            try
            {


                var members = _unitOfWork.GetRepository<MemberShip>()
                    .GetAll<MembershipViewModel>(x => new MembershipViewModel
                    {
                        MemberId = x.MemberId,
                        PlanId = x.PlanId,
                        MemberName = x.Member.Name,
                        PlanName = x.Plan.Name,
                        StartDate = x.CreatedAt,
                        EndDate = x.EndDate,
                        IsActive = x.Status == "Active"
                    });


                return members;
            }
            catch (Exception ex) 
            {
                return [];
            }
        }

       public IEnumerable<IdNameViewModelForDropDown> GetMembersForDropDown()
        {
            var members= _unitOfWork.GetRepository<Member>()
                    .GetAll(m=>new IdNameViewModelForDropDown
                    {
                      Id = m.Id,
                      Name = m.Name,
                    });
            if (members is null || !members.Any())
                return [];
            return members;
        }

        public IEnumerable<IdNameViewModelForDropDown> GetPlansForDropDown()
        {

            var plans = _unitOfWork.GetRepository<Plan>()
                    .GetAll(m => new IdNameViewModelForDropDown
                    {
                        Id = m.Id,
                        Name = m.Name,
                    },condition:x=>x.IsActive);
            if (plans is null || !plans.Any())
                return [];
            return plans;
        }


        public bool Remove(int  memberId)
        {
            try
            {


                var membership = _unitOfWork.GetRepository<MemberShip>().GetById(x => x.MemberId == memberId && x.EndDate > DateTime.UtcNow);
                if (membership is null)
                    return false;
                _unitOfWork.GetRepository<MemberShip>().Delete(membership);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #region Helper Metthods

        Plan? GetPlan(int planId)
        {
            return _unitOfWork.GetRepository<Plan>().GetById(planId);
        }
         bool IsMemberExist(int memberId)
        {
            return _unitOfWork.GetRepository<Member>().Any(x=>x.Id==memberId);
        }
         bool HasActiveMemberShips(int memberId)
        {
            return _unitOfWork.GetRepository<MemberShip>().Any(x => x.MemberId == memberId&&x.EndDate>=DateTime.UtcNow);//has active memberships => 

        }

        #endregion
    }
}
