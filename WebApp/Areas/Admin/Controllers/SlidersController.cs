using Domain.Constants;
using Domain.Entity;
using Infrastructure.IRepository;
using Infrastructure.IRepository.ServicesRepository;
using Infrastructure.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static Domain.Constants.Permissions;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SlidersController : Controller
    {
        private readonly IServicesRepository<Slider> _servicesSlider;
        private readonly UserManager<ApplicationUser> _userManager;

        public SlidersController(IServicesRepository<Slider> servicesSlider,
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _servicesSlider = servicesSlider;
        }


        [Authorize(Permissions.Sliders.View)]
        public IActionResult Sliders()
        {

            return View(new SliderViewModel()
            {
                Sliders = _servicesSlider.GetAll(),
                NewSlider = new Slider()
            });
        }
        [Authorize(Permissions.Sliders.Delete)]
        public IActionResult Delete(Guid Id)
        {
            //the Fitst Image 
            var slider = _servicesSlider.FindBy(Id);
            if (slider.ImageUrl != null && slider.ImageUrl != Guid.Empty.ToString())
            {
                var PathImage = Path.Combine(@"wwwroot/", Helper.PathSaveImageSlider, slider.ImageUrl);
                if (System.IO.File.Exists(PathImage))
                    System.IO.File.Delete(PathImage);
            }
            var userId = _userManager.GetUserId(User);
            if (_servicesSlider.Delete(Id))
                return RedirectToAction(nameof(Sliders));

            return RedirectToAction(nameof(Sliders));
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Permissions.Sliders.Create)]

        public IActionResult Save(SliderViewModel model)
        {

            var file = HttpContext.Request.Form.Files;

            if (file.Count() > 0)
            {
                string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(file[0].FileName);
                var fileStream = new FileStream(Path.Combine(@"wwwroot/", Helper.PathSaveImageSlider, ImageName), FileMode.Create);
                file[0].CopyTo(fileStream);
                model.NewSlider.ImageUrl = ImageName;

            }
            else
            {
                model.NewSlider.ImageUrl = model.NewSlider.ImageUrl;
            }
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                if (model.NewSlider.Id == Guid.Parse(Guid.Empty.ToString()))
                {
                    // Create
                    if (_servicesSlider.FindBy(model.NewSlider.Name) != null)
                        SessionMsg(Helper.Error, Resource.ResourceWeb.lbNotSaved, Resource.ResourceWeb.lbMsgDuplicateNameCategory);
                    else
                    {
                        if (_servicesSlider.Save(model.NewSlider))
                            SessionMsg(Helper.Success, Resource.ResourceWeb.lbSave, Resource.ResourceWeb.lbMsgSaveCategory);
                        else
                            SessionMsg(Helper.Error, Resource.ResourceWeb.lbNotSaved, Resource.ResourceWeb.lbMsgNotSavedCategory);
                    }
                }
                else
                {
                    //Update

                    var OldPath = _servicesSlider.FindBy(model.NewSlider.Id);
                    //if (OldPath.ImageUrl != null && OldPath.ImageUrl != Guid.Empty.ToString())
                    //{
                    //    var PathImage = Path.Combine(@"wwwroot/", Helper.PathSaveImageCategory, OldPath.ImageUrl);
                    //    if (System.IO.File.Exists(PathImage))
                    //        System.IO.File.Delete(PathImage);
                    //}



                    if (_servicesSlider.Save(model.NewSlider))
                        SessionMsg(Helper.Success, Resource.ResourceWeb.lbUpdate, Resource.ResourceWeb.lbMsgUpdateCategory);
                    else
                        SessionMsg(Helper.Error, Resource.ResourceWeb.lbNotUpdate, Resource.ResourceWeb.lbMsgNotUpdatedCategory);
                }
            }
            return RedirectToAction(nameof(Sliders));
        }


        private void SessionMsg(string MsgType, string Title, string Msg)
        {
            HttpContext.Session.SetString(Helper.MsgType, MsgType);
            HttpContext.Session.SetString(Helper.Title, Title);
            HttpContext.Session.SetString(Helper.Msg, Msg);
        }
    }


}
