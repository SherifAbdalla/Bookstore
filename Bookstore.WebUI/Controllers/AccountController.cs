using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bookstore.WebUI.Infrastructure.Adstract;
using Bookstore.WebUI.Models;

namespace Bookstore.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private IAuthProvider AuthProvider;
        public AccountController(IAuthProvider auth)
        {
            AuthProvider = auth;
        }

        public ViewResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model, string Returnurl)
        {
            if(ModelState.IsValid)
            {
                if (AuthProvider.Authenticate(model.Username, model.Password))
                    return Redirect(Returnurl ?? Url.Action("Index", "Admin"));
                else
                {
                    ModelState.AddModelError("", "Incorrect username/password");
                    return View();
                }
            }
            else
            return View();
        }
    }
}