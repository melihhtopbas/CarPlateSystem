﻿using AracPlakaSistemi.Controllers.Abstract;
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
    public class KaraListeAracController : AdminBaseController
    {
        private readonly KaraListeAracService _karaListeAracService;
        private readonly KayitliAracService _kayitliAracService;

        public KaraListeAracController(KaraListeAracService karaListeAracService, KayitliAracService kayitliAracService)
        {
            _karaListeAracService = karaListeAracService;
            _kayitliAracService = kayitliAracService;
        }

        public ActionResult Index(string plaka)
        {

            ViewBag.Title = "Kara Liste Araçlar";
            return View("~/Views/KaraListeArac/Index.cshtml", new KaraListeAracSearchViewModel { Plaka = plaka});
        }
        [AjaxOnly, HttpPost, ValidateInput(false)]

        public async Task<ActionResult> AracList(KaraListeAracSearchViewModel model, int? page)
        {




            var currentPageIndex = page - 1 ?? 0;

            var result = _karaListeAracService.GetKaraListeAracListIQueryable(model)
                .OrderBy(p => p.Plaka)
                .ToPagedList(currentPageIndex, SystemConstants.DefaultCarPageSize);



            ModelState.Clear();

            return new ContentResult
            {
                ContentType = "application/json",
                Content = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(new
                {
                    success = true,
                    responseText = RenderPartialViewToString("~/Views/KaraListeArac/AracList.cshtml", result)
                })
            };

        }
        [AjaxOnly]
        [HttpGet]
        public ActionResult Add()
        {


            ViewData["AracListesi"] = _karaListeAracService.GetAracList().ToList();
            var model = new KaraListeAracAddViewModel
            {

            };
            return PartialView("~/Views/KaraListeArac/_AracEkle.cshtml", model);

        }
        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(KaraListeAracAddViewModel model)
        {
            

            if (ModelState.IsValid)
            {
                var callResult = await _karaListeAracService.AddCarAsync(model);
                if (callResult.Success)
                {

                    ModelState.Clear();
                    var viewModel = (KaraListeAracListViewModel)callResult.Item;
                    var jsonResult = Json(
                        new
                        {
                            success = true,
                            responseText = RenderPartialViewToString("~/Views/KaraListeArac/DisplayTemplates/KaraListeAracListViewModel.cshtml", viewModel),
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
                    responseText = RenderPartialViewToString("~/Views/KaraListeArac/_AracEkle.cshtml", model)
                });

        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            var callResult = await _karaListeAracService.DeleteKaraListeAracAsync(id);
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