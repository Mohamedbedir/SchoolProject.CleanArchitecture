using Microsoft.AspNetCore.Identity;
using SchoolProject.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Infrastructure.Seeder
{
    public static class DataSeeder
    {
        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] Roles = { "Admin", "User", "Supervisor" };
            if (roleManager.Roles.Any())
            {
                foreach (var role in Roles)
                {
                    var identityrole=new IdentityRole()
                    {
                        Name=role
                    };
                    await roleManager.CreateAsync(identityrole);
                }
            }

        }
        public static async Task SeedUsers(UserManager<AppUser> userManager)
        {
            if (userManager.Users.Any()) 
            {
                var user= new AppUser()
                {
                    FullName="Mahmoud El-Der",
                    Email="Elder@gmail.com",
                    UserName="Elder",
                    PhoneNumber="01234567891",
                    Address="Cairo",
                    Country="EGY"
                };
                await userManager.CreateAsync(user,"P@ssw0rd");
                await userManager.AddToRoleAsync(user, "Admin");
            }

        }
    }
}
