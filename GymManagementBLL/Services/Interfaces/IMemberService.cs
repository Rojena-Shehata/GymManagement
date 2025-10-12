using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IMemberService
    {
        bool createMember(CreateMemberViewModel memberModel);
        bool UpdateMemberData(int memberId, MemberToUpdateViewModel memberViewModel);
        MemberToUpdateViewModel? GetMemberToUpdate(int memberId);
        IEnumerable<MemberViewModel> GetAllMembers();
        MemberViewModel? GetMemberDetails(int memberId);
        HealthRecordViewModel? GetMemberHealthRecord(int memberId);
        bool RemoveMember(int memberId);
    }
}
