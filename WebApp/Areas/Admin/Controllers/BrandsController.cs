using Domain.Constants;
using Domain.Entity;
using Infrastructure.IRepository;
using Infrastructure.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class BrandsController : Controller
	{
		private readonly IServicesRepository<Brand> _servicesBrand;
		private readonly IServicesRepositoryLog<LogBrand> _servicesLogBrand;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IServicesRepository<Category> _servicesCategory;




		public BrandsController(IServicesRepository<Brand> servicesBrand,
			IServicesRepositoryLog<LogBrand> servicesLogBrand,
			UserManager<ApplicationUser> userManager, IServicesRepository<Category> servicesCategory)
		{
			_userManager = userManager;
			_servicesLogBrand = servicesLogBrand;
			_servicesBrand = servicesBrand;
			_servicesCategory = servicesCategory;


		}

        [Authorize(Permissions.Brands.View)]
        public IActionResult Brands()
		{
			//ViewData["CategoryId"] = new SelectList(_servicesCategory.GetAll(), "Id", "Name");

			return View(new BrandViewModel
			{
				Brands = _servicesBrand.GetAll(),
				LogBrands = _servicesLogBrand.GetAll(),
				NewBrand = new Brand(),
				Categories = _servicesCategory.GetAll(),

			});
		}

        [Authorize(Permissions.Brands.Delete)]

        public IActionResult Delete(Guid Id)
		{
			var userId = _userManager.GetUserId(User);
			if (_servicesBrand.Delete(Id) && _servicesLogBrand.Delete(Id, Guid.Parse(userId)))
				return RedirectToAction(nameof(Brands));

			return RedirectToAction(nameof(Brands));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[AllowAnonymous]
        [Authorize(Permissions.Brands.Create)]

        public IActionResult Save(BrandViewModel model)
		{

			if (ModelState.IsValid)
			{
				var userId = _userManager.GetUserId(User);
				if (model.NewBrand.Id == Guid.Parse(Guid.Empty.ToString()))
				{
					//Create
					if (_servicesBrand.FindBy(model.NewBrand.Name) != null)
						SessionMsg(Helper.Error, Resource.ResourceWeb.lbNotSaved, Resource.ResourceWeb.lbMsgDuplicateNameBrand);
					else
					{
						if (_servicesBrand.Save(model.NewBrand) && _servicesLogBrand.Save(model.NewBrand.Id, Guid.Parse(userId)))
							SessionMsg(Helper.Success, Resource.ResourceWeb.lbSave, Resource.ResourceWeb.lbMsgSaveBrand);
						else
							SessionMsg(Helper.Error, Resource.ResourceWeb.lbNotSaved, Resource.ResourceWeb.lbMsgNotSavedBrand);
					}
				}
				else//Update
				{
					if (_servicesBrand.Save(model.NewBrand) && _servicesLogBrand.Update(model.NewBrand.Id, Guid.Parse(userId)))
						SessionMsg(Helper.Success, Resource.ResourceWeb.lbUpdate, Resource.ResourceWeb.lbMsgUpdateBrand);
					else
						SessionMsg(Helper.Error, Resource.ResourceWeb.lbNotUpdate, Resource.ResourceWeb.lbMsgNotUpdatedBrand);
				}

				//ViewData["CategoryId"] = new SelectList(_servicesCategory.GetAll(), "Id", "Name", model.Categories);
			}
			return RedirectToAction(nameof(Brands));

		}

		public IActionResult DeleteLog(Guid id)
		{
			if (_servicesLogBrand.DeleteLog(id))
				return RedirectToAction(nameof(Brands));
			return RedirectToAction(nameof(Brands));
		}



		private void SessionMsg(string MsgType, string Title, string Msg)
		{
			HttpContext.Session.SetString(Helper.MsgType, MsgType);
			HttpContext.Session.SetString(Helper.Title, Title);
			HttpContext.Session.SetString(Helper.Msg, Msg);
		}
	}
}
