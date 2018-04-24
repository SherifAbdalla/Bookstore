using System.Linq;
using System.Web.Mvc;
using Bookstore.Domain.Adstract;
using Bookstore.WebUI.Models;
using Bookstore.Domain.Entities;

namespace Bookstore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private IBookRepository repository;
        private IOrderProcessor OrderProcessor;
        public CartController(IBookRepository repo, IOrderProcessor proc)
        {
            repository = repo;
        }
        public RedirectToRouteResult AddToCart(Cart cart, int BookID, string returnUrl)
        {
            Domain.Book book = repository.Books.FirstOrDefault(b => b.BookID == BookID);
            if(book != null)
            {
                cart.AddItem(book);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
        public RedirectToRouteResult RemoveFromCart(Cart cart, int BookID, string returnUrl)
        {
            Domain.Book book = repository.Books.FirstOrDefault(b => b.BookID == BookID);
            if (book != null)
            {
                cart.RemoveLine(book);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }
        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }
        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails ShippingDetails)
        {
            if (cart.Lines.Count() == 0)
                ModelState.AddModelError("", "Sorry, your cart is empty");
            if(ModelState.IsValid)
            {
                OrderProcessor.ProcessorOrder(cart, ShippingDetails);
                cart.Clear();
                return View("Completed");
            }
            else
            {
                return View(ShippingDetails);
            }
        }

        // GET: Cart
        public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel { Cart = cart, ReturnUrl = returnUrl });
        }
    }
}