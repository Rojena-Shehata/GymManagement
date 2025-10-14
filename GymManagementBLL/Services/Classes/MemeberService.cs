using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Entities.Enums;
using GymManagementDAL.Repositories.Classes;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class MemeberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        public MemeberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool createMember(CreateMemberViewModel memberModel)
        {
            try
            {
                if(memberModel == null) 
                    return false;
                if(IsEmailExists(memberModel.Email)) 
                    return false;
                if(IsPhoneExists(memberModel.Phone))
                    return false;
                var member = new Member
                {
                    Name = memberModel.Name,
                    Email = memberModel.Email,
                    Phone = memberModel.Phone,
                    Gender = memberModel.Gender,
                    DateOfBirth = memberModel.DateOfBirth,
                    Address = new Address
                    {
                        BuildingNumber = memberModel.BuildingNumber,
                        Street = memberModel.Street,
                        City = memberModel.City,
                    },
                    HealthRecord = new HealthRecord
                    {
                        BloodType = memberModel.HealthRecord.BloodType,
                        Height = memberModel.HealthRecord.Height,
                        Weight = memberModel.HealthRecord.Weight,
                        Note = memberModel.HealthRecord.Note
                    }

                };

                _unitOfWork.GetRepository<Member>().Add(member);
                _unitOfWork.SaveChanges();
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var members = _unitOfWork.GetRepository<Member>().GetAll()?? []; //[] c#12
            if (members is null || !members.Any())
                return [];
            var membersViewModel = members.Select(x => new MemberViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Photo = x.Photo,
                Phone = x.Phone,
                Email = x.Email,
                DateOfBirth = x.DateOfBirth.ToShortDateString(),
                Gender = x.Gender.ToString(),
            });
            return membersViewModel;
        }

        public MemberViewModel? GetMemberDetails(int memberId)
        {
            var member=_unitOfWork.GetRepository<Member>().GetById(memberId);
            if (member is null)
                return null;
            var memberViewModel = new MemberViewModel
            {

                Id = member.Id,
                Name = member.Name,
                Photo = member.Photo,
                Phone = member.Phone,
                Email = member.Email,
                DateOfBirth = member.DateOfBirth.ToShortDateString(),
                Gender = member.Gender.ToString(),
                Address=FormatAddress(member.Address),
            };
            var activeMemberShip = _unitOfWork.GetRepository<MemberShip>()
                                  .GetById(x => x.MemberId == memberId && x.Status == "Active");
            if (activeMemberShip is not null )
            {
                var activePlan=_unitOfWork.GetRepository<Plan>().GetById(activeMemberShip.PlanId);
                memberViewModel.PlanName = activePlan?.Name;
                memberViewModel.MembershipStartDate=activeMemberShip.CreatedAt.ToShortDateString();
                memberViewModel.MembershipStartDate=activeMemberShip.EndDate.ToShortDateString();
            }
            return memberViewModel;
        }

        public HealthRecordViewModel? GetMemberHealthRecord(int memberId)
        {
            var healthRecord = _unitOfWork.GetRepository<HealthRecord>().GetById(memberId);
            if (healthRecord is null)
                return null;
            var healthRecordViewModel = new HealthRecordViewModel
            {
                Height = healthRecord.Height,
                Weight = healthRecord.Weight,
                BloodType = healthRecord.BloodType,
                Note = healthRecord.Note,
            };
            return healthRecordViewModel;
        }

        public MemberToUpdateViewModel? GetMemberToUpdate(int memberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(memberId);
            if(member is null)  
                return null;

            var memberViewModel = new MemberToUpdateViewModel
            {
                Name = member.Name,
                Phone = member.Phone,
                Email = member.Email,
                Photo = member.Photo,
                //DateOfBirth = member.DateOfBirth,
                Gender = member.Gender,
                BuildingNumber = member.Address.BuildingNumber,
                Street = member.Address.Street,
                City = member.Address.City,
            };
            return memberViewModel;
        }


        public bool RemoveMember(int memberId)
        {
            var memberRepo=_unitOfWork.GetRepository<Member>();
            var member=memberRepo.GetById(memberId);
            if (member is null) 
                return false;
            var hasActiveBookings=_unitOfWork.GetRepository<Booking>()
                .Any(x=>x.MemberId==memberId&&x.Session.StartDate>DateTime.UtcNow);
            if(hasActiveBookings)
                return false ;
            var memberships=_unitOfWork.GetRepository<MemberShip>().GetAll(x=>x.MemberId==memberId);
            try
            {
                if(memberships is not null)
                {
                    if (memberships.Any())
                    {
                        _unitOfWork.GetRepository<MemberShip>().DeleteRange(memberships);
                      
                    }
                }
                memberRepo.Delete(member);
                return _unitOfWork.SaveChanges()>0;
                 
            }
            catch  
            {
                return false;
            }
        }

        public bool UpdateMemberData(int memberId, MemberToUpdateViewModel memberViewModel)
        {
            if(memberViewModel is null) 
                return false;
            var member=_unitOfWork.GetRepository<Member>().GetById(memberId);
            if (member is null) 
                return false;

            try
            {

                if (member.Email != memberViewModel.Email)
                {
                    if (IsEmailExists(memberViewModel.Email))
                        return false;
                    member.Email = memberViewModel.Email;
                }
                if(member.Phone != memberViewModel.Phone)
                {
                    if (IsPhoneExists(memberViewModel.Phone))
                        return false;
                    member.Phone = memberViewModel.Phone;
                }
                
                //member.DateOfBirth = memberViewModel.DateOfBirth;
                member.Gender = memberViewModel.Gender;
                member.Address.BuildingNumber = memberViewModel.BuildingNumber;
                member.Address.Street= memberViewModel.Street;
                member.Address.City = memberViewModel.City;
                member.UpdatedAt=DateTime.UtcNow;
                _unitOfWork.GetRepository<Member>().Update(member);
                _unitOfWork.SaveChanges();
                return true;

            }
            catch 
            {
                return false; 
            }
        }



        #region Helper Methods
        private string FormatAddress(Address address)
        {
            if (address is null)
                return "N/A";
            
            return $"{address.BuildingNumber}, {address.Street}, {address.City}";
        }

        private bool IsEmailExists(string email)
        {
            var hasEmailExist=_unitOfWork.GetRepository<Member>().Any(x=>x.Email==email);
            return hasEmailExist;
        }
        private bool IsPhoneExists(string phone)
        {
            var hasPhoneExist=_unitOfWork.GetRepository<Member>().Any(x=>x.Phone==phone);
            return hasPhoneExist;
        }
        #endregion
    }
}
