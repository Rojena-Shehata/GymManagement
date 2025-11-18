using GymManagementBLL.ViewModels;
using GymManagementBLL.ViewModels.MembershipViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IMembershipService
    {
        IEnumerable<MembershipViewModel> GetAll();
        bool Create(AddMembershipViewModel model);
        IEnumerable<IdNameViewModelForDropDown> GetMembersForDropDown();
        IEnumerable<IdNameViewModelForDropDown> GetPlansForDropDown();
        bool Remove(int memberId);

    }
}
