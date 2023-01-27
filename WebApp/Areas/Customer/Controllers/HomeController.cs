using Domain.Entity;
using Infrastructure.Data;
using Infrastructure.IRepository;
using Infrastructure.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Stripe.BillingPortal;
using System.Diagnostics;
using System.Security.Claims;
using WebApp.Models;
using X.PagedList;

namespace WebApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IServicesRepository<Category> _servicesCategory;
        private readonly IServicesRepository<Product> _servicesProduct;
        private readonly IServicesRepository<Brand> _servicesBrand;
        private readonly ApplicationDbContext _context;


        public static int brandCount = 0;
        public static int categoryCount = 0;


        public HomeController(ILogger<HomeController> logger,
            IServicesRepository<Product> servicesProduct,
            IServicesRepository<Category> servicesCategory,
            IServicesRepository<Brand> servicesBrand,
            ApplicationDbContext context)
        {
            _logger = logger;
            _servicesProduct = servicesProduct;
            _servicesCategory = servicesCategory;
            _servicesBrand = servicesBrand;
            _context = context;
            brandCount = _context.Brands.Count();
            categoryCount = _context.Categories.Count();

        }

		[AllowAnonymous]
		[HttpGet]
        public IActionResult Index()
        {
            ProductAndCategoriesViewModel proAndCatVm = new ProductAndCategoriesViewModel()
            {
                Categories = _servicesCategory.GetAll(),
                Products = _servicesProduct.GetAll(),
            };

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                var cnt = _context.ShoppingCarts.Where(u => u.ApplicationUserId == claim.Value).ToList().Count;

                HttpContext.Session.SetInt32(Helper.ShoppingCartCount, cnt);
            }


            ViewBag.RecentProduct = _servicesProduct.GetAll().OrderByDescending(x => x.UpdateDate).Take(5).ToList();
            ViewBag.BestDiscount = _servicesProduct.GetAll().Where(x => (x.Discount - x.Price) > 1000).Take(5).OrderByDescending(x => x.Discount).ToList();



            return View(proAndCatVm);
        }

		[AllowAnonymous]
		[HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var product = _servicesProduct.FindBy(id);
			ViewBag.RelatedProducts = _servicesProduct.GetAll().Where(x => x.Category==product.Category).ToList();

			ShoppingCart cartObj = new ShoppingCart()
            {
                Product= product,
                ProductId= id
            };

            return View(cartObj);
        }



		// Add To Cart method 
		[Authorize]
		[HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Details")]
		public async Task<IActionResult> AddToCart(ShoppingCart shoppingCart,Guid? productId)
		{
            shoppingCart.Id = Guid.Empty;
			if (ModelState.IsValid)
			{
				var claimsIdentity = (ClaimsIdentity)this.User.Identity;
				var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);              
				shoppingCart.ApplicationUserId = claim.Value;

				var shoppingCartFromDb = await _context.ShoppingCarts
					.Where(c => c.ApplicationUserId == shoppingCart.ApplicationUserId && c.ProductId == shoppingCart.ProductId).FirstOrDefaultAsync();

				if (shoppingCartFromDb == null)
				{
					await _context.ShoppingCarts.AddAsync(shoppingCart);
				}
				else
				{
					shoppingCartFromDb.Count += shoppingCart.Count;
				}
				await _context.SaveChangesAsync();

				var count = _context.ShoppingCarts.Where(c => c.ApplicationUserId == shoppingCart.ApplicationUserId).ToList().Count();

				HttpContext.Session.SetInt32(Helper.ShoppingCartCount, count);

				return RedirectToAction("Index");
			}

			else
			{
                var product =await _context.Products.Include(x => x.Category).Include(x => x.Brand).Where(x => x.Id == shoppingCart.ProductId).FirstOrDefaultAsync();

				ShoppingCart cartObj = new ShoppingCart()
				{
					Product = product,
					ProductId = product.Id
				};

				return View(cartObj);
			}


		}



		[AllowAnonymous]
        public IActionResult ShopPage(int? page)
        {
            var products = _servicesProduct.GetAll().ToList().ToPagedList(pageNumber: page ?? 1, pageSize: 9);

            ViewBag.Categories = _servicesCategory.GetAll().ToList();
            ViewBag.Brands = _servicesBrand.GetAll().ToList();

            return View(products);
        }

        // Filter By Price
        [HttpPost]
        [AllowAnonymous]
        public IActionResult ShopPage(double? lowAmount, double? largeAmount,int? page)
        {
            var products = _servicesProduct.GetAll().Where(c => c.Price >= lowAmount && c.Price <= largeAmount).ToList().ToPagedList(pageNumber: page ?? 1, pageSize: 9);
            if (lowAmount == null || largeAmount == null)
            {
                products = _servicesProduct.GetAll().ToPagedList(pageNumber: page ?? 1, pageSize: 9);
            }

            ViewBag.Categories = _servicesCategory.GetAll().ToList();
            ViewBag.Brands = _servicesBrand.GetAll().ToList();

            return View(products);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Search(string? searchName,int? page)
        {

            if (searchName == null)
            {
                var products = _servicesProduct.GetAll().ToList();
                return View("ShopPage", products);
            }

            var product = _servicesProduct.GetAll().Where(c => c.Name.Contains(searchName) || c.Category.Name.Contains(searchName) || c.Brand.Name.Contains(searchName)).ToList().ToPagedList(pageNumber: page ?? 1, pageSize: 9);
            ViewBag.Categories = _servicesCategory.GetAll().ToList();
            ViewBag.Brands = _servicesBrand.GetAll().ToList();
            return View("ShopPage", product);
        }



        public IActionResult Denied()
        {
            return View();
        }





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}