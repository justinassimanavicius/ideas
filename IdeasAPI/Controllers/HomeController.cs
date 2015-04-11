﻿using System.DirectoryServices.AccountManagement;
using System.Web.Security;
using IdeasAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IdeasAPI.Models;

namespace IdeasAPI.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return File("~/index.html", "text/html");
        }


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login model)
        {
            if (!LoginValid(model) || model.Password == null || model.Username == null) return View(new Login{Username = model.Username});

            FormsAuthentication.SetAuthCookie(model.Username, false);
            return RedirectToAction("Index");
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        private bool LoginValid(Login credentials)
        {
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, "WEBMEDIA"))
            {
                // validate the credentials
                var result = pc.ValidateCredentials(credentials.Username, credentials.Password);
                return result;
            }
        }
    }
}
