using Domain.Entity;
using Infrastructure.Data;
using Infrastructure.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace WebApp.Areas.Customer.Controllers
{
	[Area("Customer")]
    [AllowAnonymous]
    public class OrdersController : Controller
	{
		private readonly ApplicationDbContext _db;
		public OrdersController(ApplicationDbContext db)
		{
			_db = db;
			
		}
       

        public async Task<IActionResult> Orders()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            List<OrderDetailsViewModel> orderDetailsVMList = new List<OrderDetailsViewModel>();

            List<OrderHeader> orderHeadersList = await _db.OrderHeaders.Include(x => x.ApplicationUser).ToListAsync();

            foreach (var orderHeader in orderHeadersList)
            {
                OrderDetailsViewModel orderDetailsVM = new OrderDetailsViewModel()
                {
                    OrderHeader = orderHeader,
                    OrderDetails = _db.OrderDetails.Where(x => x.OrderId == orderHeader.Id).ToList()
                };

                orderDetailsVMList.Add(orderDetailsVM);
            }

            return View(orderDetailsVMList);
        }



        public async Task<IActionResult> Confirm(int id)
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

			OrderDetailsViewModel orderDetailsVM = new OrderDetailsViewModel()
			{
				OrderHeader = await _db.OrderHeaders.Include(x => x.ApplicationUser).FirstOrDefaultAsync(m => m.UserId == claim.Value && m.Id == id),
				OrderDetails = await _db.OrderDetails.Where(x => x.OrderId == id).ToListAsync()
			};


			return View(orderDetailsVM);
		}


        public async Task<IActionResult> OrderHistory()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            List<OrderDetailsViewModel> orderDetailsVMList = new List<OrderDetailsViewModel>();

            List<OrderHeader> orderHeadersList = await _db.OrderHeaders.Include(x => x.ApplicationUser).Where(x => x.UserId == claim.Value).ToListAsync();

            foreach (var orderHeader in orderHeadersList)
            {
                OrderDetailsViewModel orderDetailsVM = new OrderDetailsViewModel()
                {
                    OrderHeader = orderHeader,
                    OrderDetails = _db.OrderDetails.Where(x => x.OrderId == orderHeader.Id).ToList()
                };

                orderDetailsVMList.Add(orderDetailsVM);
            }

            return View(orderDetailsVMList);
        }




		public async Task<IActionResult> GetOrderDetails(int id)
		{
			OrderDetailsViewModel orderDetailsViewModel = new OrderDetailsViewModel()
			{
				OrderHeader = await _db.OrderHeaders.Include(x => x.ApplicationUser).FirstOrDefaultAsync(m => m.Id == id),
				OrderDetails = await _db.OrderDetails.Where(m => m.OrderId == id).ToListAsync()
			};

			//orderDetailsViewModel.OrderHeader.ApplicationUser = await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == orderDetailsViewModel.OrderHeader.UserId);

			return PartialView("_IndividualOrderDetails", orderDetailsViewModel);
		}

		public IActionResult GetOrderStatus(int id)
		{
			return PartialView("_OrderStatus", _db.OrderHeaders.Where(m => m.Id == id).FirstOrDefault().Status);
		}



        public async Task<IActionResult> ManageOrder()
        {


            List<OrderDetailsViewModel> orderDetailsVMList = new List<OrderDetailsViewModel>();

            List<OrderHeader> orderHeadersList = await _db.OrderHeaders.Where(x => x.Status == Helper.StatusInProcess || x.Status == Helper.StatusSubmitted).ToListAsync();

            foreach (var orderHeader in orderHeadersList)
            {
                OrderDetailsViewModel orderDetailsVM = new OrderDetailsViewModel()
                {
                    OrderHeader = orderHeader,
                    OrderDetails = _db.OrderDetails.Where(x => x.OrderId == orderHeader.Id).ToList()
                };

                orderDetailsVMList.Add(orderDetailsVM);
            }

            return View(orderDetailsVMList.OrderByDescending(x => x.OrderHeader.PickUpDate).ToList());
        }



        
        public async Task<IActionResult> OrderPrepare(int OrderId)
        {
            OrderHeader orderHeader = await _db.OrderHeaders.FindAsync(OrderId);
            orderHeader.Status = Helper.StatusInProcess;
            await _db.SaveChangesAsync();
            return RedirectToAction("ManageOrder", "Orders");
        }

        public async Task<IActionResult> OrderReady(int OrderId)
        {
            OrderHeader orderHeader = await _db.OrderHeaders.FindAsync(OrderId);
            orderHeader.Status = Helper.StatusReady;
            await _db.SaveChangesAsync();
            //await _emailSender.SendEmailAsync(_db.Users.Where(u => u.Id == orderHeader.UserId).FirstOrDefault().Email, "Spice - Order Ready for Pickup " + orderHeader.Id.ToString(), "Order is ready for pickup.");

            return RedirectToAction("ManageOrder", "Orders");
        }

        public async Task<IActionResult> OrderCancel(int OrderId)
        {
            OrderHeader orderHeader = await _db.OrderHeaders.FindAsync(OrderId);
            orderHeader.Status = Helper.StatusCancelled;
            await _db.SaveChangesAsync();
            //await _emailSender.SendEmailAsync(_db.Users.Where(u => u.Id == orderHeader.UserId).FirstOrDefault().Email, "Spice - Order Cancelled " + orderHeader.Id.ToString(), "Order has been cancelled successfully.");

            return RedirectToAction("ManageOrder", "Orders");
        }


        [Authorize]
        public async Task<IActionResult> OrderPickup()
        {
            OrderListViewModel orderListVM = new OrderListViewModel()
            {
                Orders = new List<OrderDetailsViewModel>()
            };
            List<OrderHeader> orderHeadersList = new List<OrderHeader>();
            orderHeadersList = await _db.OrderHeaders.Include(o => o.ApplicationUser).Where(u => u.Status == Helper.StatusReady).ToListAsync();

            foreach (var orderHeader in orderHeadersList)
            {
                OrderDetailsViewModel individual = new OrderDetailsViewModel
                {
                    OrderHeader = orderHeader,
                    OrderDetails = await _db.OrderDetails.Where(x => x.OrderId == orderHeader.Id).ToListAsync()
                };

                orderListVM.Orders.Add(individual);
            }

            var count = orderListVM.Orders.Count;
            orderListVM.Orders = orderListVM.Orders.OrderByDescending(x => x.OrderHeader.Id).ToList();

            return View(orderListVM);
        }


        [HttpPost]
        [ActionName("OrderPickup")]
        public async Task<IActionResult> OrderPickupPost(int OrderId)
        {
            OrderHeader orderHeader = await _db.OrderHeaders.FindAsync(OrderId);
            orderHeader.Status = Helper.StatusCompleted;
            await _db.SaveChangesAsync();
            //await _emailSender.SendEmailAsync(_db.Users.Where(u => u.Id == orderHeader.UserId).FirstOrDefault().Email, "Spice - Order Completed " + orderHeader.Id.ToString(), "Order has been completed successfully.");

            return RedirectToAction("OrderPickup", "Orders");
        }
    }
}
