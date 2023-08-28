using AracPlakaSistemi.Controllers.Abstract;
using AracPlakaSistemi.Data;
using AracPlakaSistemi.Extensions;
using AracPlakaSistemi.Service.Admin;
using AracPlakaSistemi.ViewModels.Admin;
using ImageResizer;
using Microsoft.Web.Mvc;
using MvcPaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace AracPlakaSistemi.Controllers
{
    [Authorize]
    public class KayitliAracController : AdminBaseController
    {

        AracPlakaSistemiEntities _context = new AracPlakaSistemiEntities();
        private readonly KayitliAracService _kayitliAracService;
       
        public KayitliAracController(KayitliAracService kayitliAracService)
        {
                _kayitliAracService = kayitliAracService;
        }
        
        public ActionResult Index(string plaka)
        {
            ViewBag.Title = "Kayıtlı Araçlar";
            return View("~/Views/KayitliArac/Index.cshtml", new KayitliAracSearchViewModel { Plaka = plaka});
        }
        [AjaxOnly, HttpPost, ValidateInput(false)]

        public async Task<ActionResult> AracList(KayitliAracSearchViewModel model, int? page)
        {


            

            var currentPageIndex = page - 1 ?? 0;

            var result = _kayitliAracService.GetKayitliAracListIQueryable(model)
                .OrderBy(p => p.Ad)
                .ToPagedList(currentPageIndex, SystemConstants.DefaultCarPageSize);

             

            ModelState.Clear();
           
            return new ContentResult
            {
                ContentType = "application/json",
                Content = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(new
                {
                    success = true,
                    responseText = RenderPartialViewToString("~/Views/KayitliArac/AracList.cshtml", result)
                })
            };

        }
        [AjaxOnly]
        [HttpGet]
        public ActionResult Add()
        {

             

            var model = new KayitliAracAddViewModel
            {
                 
            };
            return PartialView("~/Views/KayitliArac/_AracEkle.cshtml", model);

        }
        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(KayitliAracAddViewModel model)
        {
            HttpFileCollectionBase Files = Request.Files;
            HttpPostedFileBase ImageFile = Files[0];
            if (ImageFile != null && ImageFile.ContentLength > 0)
            {
                var tempImageDirectory = System.IO.Path.Combine(Server.MapPath(SystemConstants.GalleryImagePath));
                var tempImageThumbDirectory = System.IO.Path.Combine(Server.MapPath(SystemConstants.GalleryImageThumbPath));

                if (!System.IO.Directory.Exists(tempImageDirectory))
                    System.IO.Directory.CreateDirectory(tempImageDirectory);

                if (!System.IO.Directory.Exists(tempImageThumbDirectory))
                    System.IO.Directory.CreateDirectory(tempImageThumbDirectory);

                string fileName = $"{Guid.NewGuid().ToString("N")}{Path.GetExtension(ImageFile.FileName)}";

                string pathImage = System.IO.Path.Combine(tempImageDirectory, fileName);
                string pathImageThumb = System.IO.Path.Combine(tempImageThumbDirectory, fileName);

                ImageBuilder.Current.Build(ImageFile, pathImage, new ResizeSettings(SystemConstants.ImageResizerServiceImageSettings));
                ImageBuilder.Current.Build(ImageFile, pathImageThumb, new ResizeSettings(SystemConstants.ImageResizerServiceThumbImageSettings));
                model.FileName = "\\thumbs\\" + fileName;
                ImageFile.SaveAs(Server.MapPath(model.FileName));
                
            }

            if (ModelState.IsValid)
            {
                var callResult = await _kayitliAracService.AddCarAsync(model);
                if (callResult.Success)
                {

                    ModelState.Clear();
                    var viewModel = (KayitliAracListViewModel)callResult.Item;
                    var jsonResult = Json(
                        new
                        {
                            success = true,
                            responseText = RenderPartialViewToString("~/Views/KayitliArac/DisplayTemplates/KayitliAracListViewModel.cshtml", viewModel),
                            //item = viewModel
                        });
                    jsonResult.MaxJsonLength = int.MaxValue;
                    return jsonResult;
                }
                foreach (var error in callResult.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
                foreach (var item in callResult.WarningMessages)
                {
                    ModelState.AddModelError("", item);
                }
            }

            return Json(
                new
                {
                    success = false,
                    responseText = RenderPartialViewToString("~/Views/KayitliArac/_AracEkle.cshtml", model)
                });

        }
        public async Task<ActionResult> Edit(int carId)
        {
            
            var model = await _kayitliAracService.GetAracEditViewModelAsync(carId);
            if (model != null)
            {

                return PartialView("~/Views/KayitliArac/_AracDuzenle.cshtml", model);
            }
            return PartialView("~/Views/Shared/_ItemNotFoundPartial.cshtml", "Araç sistemde bulunamadı!");
        }
        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(KayitliAracAEditViewModel model)
        {
             
            if (ModelState.IsValid)
            {
                var callResult = await _kayitliAracService.EditAracAsync(model);
                if (callResult.Success)
                {

                    ModelState.Clear();
                    var viewModel = (KayitliAracListViewModel)callResult.Item;


                    var jsonResult = Json(
                        new
                        {
                            success = true,
                            responseText = RenderPartialViewToString("~/Views/KayitliArac/DisplayTemplates/KayitliAracListViewModel.cshtml", viewModel),
                            item = viewModel
                        });
                    jsonResult.MaxJsonLength = int.MaxValue;
                    return jsonResult;
                }
                foreach (var error in callResult.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
            }

            return Json(
                new
                {
                    success = false,
                    responseText = RenderPartialViewToString("~/Views/KayitliArac/_AracDuzenle.cshtml", model)
                });

        }
        [AjaxOnly, HttpPost]
        public async Task<ActionResult> Delete(int aracId)
        {
            var callResult = await _kayitliAracService.DeleteKayitliAracAsync(aracId);
            if (callResult.Success)
            {

                ModelState.Clear();

                return Json(
                    new
                    {
                        success = true,
                        warningMessages = callResult.WarningMessages,
                        successMessages = callResult.SuccessMessages,
                    });
            }

            return Json(
                new
                {
                    success = false,
                    errorMessages = callResult.ErrorMessages
                });

        }
        [AjaxOnly]
        [HttpGet]
        public async Task<ActionResult> PlakaGoster(int carId)
        {


            var plaka = await _kayitliAracService.GetKayitliAracPlaka(carId);
            if (plaka.PathName == null)
            {
                return PartialView("~/Views/Shared/_ItemNotFoundPartial.cshtml", "Araç plaka resmi eklenmedi!");
            }
            
            return PartialView("~/Views/KayitliArac/_AracPlaka.cshtml", plaka);

        }


    }
}