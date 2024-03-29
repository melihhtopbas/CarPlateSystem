﻿using AracPlakaSistemi.Data;
using AracPlakaSistemi.ViewModels.Admin;
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
        public ActionResult Login(KullaniciViewModel kullanici)
        {
            var kullaniciInDb = db.Kullanici.FirstOrDefault(x => x.KullaniciAd == kullanici.KullaniciAd && x.Sifre==kullanici.Sifre);
            if (kullaniciInDb!=null)
            {
                FormsAuthentication.SetAuthCookie(kullaniciInDb.KullaniciAd, false);
                return RedirectToAction("Index", "KayitliArac");   
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