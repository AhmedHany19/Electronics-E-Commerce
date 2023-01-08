using Domain.Constants;
using Domain.Entity;
using Infrastructure.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class PermissionsController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public PermissionsController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }


        public async Task<IActionResult> Index(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            var claims = _roleManager.GetClaimsAsync(role).Result.Select(x => x.Value).ToList();
            var allPermission = Permissions.PermissionList().Select(x => new RoleClaimsViewModel { Value = x }).ToList();
            foreach (var permission in allPermission)
                if (claims.Any(x => x == permission.Value))
                    permission.Selected = true;

            return View(new PermissionViewModel
            {
                RoleId = roleId,
                RoleName = role.Name,
                RoleClaims = allPermission
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(PermissionViewModel? model)
        {
            var role = await _roleManager.FindByIdAsync(model.RoleId);
            var claims = await _roleManager.GetClaimsAsync(role);
            foreach (var claim in claims)
                await _roleManager.RemoveClaimAsync(role, claim);

            var selectedClaims = model.RoleClaims.Where(x => x.Selected).ToList();
            foreach (var claim in selectedClaims)
                await _roleManager.AddClaimAsync(role, new Claim(Helper.Permission, claim.Value));



            return RedirectToAction("Roles", "Accounts");

        }
    }
}
