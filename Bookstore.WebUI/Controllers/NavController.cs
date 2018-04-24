using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bookstore.Domain.Adstract;

namespace Bookstore.WebUI.Controllers
{
    public class NavController : Controller
    {
        private IBookRepository repository;
        public NavController(IBookRepository repo)
        {
            repository = repo;
        }
        // GET: Nav
        public PartialViewResult Menu(string Specilization = null)
        {
            ViewBag.SelectedSpec = Specilization;
            IEnumerable<string> spec = repository.Books
                .Select(b => b.Specialization)
                .Distinct();
            //string viewName = MobileLayout ? "MenuHorzontal" : "Menu";
            return PartialView("FlexMenu", spec);
        }
    }
}