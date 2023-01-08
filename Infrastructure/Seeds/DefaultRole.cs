using Domain.Entity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Seeds
{
    public class DefaultRole
    {
         public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
            {
                // Method To Create Roles Automatic by Default
                //if (!roleManager.Roles.Any())
                //{

                await roleManager.CreateAsync(new IdentityRole(Helper.Roles.SuperAdmin.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Helper.Roles.Admin.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Helper.Roles.Basic.ToString()));


                // }
            }
    }
   
}
