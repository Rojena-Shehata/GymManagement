using GymManagementBLL.AttachmentService;
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
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAttachmentService _attachmentService;

        public MemberService(IUnitOfWork unitOfWork,IAttachmentService attachmentService)
        {
            _unitOfWork = unitOfWork;
            _attachmentService = attachmentService;
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

                var photoName = _attachmentService.Upload("Members", memberModel.Photo);
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
                    },
                    Photo = photoName
                };

                _unitOfWork.GetRepository<Member>().Add(member);
              var isCreated=  _unitOfWork.SaveChanges()>0;
                if (!isCreated)
                {
                    _attachmentService.Delete(photoName, "Members");
                    return false;
                }
                else
                    return isCreated;
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
                                  .GetById(x => x.MemberId == memberId && x.EndDate > DateTime.Now); //Status == "Active"

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
                var PhotoName = member.Photo;

                memberRepo.Delete(member);
                var isDeleted= _unitOfWork.SaveChanges()>0;
                if(isDeleted)
                {
                    if(PhotoName is not null)
                    {
                       _attachmentService.Delete(PhotoName, "Members");
                        
                    }
                }
                return isDeleted;


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
                
                    if (IsEmailExists(memberViewModel.Email,memberId))
                        return false;                
                
                    if (IsPhoneExists(memberViewModel.Phone,memberId))
                        return false;


                //member.DateOfBirth = memberViewModel.DateOfBirth;
                member.Phone = memberViewModel.Phone;
                member.Email = memberViewModel.Email;
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

        private bool IsEmailExists(string email,int? id=null)
        {
            bool hasEmailExist;
            if (id  is null)
            {
                hasEmailExist = _unitOfWork.GetRepository<Member>().Any(x => x.Email == email);

            }
            else
            {                //Exclude last email of member need to update
                hasEmailExist = _unitOfWork.GetRepository<Member>().Any(x => x.Email == email && x.Id != id);

            }
            return hasEmailExist;

        }
        private bool IsPhoneExists(string phone, int? id=null)
        {
            bool isPhoneExist;
            if(id is null)
                isPhoneExist = _unitOfWork.GetRepository<Member>().Any(x => x.Phone == phone);
            else
                isPhoneExist = _unitOfWork.GetRepository<Member>().Any(x => x.Phone == phone && x.Id != id);//Exclude phone of user need to update           


            return isPhoneExist;
        }
        #endregion
    }
}
