using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bookstore.Domain.Entities;
using System.Linq;
using Moq;
using Bookstore.Domain;
using Bookstore.WebUI.Controllers;
using System.Web.Mvc;
using Bookstore.WebUI.Models;
using Bookstore.Domain.Adstract;

namespace Bookstore.UnitTests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void Can_AddNew_Lines()
        {
            Domain.Book b1 = new Domain.Book { BookID = 1, Title = "ASP.NET" };
            Domain.Book b2 = new Domain.Book { BookID = 2, Title = "Oracle" };
            Cart Target = new Cart();
            Target.AddItem(b1);
            Target.AddItem(b2, 3);
            CartLine[] result = Target.Lines.ToArray();
            Assert.AreEqual(result[0].Book, b1);
            Assert.AreEqual(result[1].Book, b2);
        }
        [TestMethod]
        public void Can_Add_Qty_for_Existing_Lines()
        {
            Domain.Book b1 = new Domain.Book { BookID = 1, Title = "ASP.NET" };
            Domain.Book b2 = new Domain.Book { BookID = 2, Title = "Oracle" };
            Cart Target = new Cart();
            Target.AddItem(b1);
            Target.AddItem(b2, 3);
            Target.AddItem(b1, 5);
            CartLine[] result = Target.Lines.OrderBy(cl => cl.Book.BookID).ToArray();
            Assert.AreEqual(result.Length, 2);
            Assert.AreEqual(result[0].Quantity, 6);
            Assert.AreEqual(result[1].Quantity, 3);
        }
        [TestMethod]
        public void Can_Remove_Line ()
        {
            Domain.Book b1 = new Domain.Book { BookID = 1, Title = "ASP.NET" };
            Domain.Book b2 = new Domain.Book { BookID = 2, Title = "Oracle" };
            Domain.Book b3 = new Domain.Book { BookID = 3, Title = "C#" };
            Cart Target = new Cart();
            Target.AddItem(b1);
            Target.AddItem(b2, 3);
            Target.AddItem(b3, 5);
            Target.AddItem(b2, 1);
            Target.RemoveLine(b2);
            Assert.AreEqual((Target.Lines.Where(cl => cl.Book == b2)).Count(), 0);
            Assert.AreEqual(Target.Lines.Count(), 2);
        }
        [TestMethod]
        public void Calculate_Cart_Total()
        {
            Domain.Book b1 = new Domain.Book { BookID = 1, Title = "ASP.NET", Price = 100m };
            Domain.Book b2 = new Domain.Book { BookID = 2, Title = "Oracle", Price = 50m };
            Domain.Book b3 = new Domain.Book { BookID = 3, Title = "C#", Price = 70m };
            Cart Target = new Cart();
            Target.AddItem(b1, 1);
            Target.AddItem(b2, 2);
            Target.AddItem(b3);
            decimal result = Target.ComputeTotalValue();
            Assert.AreEqual(result, 270);
        }
        [TestMethod]
        public void Can_Clear_Contents()
        {
            Domain.Book b1 = new Domain.Book { BookID = 1, Title = "ASP.NET" };
            Domain.Book b2 = new Domain.Book { BookID = 2, Title = "Oracle" };
            Domain.Book b3 = new Domain.Book { BookID = 3, Title = "C#" };
            Cart Target = new Cart();
            Target.AddItem(b1);
            Target.AddItem(b2, 3);
            Target.AddItem(b3, 5);
            Target.AddItem(b2, 1);
            Target.Clear();
            Assert.AreEqual(Target.Lines.Count(), 0);
        }
        [TestMethod]
        public void Can_Add_To_Cart()
        {
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(
                    new Domain.Book[]
                    {
                        new Domain.Book { BookID = 1, Title = "ASP.net MVC", Specialization = "Programming" }
                    }.AsQueryable()

                );
            Cart cart = new Cart();
            CartController target = new CartController(mock.Object, null);
            target.AddToCart(cart, 1, null);
            //RedirectToRouteResult result = target.AddToCart(cart, 2, "myurl");
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToArray()[0].Book.Title, "ASP.net MVC");
        }
        [TestMethod]
        public void Adding_Book_To_Cart_Goes_To_Cart_Screen()
        {
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(
                    new Domain.Book[]
                    {
                        new Domain.Book { BookID = 1, Title = "ASP.net MVC", Specialization = "Programming" }
                    }.AsQueryable()

                );
            Cart cart = new Cart();
            CartController target = new CartController(mock.Object, null);
            RedirectToRouteResult result = target.AddToCart(cart, 2, "myurl");
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myurl");
        }
        [TestMethod]
        public void Can_View_Cart_Content()
        {
            Cart cart = new Cart();
            CartController target = new CartController(null, null);
            CartIndexViewModel result = (CartIndexViewModel)target.Index(cart, "myurl").ViewData.Model;
            Assert.AreEqual(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myurl");
        }
        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            ShippingDetails shippingDetails = new ShippingDetails();
            CartController target = new CartController(null, mock.Object);
            ViewResult result = target.Checkout(cart, shippingDetails);
            //mock.Verify(m => m.ProcessorOrder);
            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }
        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            cart.AddItem(new Domain.Book(), 1);
            ShippingDetails shippingDetails = new ShippingDetails();
            CartController target = new CartController(null, mock.Object);
            target.ModelState.AddModelError("error", "error");
            ViewResult result = target.Checkout(cart, shippingDetails);
            mock.Verify(m => m.ProcessorOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());
            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }
        [TestMethod]
        public void Can_Checkout_And_Submit_Order()
        {
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            cart.AddItem(new Domain.Book(), 1);
            ShippingDetails shippingDetails = new ShippingDetails();
            CartController target = new CartController(null, mock.Object);
            ViewResult result = target.Checkout(cart, shippingDetails);
            mock.Verify(m => m.ProcessorOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Once());
            Assert.AreEqual("Completed", result.ViewName);
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }
    }
}
