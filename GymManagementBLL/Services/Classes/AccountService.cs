using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.AccountViewModels;
using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountService(UserManager<ApplicationUser> userManager)
        {
           _userManager = userManager;
        }

        public async Task<ApplicationUser?> ValidateUser(LoginViewModel input)
        {
            if (input is null)
                return null;

            var User= await _userManager.FindByEmailAsync(input.Email);

            if (User is null) 
                return null;

            var isPasswordValid = await _userManager.CheckPasswordAsync(User,input.Password);

            return isPasswordValid ? User : null;
        }
    }
}
