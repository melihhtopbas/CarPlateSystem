using AracPlakaSistemi.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AracPlakaSistemi.Controllers
{
    public class SecurityController : Controller
    {
        AracPlakaSistemiEntities db = new AracPlakaSistemiEntities();
        // GET: Security
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Kullanici kullanici)
        {
            var kullaniciInDb = db.Kullanici.FirstOrDefault(x => x.KullaniciAd == kullanici.KullaniciAd && x.Sifre==kullanici.Sifre);
            if (kullaniciInDb!=null)
            {
                FormsAuthentication.SetAuthCookie(kullaniciInDb.KullaniciAd, false);
                return RedirectToAction("Index", "Home");   
            }
            else
            {
                ViewBag.Mesaj = " gecersiz";
                return View(Login());
            }
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Security");
        }
    }
}