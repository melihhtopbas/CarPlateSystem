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
    public class GirisKapilariController : AdminBaseController
    {

        AracPlakaSistemiEntities _context = new AracPlakaSistemiEntities();
        private readonly GirisKapisiService _kapiService;
       
        public GirisKapilariController(GirisKapisiService kapiService)
        {
            _kapiService = kapiService;
        }
        
        public ActionResult Index()
        {
            ViewBag.Title = "Giriş Kapıları";
            return View("~/Views/GirisKapilari/Index.cshtml");
        }
        [AjaxOnly, HttpPost, ValidateInput(false)]

        public async Task<ActionResult> KapiList(GirisKapilariSearchViewModel model, int? page)
        {


            

            var currentPageIndex = page - 1 ?? 0;

            var result = _kapiService.GetGirisKapilariListIQueryable(model)
                .OrderBy(p => p.KapiAdres)
                .ToPagedList(currentPageIndex, SystemConstants.DefaultCarPageSize);

             

            ModelState.Clear();
           
            return new ContentResult
            {
                ContentType = "application/json",
                Content = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(new
                {
                    success = true,
                    responseText = RenderPartialViewToString("~/Views/GirisKapilari/KapiList.cshtml", result)
                })
            };

        }
        [AjaxOnly]
        [HttpGet]
        public ActionResult Add()
        {

             

            var model = new GirisKapilariAddViewModel
            {
                 
            };
            return PartialView("~/Views/GirisKapilari/_KapiEkle.cshtml", model);

        }
        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(GirisKapilariAddViewModel model)
        {
            

            if (ModelState.IsValid)
            {
                var callResult = await _kapiService.AddKapiAsync(model);
                if (callResult.Success)
                {

                    ModelState.Clear();
                    var viewModel = (GirisKapilariListViewModel)callResult.Item;
                    var jsonResult = Json(
                        new
                        {
                            success = true,
                            responseText = RenderPartialViewToString("~/Views/GirisKapilari/DisplayTemplates/GirisKapilariListViewModel.cshtml", viewModel),
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
                    responseText = RenderPartialViewToString("~/Views/GirisKapilari/_KapiEkle.cshtml", model)
                });

        }
        public async Task<ActionResult> Edit(int kapiId)
        {
            
            var model = await _kapiService.GetGirisKapilariEditViewModelAsync(kapiId);
            if (model != null)
            {

                return PartialView("~/Views/GirisKapilari/_KapiDuzenle.cshtml", model);
            }
            return PartialView("~/Views/Shared/_ItemNotFoundPartial.cshtml", "Araç sistemde bulunamadı!");
        }
        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(GirisKapilariEditViewModel model)
        {
             
            if (ModelState.IsValid)
            {
                var callResult = await _kapiService.GirisKapilariEditAracAsync(model);
                if (callResult.Success)
                {

                    ModelState.Clear();
                    var viewModel = (GirisKapilariListViewModel)callResult.Item;


                    var jsonResult = Json(
                        new
                        {
                            success = true,
                            responseText = RenderPartialViewToString("~/Views/GirisKapilari/DisplayTemplates/GirisKapilariListViewModel.cshtml", viewModel),
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
                    responseText = RenderPartialViewToString("~/Views/GirisKapilari/_KapiDuzenle.cshtml", model)
                });

        }
        [AjaxOnly, HttpPost]
        public async Task<ActionResult> Delete(int kapiId)
        {
            var callResult = await _kapiService.DeleteGirisKapilariAsync(kapiId);
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