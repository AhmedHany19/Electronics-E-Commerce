using Domain.Entity;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Customer.Controllers
{
	[Area("Customer")]
    [AllowAnonymous]

    public class ContactUsController : Controller
	{
        private readonly ApplicationDbContext _context;

        public ContactUsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Message()
		{
			return View();
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task <IActionResult> Message(ContactUs model)
        {
                    //model.Name = name;
                    //model.Email = email;
                    //model.Message = message;
                if (ModelState.IsValid)
                {
                    _context.ContactUs.Add(model);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
                }
                  
            
            return View(model);
        }

        public  async Task<IActionResult> GetAllMessages()
        {      
            return View(await _context.ContactUs.ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (_context.ContactUs == null || id == null)
            {
                return Problem("Entity set 'ApplicationDbContext.SpecialTags'  is null.");
            }

            var message = await _context.ContactUs.FindAsync(id);
            if (message != null)
            {
                _context.ContactUs.Remove(message);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(GetAllMessages));

        }

    }
}
