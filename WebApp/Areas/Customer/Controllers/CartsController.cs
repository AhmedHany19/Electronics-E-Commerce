using Domain.Entity;
using Infrastructure.Data;
using Infrastructure.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System.Security.Claims;

namespace WebApp.Areas.Customer.Controllers
{
	[Area("Customer")]
	[AllowAnonymous]
	public class CartsController : Controller
	{
		private readonly ApplicationDbContext _db;

		[BindProperty]
		public OrderDetailsCartViewModel OrderDetailsCartVM { get; set; }

		public CartsController(ApplicationDbContext db)
		{
			_db = db;			
		}


		// Carts Index 
		public async Task<IActionResult> Index()
		{

			OrderDetailsCartVM = new OrderDetailsCartViewModel()
			{
				OrderHeader = new OrderHeader()
			};
			OrderDetailsCartVM.OrderHeader.OrderTotal = 0;

			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
			var shoppingCart = _db.ShoppingCarts.Where(x => x.ApplicationUserId == claim.Value).ToList();


			if (shoppingCart != null)
			{
				OrderDetailsCartVM.ShoppingCarts = shoppingCart;
			}

			foreach (var item in OrderDetailsCartVM.ShoppingCarts)
			{
				item.Product = await _db.Products.FirstOrDefaultAsync(x => x.Id == item.ProductId);
				OrderDetailsCartVM.OrderHeader.OrderTotal += item.Product.Price * item.Count;
				item.Product.Description = Helper.ConvertToRawHtml(item.Product.Description);

				if (item.Product.Description.Length > 100)
				{
					item.Product.Description = item.Product.Description.Substring(0, 99) + "...";
				}
			}

			OrderDetailsCartVM.OrderHeader.OrderTotalOriginal = OrderDetailsCartVM.OrderHeader.OrderTotal;

			return View(OrderDetailsCartVM);
		}



		
		public async Task<IActionResult> RemoveFromCart(Guid cartId)
		{
			var cart = await _db.ShoppingCarts.FirstOrDefaultAsync(c => c.Id == cartId);

			_db.ShoppingCarts.Remove(cart);
			await _db.SaveChangesAsync();

			var count = _db.ShoppingCarts.Where(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count;
			HttpContext.Session.SetInt32(Helper.ShoppingCartCount, count);

			await _db.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

        public async Task<IActionResult> Plus(Guid cartId)
        {
            var cart = await _db.ShoppingCarts.FirstOrDefaultAsync(c => c.Id == cartId);
            cart.Count += 1;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Minus(Guid cartId)
        {
            var cart = await _db.ShoppingCarts.FirstOrDefaultAsync(c => c.Id == cartId);
            if (cart.Count == 1)
            {
                _db.ShoppingCarts.Remove(cart);
                await _db.SaveChangesAsync();

                var count = _db.ShoppingCarts.Where(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count;
                HttpContext.Session.SetInt32(Helper.ShoppingCartCount, count);
            }
            else
            {
                cart.Count -= 1;
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }




        public async Task<IActionResult> PlaceOrder()
        {
            OrderDetailsCartVM = new OrderDetailsCartViewModel()
            {
                OrderHeader = new OrderHeader()
            };
            OrderDetailsCartVM.OrderHeader.OrderTotal = 0;

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var appUser = _db.ApplicationUsers.Find(claim.Value);

            OrderDetailsCartVM.OrderHeader.PickUpName = appUser.Name;
            OrderDetailsCartVM.OrderHeader.PhoneNumber = appUser.PhoneNumber;
            OrderDetailsCartVM.OrderHeader.Email = appUser.Email;
            OrderDetailsCartVM.OrderHeader.OrderDate = DateTime.Now;


            var shoppingCart = _db.ShoppingCarts.Where(x => x.ApplicationUserId == claim.Value).ToList();
            if (shoppingCart != null)
            {
                OrderDetailsCartVM.ShoppingCarts = shoppingCart;
            }

            foreach (var item in OrderDetailsCartVM.ShoppingCarts)
            {
                item.Product = await _db.Products.FirstOrDefaultAsync(x => x.Id == item.ProductId);
                OrderDetailsCartVM.OrderHeader.OrderTotal += item.Product.Price * item.Count;

            }

            OrderDetailsCartVM.OrderHeader.OrderTotalOriginal = OrderDetailsCartVM.OrderHeader.OrderTotal;

            return View(OrderDetailsCartVM);
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ActionName("PlaceOrder")]
		public async Task<IActionResult> PlaceOrderPost(string? stripeToken)
		{


			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);




			OrderDetailsCartVM.ShoppingCarts = await _db.ShoppingCarts.Where(x => x.ApplicationUserId == claim.Value).ToListAsync();
			OrderDetailsCartVM.OrderHeader.PaymentStatus = Helper.PaymentStatusPending;
			OrderDetailsCartVM.OrderHeader.OrderDate = DateTime.Now;
			OrderDetailsCartVM.OrderHeader.UserId = claim.Value;
			OrderDetailsCartVM.OrderHeader.Status = Helper.PaymentStatusPending;
			OrderDetailsCartVM.OrderHeader.OrderTotalOriginal = 0;

			OrderDetailsCartVM.OrderHeader.PickUpDate = Convert.ToDateTime(
				OrderDetailsCartVM.OrderHeader.PickUpDate.ToShortDateString()
				+ " " +
				OrderDetailsCartVM.OrderHeader.PickUpDate.ToShortTimeString());

			await _db.OrderHeaders.AddAsync(OrderDetailsCartVM.OrderHeader);
			await _db.SaveChangesAsync();




			foreach (var item in OrderDetailsCartVM.ShoppingCarts)
			{
				item.Product = await _db.Products.FirstOrDefaultAsync(x => x.Id == item.ProductId);
				OrderDetail orderDetail = new OrderDetail()
				{
					ProductId = item.ProductId,
					OrderId = OrderDetailsCartVM.OrderHeader.Id,
					Description = item.Product.Description,
					Name = item.Product.Name,
					Price = item.Product.Price,
					Count = item.Count
				};

				OrderDetailsCartVM.OrderHeader.OrderTotalOriginal += item.Product.Price * item.Count;
				_db.Add(orderDetail);

			}


			


			


			_db.ShoppingCarts.RemoveRange(OrderDetailsCartVM.ShoppingCarts);
			HttpContext.Session.SetInt32(Helper.ShoppingCartCount, 0);
			await _db.SaveChangesAsync();


			// This part About Payment Method Stripe for Testing

			var options = new Stripe.ChargeCreateOptions
			{
				Amount = Convert.ToInt32(OrderDetailsCartVM.OrderHeader.OrderTotalOriginal * 100),
				Currency = "usd",
				Description = "Order ID : " + OrderDetailsCartVM.OrderHeader.Id,
				Source = stripeToken

			};



			var service = new ChargeService();
			Charge charge = service.Create(options);

			if (charge.BalanceTransactionId == null)
			{
				OrderDetailsCartVM.OrderHeader.PaymentStatus = Helper.PaymentStatusRejected;
			}
			else
			{
				OrderDetailsCartVM.OrderHeader.TransactionId = charge.BalanceTransactionId;
			}

			if (charge.Status.ToLower() == "succeeded")
			{
				//await _emailSender.SendEmailAsync(_db.Users.Where(u => u.Id == claim.Value).FirstOrDefault().Email, "Spice - Order Created " + OrderDetailsCartVM.OrderHeader.Id.ToString(), "Order has been submitted successfully.");

				OrderDetailsCartVM.OrderHeader.PaymentStatus = Helper.PaymentStatusApproved;
				OrderDetailsCartVM.OrderHeader.Status = Helper.StatusSubmitted;
			}
			else
			{
				OrderDetailsCartVM.OrderHeader.PaymentStatus = Helper.PaymentStatusRejected;
			}
			await _db.SaveChangesAsync();
			return RedirectToAction("Confirm", "Orders", new { id = OrderDetailsCartVM.OrderHeader.Id });
		}


	}
}
