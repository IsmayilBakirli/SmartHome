using Microsoft.AspNetCore.Identity;
using SmartHome.Domain.Entities.Identity;
using SmartHome.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Infrastructure.Data
{
    public class DbInitializer
    {
        public static async Task SeedRolesAndAdminAsync(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            string[] roleNames = Enum.GetNames(typeof(Roles));

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
            string firstName = "Kamal";
            string lastName = "Xelefov";
            string adminEmail = "KamalXelefov50@gmail.com";
            string userName = "Kamal1905";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var newAdminUser = new AppUser
                {
                    FirstName=firstName,
                    LastName=lastName,
                    UserName = userName,
                    Email = adminEmail
                };

                var createAdminResult = await userManager.CreateAsync(newAdminUser, "d30825de-f838-46dc-a0c3-94efe4fece51"); 

                if (createAdminResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdminUser,Roles.Admin.ToString());
                }
                else
                {
                    
                }
            }
        }

    }
}
