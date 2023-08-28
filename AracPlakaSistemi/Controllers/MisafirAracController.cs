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
    public class MisafirAracController : AdminBaseController
    {

        AracPlakaSistemiEntities _context = new AracPlakaSistemiEntities();
        private readonly MisafirAracService _misafirAracService;
       
        public MisafirAracController(MisafirAracService misafirAracService)
        {
            _misafirAracService = misafirAracService;
        }
        
        public ActionResult Index(string plaka)
        {
            ViewBag.Title = "Misafir Araçlar";
            return View("~/Views/MisafirArac/Index.cshtml", new MisafirAracSearchViewModel { Plaka = plaka});
        }
        [AjaxOnly, HttpPost, ValidateInput(false)]

        public async Task<ActionResult> AracList(MisafirAracSearchViewModel model, int? page)
        {


            

            var currentPageIndex = page - 1 ?? 0;

            var result = _misafirAracService.GetMisafirAracListIQueryable(model)
                .OrderBy(p => p.Ad)
                .ToPagedList(currentPageIndex, SystemConstants.DefaultCarPageSize);

             

            ModelState.Clear();
           
            return new ContentResult
            {
                ContentType = "application/json",
                Content = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(new
                {
                    success = true,
                    responseText = RenderPartialViewToString("~/Views/MisafirArac/AracList.cshtml", result)
                })
            };

        }
        [AjaxOnly]
        [HttpGet]
        public ActionResult Add()
        {

             

            var model = new MisafirAracAddViewModel
            {
                 
            };
            return PartialView("~/Views/MisafirArac/_AracEkle.cshtml", model);

        }
        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(MisafirAracAddViewModel model)
        {
            

            if (ModelState.IsValid)
            {
                var callResult = await _misafirAracService.AddCarAsync(model);
                if (callResult.Success)
                {

                    ModelState.Clear();
                    var viewModel = (MisafirAracListViewModel)callResult.Item;
                    var jsonResult = Json(
                        new
                        {
                            success = true,
                            responseText = RenderPartialViewToString("~/Views/MisafirArac/DisplayTemplates/MisafirAracListViewModel.cshtml", viewModel),
                            //item = viewModel
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
                    responseText = RenderPartialViewToString("~/Views/MisafirArac/_AracEkle.cshtml", model)
                });

        }
        public async Task<ActionResult> Edit(int carId)
        {
            
            var model = await _misafirAracService.GetMisafirAracEditViewModelAsync(carId);
            if (model != null)
            {

                return PartialView("~/Views/MisafirArac/_AracDuzenle.cshtml", model);
            }
            return PartialView("~/Views/Shared/_ItemNotFoundPartial.cshtml", "Araç sistemde bulunamadı!");
        }
        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(MisafirAracEditViewModel model)
        {
             
            if (ModelState.IsValid)
            {
                var callResult = await _misafirAracService.MisafirAracEditAracAsync(model);
                if (callResult.Success)
                {

                    ModelState.Clear();
                    var viewModel = (MisafirAracListViewModel)callResult.Item;


                    var jsonResult = Json(
                        new
                        {
                            success = true,
                            responseText = RenderPartialViewToString("~/Views/MisafirArac/DisplayTemplates/MisafirAracListViewModel.cshtml", viewModel),
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
                    responseText = RenderPartialViewToString("~/Views/MisafirArac/_AracDuzenle.cshtml", model)
                });

        }
        [AjaxOnly, HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            var callResult = await _misafirAracService.DeleteMisafirAracAsync(id);
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
        [AjaxOnly, HttpPost]
        public async Task<ActionResult> AracKaydıYap(int id)
        {
            var callResult = await _misafirAracService.AracKaydıYapAsync(id);
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



    }
}