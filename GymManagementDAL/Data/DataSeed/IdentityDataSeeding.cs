using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.DataSeed
{
    public static class IdentityDataSeeding
    {

        public static async Task<bool> SeedData(RoleManager<IdentityRole> roleManager,UserManager<ApplicationUser> userManager)
        {
            try
            {
                var hasRoles= await roleManager.Roles.AnyAsync();
                var hasUsers=await userManager.Users.AnyAsync();
                if (hasRoles&&hasUsers) 
                    return false;

                if (!hasRoles)
                {
                    var roles = new List<IdentityRole>
                    {
                       new IdentityRole()  {Name="SuperAdmin"},
                       new IdentityRole () {Name="Admin"}
                    };

                    foreach(var role in roles)
                    {
                        if (!await roleManager.RoleExistsAsync(role.Name))
                        {
                            await roleManager.CreateAsync(role);
                        }
                    }
                }

                if (!hasUsers)
                {
                    var mainAdmin = new ApplicationUser
                    {
                        FirstName = "Rojena",
                        LastName = "Shehata",
                        UserName = "rojena_shehata",
                        Email = "rojenashehata@gmail.com",
                        PhoneNumber = "01515857414"

                    };
                    await userManager.CreateAsync(mainAdmin, "P@ssw0rd");
                    await userManager.AddToRoleAsync(mainAdmin, "SuperAdmin");
                    var admin = new ApplicationUser
                    {
                        FirstName = "Dina",
                        LastName = "Shehata",
                        UserName = "dina_shehata",
                        Email = "dinashehata@gmail.com",
                        PhoneNumber = "01113129693"

                    };
                    await userManager.CreateAsync (admin, "P@ssw0rd");
                    await userManager.AddToRoleAsync(admin, "Admin");
                }


                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Failed in Identity Seeding : {ex.Message}");
                return false;
            }

        }
    }
}
