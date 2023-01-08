using Domain.Entity;
using Infrastructure.IRepository;
using Infrastructure.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ProductsController : Controller
	{
		private readonly IServicesRepository<Product> _servicesProduct;
		private readonly IServicesRepositoryLog<LogProduct> _servicesLogProduct;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IServicesRepository<Category> _servicesCategory;
		private readonly IServicesRepository<Brand> _servicesBrand;


		public ProductsController(IServicesRepository<Brand> servicesBrand,
			IServicesRepositoryLog<LogProduct> servicesLogProduct,
			UserManager<ApplicationUser> userManager,
			IServicesRepository<Category> servicesCategory,
			IServicesRepository<Product> servicesProduct)
		{
			_userManager = userManager;
			_servicesBrand = servicesBrand;
			_servicesCategory = servicesCategory;
			_servicesProduct = servicesProduct;
			_servicesLogProduct = servicesLogProduct;


		}
		public IActionResult Products()
		{
			return View(new ProductViewModel
			{
				Brands = _servicesBrand.GetAll(),
				Categories = _servicesCategory.GetAll(),
				LogProducts = _servicesLogProduct.GetAll(),
				NewProduct = new Product(),
				Products = _servicesProduct.GetAll(),
			});
		}

		public IActionResult Delete(Guid Id)
		{
			var product = _servicesProduct.FindBy(Id);
			if (product.ImageUrl != null && product.ImageUrl != Guid.Empty.ToString())
			{
				var PathImage = Path.Combine(@"wwwroot/", Helper.PathSaveImageProduct, product.ImageUrl);
				if (System.IO.File.Exists(PathImage))
					System.IO.File.Delete(PathImage);
			}

			var userId = _userManager.GetUserId(User);
			if (_servicesProduct.Delete(Id) && _servicesLogProduct.Delete(Id, Guid.Parse(userId)))
				return RedirectToAction(nameof(Products));

			return RedirectToAction(nameof(Products));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[AllowAnonymous]
		public IActionResult Save(ProductViewModel model)
		{
			var file = HttpContext.Request.Form.Files;
			if (file.Count() > 0)
			{
				string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(file[0].FileName);
				var fileStream = new FileStream(Path.Combine(@"wwwroot/", Helper.PathSaveImageProduct, ImageName), FileMode.Create);
				file[0].CopyTo(fileStream);
				model.NewProduct.ImageUrl = ImageName;
			}
			else
			{
				model.NewProduct.ImageUrl = model.NewProduct.ImageUrl;
			}

			if (ModelState.IsValid)
			{
				var userId = _userManager.GetUserId(User);
				if (model.NewProduct.Id == Guid.Parse(Guid.Empty.ToString()))
				{
					//Create
					if (_servicesBrand.FindBy(model.NewProduct.Name) != null)
						SessionMsg(Helper.Error, Resource.ResourceWeb.lbNotSaved, Resource.ResourceWeb.lbMsgDuplicateNameProduct);
					else
					{
						if (_servicesProduct.Save(model.NewProduct) && _servicesLogProduct.Save(model.NewProduct.Id, Guid.Parse(userId)))
							SessionMsg(Helper.Success, Resource.ResourceWeb.lbSave, Resource.ResourceWeb.lbMsgSaveProduct);
						else
							SessionMsg(Helper.Error, Resource.ResourceWeb.lbNotSaved, Resource.ResourceWeb.lbMsgNotSavedProduct);
					}
				}
				else//Update
				{
					var OldPath = _servicesProduct.FindBy(model.NewProduct.Id);
					if (OldPath.ImageUrl != null && OldPath.ImageUrl != Guid.Empty.ToString())
					{
						var PathImage = Path.Combine(@"wwwroot/", Helper.PathSaveImageuser, OldPath.ImageUrl);
						if (System.IO.File.Exists(PathImage))
							System.IO.File.Delete(PathImage);
					}

					if (_servicesProduct.Save(model.NewProduct) && _servicesLogProduct.Update(model.NewProduct.Id, Guid.Parse(userId)))
						SessionMsg(Helper.Success, Resource.ResourceWeb.lbUpdate, Resource.ResourceWeb.lbMsgUpdateProduct);
					else
						SessionMsg(Helper.Error, Resource.ResourceWeb.lbNotUpdate, Resource.ResourceWeb.lbMsgNotUpdatedProduct);
				}

				//ViewData["CategoryId"] = new SelectList(_servicesCategory.GetAll(), "Id", "Name", model.Categories);
			}
			return RedirectToAction(nameof(Products));

		}




		public IActionResult DeleteLog(Guid id)
		{
			if (_servicesLogProduct.DeleteLog(id))
				return RedirectToAction(nameof(Products));
			return RedirectToAction(nameof(Products));
		}





		private void SessionMsg(string MsgType, string Title, string Msg)
		{
			HttpContext.Session.SetString(Helper.MsgType, MsgType);
			HttpContext.Session.SetString(Helper.Title, Title);
			HttpContext.Session.SetString(Helper.Msg, Msg);
		}
	}
}
