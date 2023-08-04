using AracPlakaSistemi.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AracPlakaSistemi.Controllers
{
    public class HomeController : Controller
    {
        AracPlakaSistemiEntities db = new AracPlakaSistemiEntities();
        [Authorize]
        public ActionResult Index()
        {
            var model = db.KayitliAraclar.ToList();
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}