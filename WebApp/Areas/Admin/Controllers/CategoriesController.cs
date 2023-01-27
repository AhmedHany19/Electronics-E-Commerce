using Domain.Constants;
using Domain.Entity;
using Infrastructure.IRepository;
using Infrastructure.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
	[Area(Helper.Admin)]
	public class CategoriesController : Controller
	{
		private readonly IServicesRepository<Category> _servicesCategory;
		private readonly IServicesRepositoryLog<LogCategory> _servicesLogCategory;
		private readonly UserManager<ApplicationUser> _userManager;

		public CategoriesController(IServicesRepository<Category> servicesCategory,
			IServicesRepositoryLog<LogCategory> servicesLogCategory,
			UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
			_servicesLogCategory = servicesLogCategory;
			_servicesCategory = servicesCategory;
		}

        [Authorize(Permissions.Categories.View)]
        public IActionResult Categories()
		{
			return View(new CategoryViewModel
			{
				Categories = _servicesCategory.GetAll(),
				LogCategories = _servicesLogCategory.GetAll(),
				NewCategory = new Category()
			});
		}

        [Authorize(Permissions.Categories.Delete)]
        public IActionResult Delete(Guid Id)
		{
			var category = _servicesCategory.FindBy(Id);
			if (category.ImageUrl != null && category.ImageUrl != Guid.Empty.ToString())
			{
				var PathImage = Path.Combine(@"wwwroot/", Helper.PathSaveImageCategory, category.ImageUrl);
				if (System.IO.File.Exists(PathImage))
					System.IO.File.Delete(PathImage);
			}

			var userId = _userManager.GetUserId(User);
			if (_servicesCategory.Delete(Id) && _servicesLogCategory.Delete(Id, Guid.Parse(userId)))
				return RedirectToAction(nameof(Categories));

			return RedirectToAction(nameof(Categories));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Permissions.Categories.Create)]

        public IActionResult Save(CategoryViewModel model)
		{

			var file = HttpContext.Request.Form.Files;
			if (file.Count() > 0)
			{
				string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(file[0].FileName);
				var fileStream = new FileStream(Path.Combine(@"wwwroot/", Helper.PathSaveImageCategory, ImageName), FileMode.Create);
				file[0].CopyTo(fileStream);
				model.NewCategory.ImageUrl = ImageName;
			}
			else
			{
				model.NewCategory.ImageUrl = model.NewCategory.ImageUrl;
			}


			if (ModelState.IsValid)
			{
				var userId = _userManager.GetUserId(User);
				if (model.NewCategory.Id == Guid.Parse(Guid.Empty.ToString()))
				{
					// Create
					if (_servicesCategory.FindBy(model.NewCategory.Name) != null)
						SessionMsg(Helper.Error, Resource.ResourceWeb.lbNotSaved, Resource.ResourceWeb.lbMsgDuplicateNameCategory);
					else
					{
						if (_servicesCategory.Save(model.NewCategory) && _servicesLogCategory.Save(model.NewCategory.Id, Guid.Parse(userId)))
							SessionMsg(Helper.Success, Resource.ResourceWeb.lbSave, Resource.ResourceWeb.lbMsgSaveCategory);
						else
							SessionMsg(Helper.Error, Resource.ResourceWeb.lbNotSaved, Resource.ResourceWeb.lbMsgNotSavedCategory);
					}

				}
				else

				{
					//Update

					var OldPath = _servicesCategory.FindBy(model.NewCategory.Id);
					if (OldPath.ImageUrl != null && OldPath.ImageUrl != Guid.Empty.ToString())
					{
						var PathImage = Path.Combine(@"wwwroot/", Helper.PathSaveImageCategory, OldPath.ImageUrl);
						if (System.IO.File.Exists(PathImage))
							System.IO.File.Delete(PathImage);
					}
					if (_servicesCategory.Save(model.NewCategory) && _servicesLogCategory.Update(model.NewCategory.Id, Guid.Parse(userId)))
						SessionMsg(Helper.Success, Resource.ResourceWeb.lbUpdate, Resource.ResourceWeb.lbMsgUpdateCategory);
					else
						SessionMsg(Helper.Error, Resource.ResourceWeb.lbNotUpdate, Resource.ResourceWeb.lbMsgNotUpdatedCategory);
				}
			}

			return RedirectToAction(nameof(Categories));

		}

		public IActionResult DeleteLog(Guid Id)
		{
			if (_servicesLogCategory.DeleteLog(Id))
				return RedirectToAction(nameof(Categories));
			return RedirectToAction(nameof(Categories));
		}










		private void SessionMsg(string MsgType, string Title, string Msg)
		{
			HttpContext.Session.SetString(Helper.MsgType, MsgType);
			HttpContext.Session.SetString(Helper.Title, Title);
			HttpContext.Session.SetString(Helper.Msg, Msg);
		}
	}
}
