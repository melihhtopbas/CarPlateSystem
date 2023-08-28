using AracPlakaSistemi.Controllers.Abstract;
using AracPlakaSistemi.Data;
using AracPlakaSistemi.Extensions;
using AracPlakaSistemi.Service.Admin;
using AracPlakaSistemi.ViewModels.Admin;
using Microsoft.Web.Mvc;
using MvcPaging;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace AracPlakaSistemi.Controllers
{
    public class GirisYapanAracController : AdminBaseController
    {
        private readonly GirisYapanAracService _girisYapanAracService;
        public GirisYapanAracController(GirisYapanAracService girisYapanAracService)
        {
            _girisYapanAracService = girisYapanAracService;
        }

        public ActionResult Index()
        {
            ViewBag.Title = "En Son Giriş Yapan Araçlar";
            return View("~/Views/GirisYapanArac/Index.cshtml");
        }
        [AjaxOnly, HttpPost, ValidateInput(false)]

        public async Task<ActionResult> AracList(GirisYapanAracSearchViewModel model, int? page)
        {




            var currentPageIndex = page - 1 ?? 0;

            var result = _girisYapanAracService.GetGirisYapanAracListIQueryable(model)
                .OrderByDescending(p => p.Tarih)
                .ToPagedList(currentPageIndex, SystemConstants.DefaultPropertyPageSize);



            ModelState.Clear();

            return new ContentResult
            {
                ContentType = "application/json",
                Content = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(new
                {
                    success = true,
                    responseText = RenderPartialViewToString("~/Views/GirisYapanArac/AracList.cshtml", result)
                })
            };

        }
        [AjaxOnly]
        [HttpGet]
        public ActionResult Add()
        {

            ViewData["GirisKapisiListesi"] = _girisYapanAracService.GetKapiList().ToList();

            var model = new GirisYapanAracAddViewModel
            {

            };
            return PartialView("~/Views/GirisYapanArac/_AracEkle.cshtml", model);

        }
        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(GirisYapanAracAddViewModel model)
        {


            if (ModelState.IsValid)
            {
                var callResult = await _girisYapanAracService.AddCarAsync(model);
                if (callResult.Success)
                {

                    ModelState.Clear();
                    var viewModel = (GirisYapanAracListViewModel)callResult.Item;
                    var jsonResult = Json(
                        new
                        {
                            success = true,
                            responseText = RenderPartialViewToString("~/Views/GirisYapanArac/DisplayTemplates/GirisYapanAracListViewModel.cshtml", viewModel),
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
                    responseText = RenderPartialViewToString("~/Views/GirisYapanArac/_AracEkle.cshtml", model)
                });

        }
        [AjaxOnly, HttpPost]
        public async Task<ActionResult> Delete(int aracId)
        {
            var callResult = await _girisYapanAracService.DeleteGirisYapanAracAsync(aracId);
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
        
        public ActionResult AracGrafikGoster()
        {
            return Json(araclar(),JsonRequestBehavior.AllowGet);
        }
        public List<GirisYapanAracGrafik> araclar()
        {
            List<GirisYapanAracGrafik> cs = new List<GirisYapanAracGrafik>();
            AracPlakaSistemiEntities db = new AracPlakaSistemiEntities();

            var karaList = db.GirisYapanAraclar.Where(x=>x.karaListeArac.Equals(true)).ToList();
            cs.Add(new GirisYapanAracGrafik
            {
                car = "Kara Liste Araç",
                count = karaList.Count()
            });

            var kayitliArac = db.GirisYapanAraclar.Where(x=>x.kayitliArac.Equals(true)).ToList();
            cs.Add(new GirisYapanAracGrafik
            {
                car = "Kayıtlı Araçlar",
                count = kayitliArac.Count()
            });
            var misafirArac = db.GirisYapanAraclar.Where(x=>x.misafirArac.Equals(true)).ToList();
            cs.Add(new GirisYapanAracGrafik
            {
                car = "Misafir Araçlar",
                count = misafirArac.Count()
            });
            var yeniGirisArac = db.GirisYapanAraclar.Where(x=>x.yeniArac.Equals(true)).ToList();
            cs.Add(new GirisYapanAracGrafik
            {
                car = "İlk defa giriş yapan araç",
                count = yeniGirisArac.Count()
            });





            
            return cs;

        }
        public ActionResult Grafik()
        {

            
            return PartialView("~/Views/GirisYapanArac/Grafik.cshtml");

        }

    }
}