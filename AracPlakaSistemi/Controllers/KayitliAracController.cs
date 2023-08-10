using AracPlakaSistemi.Controllers.Abstract;
using AracPlakaSistemi.Data;
using AracPlakaSistemi.Extensions;
using AracPlakaSistemi.Service.Admin;
using AracPlakaSistemi.ViewModels.Admin;
using Microsoft.Web.Mvc;
using MvcPaging;
using System;
using System.Collections.Generic;
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
        
        public ActionResult Index()
        {
            ViewBag.Title = "Kayıtlı Araçlar";
            return View("~/Views/KayitliArac/Index.cshtml");
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
            }

            return Json(
                new
                {
                    success = false,
                    responseText = RenderPartialViewToString("~/Views/KayitliArac/_AracEkle.cshtml", model)
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


    }
}