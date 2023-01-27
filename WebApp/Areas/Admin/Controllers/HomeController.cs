using Domain.Entity;
using Infrastructure.Data;
using Infrastructure.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IServicesRepository<Product> _servicesProduct;
        private readonly ApplicationDbContext _db;
        public HomeController(ApplicationDbContext db, UserManager<ApplicationUser> userManger,IServicesRepository<Product> servicesProduct)
        {
            _db = db;
            _servicesProduct = servicesProduct;
            _userManager = userManger;
        }


		public IActionResult Index()
        {
            ViewBag.RecentProduct = _servicesProduct.GetAll().Count();
            ViewBag.UserRegistertion = _db.Users.Count();
            ViewBag.Orders = _db.OrderDetails.Count();
            ViewBag.Messages = _db.ContactUs.Count();


            return View();
        }
        public IActionResult Denied()
        {
            return View();
        }
    }
}
