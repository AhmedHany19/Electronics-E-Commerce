using Domain.Entity;
using Infrastructure.Data;
using Infrastructure.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Areas.Customer.Controllers
{
	[Area("Customer")]
	public class UsersController : Controller
	{
		#region Declaration
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly ApplicationDbContext _context;

		#endregion
		#region Constructor
		public UsersController(RoleManager<IdentityRole> roleManager,
			UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
		{
			_roleManager = roleManager;
			_userManager = userManager;
			_signInManager = signInManager;
			_context = context;
		}





        [AllowAnonymous]
        public IActionResult Register()
        {
           
            return View();
        }





        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegisterViewModel model)
        {
            model.Id = Guid.NewGuid().ToString();

            if (model!=null)
            {

                var user = new ApplicationUser
                {                
                    Name = model.Name,
                    UserName = model.Email,
                    Email = model.Email,
                    ActiveUser = true,
                    PhoneNumber=model.PhoneNumber.ToString(),              
                };

                var result=  await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Helper.Basic);
                    await _signInManager.SignInAsync(user, isPersistent: false);

                }


            }
                return RedirectToAction("Index", "Home");
            
        }




        #endregion
        [AllowAnonymous]
		public IActionResult Login()
		{
			return View();
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                    return RedirectToAction("Index", "Home");
                else
                    ViewBag.ErrorLogin = false;
            }

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Logout(LoginViewModel model)
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");

        }



    }


}