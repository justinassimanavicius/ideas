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
        private readonly AuthenticationService _authentication;
        public HomeController(AuthenticationService authentication)
        {
            _authentication = authentication;
        }

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
            if (!LoginValid(model) && false)
            {
                ViewBag.Error = "Oops! Check your credentials and try again.";
                return View(new Login { Username = model.Username });
            }

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
	        if (String.IsNullOrEmpty(credentials.Username) || String.IsNullOrEmpty(credentials.Password))
		    {
			    return false;
		    }

            return _authentication.SourceValidateCredentials(credentials);
	    }


    }
}
