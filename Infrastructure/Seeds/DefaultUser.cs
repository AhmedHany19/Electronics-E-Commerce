using Domain.Constants;
using Domain.Entity;

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Domain.Entity.Helper;

namespace Infrastructure.Seeds
{
    public static class DefaultUser
    {
        public static async Task SeedBasicAsync(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            var DefaultUser = new ApplicationUser
            {
                UserName = Helper.UserNameBasic,
                Email = Helper.EmailBasic,
                Name = Helper.NameBasic,
                ActiveUser = true,
                EmailConfirmed = true
            };

            var user = await userManager.FindByEmailAsync(DefaultUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(DefaultUser, Helper.PasswordBasic);
                await userManager.AddToRoleAsync(DefaultUser, Helper.Roles.Basic.ToString());
            }
        }



        public static async Task SeedSuperAdminAsync(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            var DefaultUser = new ApplicationUser
            {
                UserName = Helper.UserName,
                Email = Helper.Email,
                Name = Helper.Name,
                ActiveUser = true,
                EmailConfirmed = true
            };

            var user = await userManager.FindByEmailAsync(DefaultUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(DefaultUser, Helper.Password);
                await userManager.AddToRoleAsync(DefaultUser, Helper.Roles.SuperAdmin.ToString());

                // If You Need Add All Roles In This User 
                // await userManager.AddToRolesAsync(DefaultUser,new List<string> { Helper.Roles.SuperAdmin.ToString(),Helper.Roles.Admin.ToString(), Helper.Roles.Basic.ToString() });
            }

            // Code Seeding Claims
            await roleManager.SeedClaimsAsync();

        }



        // Method To Controll about all claims in role SuperAdmin
        public static async Task SeedClaimsAsync(this RoleManager<IdentityRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync(Helper.Roles.SuperAdmin.ToString());

            var modules = Enum.GetValues(typeof(PermissionsModuleName));
            foreach (var module in modules)
                await roleManager.AddPermissionClaims(adminRole, module.ToString());


        }
        public static async Task AddPermissionClaims(this RoleManager<IdentityRole> roleManager, IdentityRole role, string? module)
        {
            var AllClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = Permissions.GeneratePermissionsFromModule(module);

            foreach (var permission in allPermissions)
                if (!AllClaims.Any(x => x.Type == Helper.Permission && x.Value == permission))
                    await roleManager.AddClaimAsync(role, new Claim(Helper.Permission, permission));
        }

    }
}
