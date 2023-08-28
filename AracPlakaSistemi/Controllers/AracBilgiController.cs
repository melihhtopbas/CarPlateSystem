using AracPlakaSistemi.Controllers.Abstract;
using AracPlakaSistemi.Service.Admin;
using AracPlakaSistemi.ViewModels.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AracPlakaSistemi.Controllers
{
    public class AracBilgiController : AdminBaseController
    {
        private readonly AracBilgiService _aracBilgiService;

        public AracBilgiController(AracBilgiService aracBilgiService)
        {
            _aracBilgiService = aracBilgiService;
        }
        public ActionResult Index()
        {
            ViewBag.Title = "Araç Bilgisi Sorgulama";
            return View("~/Views/AracBilgi/Index.cshtml");
        }
        [HttpGet]
        //Fiyat hesaplama modal'i
        public async Task<ActionResult> AracBilgisiSorgula()
        {
            var model = new AracBilgiSorguViewModel
            { 

            };
           
            return PartialView("~/Views/AracBilgi/_AracBilgiSorgula.cshtml", model);



        }
        [HttpPost]
        //Fiyat hesaplama modal'i
        public async Task<ActionResult> AracBilgisiSorgula(AracBilgiSorguViewModel model)
        {
            

            if (ModelState.IsValid)
            {
                var callResult = await _aracBilgiService.AracBilgileriSorgula(model);
                if (callResult.Success)
                {

                    ModelState.Clear();
                    var viewModel = model;
                    var jsonResult = Json(
                        new
                        {

                            responseText = RenderPartialViewToString("~/Views/AracBilgi/_AracBilgiSorgula.cshtml", viewModel),
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
                   responseText = RenderPartialViewToString("~/Views/AracBilgi/_AracBilgiSorgula.cshtml", model)
               });




        }
    }
}