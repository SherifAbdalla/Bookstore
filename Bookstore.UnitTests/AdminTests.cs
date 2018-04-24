using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Bookstore.Domain.Adstract;
using Bookstore.Domain;
using Bookstore.WebUI.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Bookstore.UnitTests
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void Index_Contains_All_Products()
        {
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new Book[]
            {
                new Book { BookID = 1, Title = "Book1" },
                new Book { BookID = 2, Title = "Book2" },
                new Book { BookID = 3, Title = "Book3" }
            });
            AdminController target = new AdminController(mock.Object);
            Book[] result = ((IEnumerable<Book>)target.Index().ViewData.Model).ToArray();
            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual("Book1", result[0].Title);
            Assert.AreEqual("Book2", result[1].Title);
            Assert.AreEqual("Book3", result[2].Title);
        }
        [TestMethod]
        public void Can_Edit_Book()
        {
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new Book[]
            {
                new Book { BookID = 1, Title = "web" },
                new Book { BookID = 2, Title = "db" },
                new Book { BookID = 3, Title = "asp" }
            });
            AdminController target = new AdminController(mock.Object);
            Book b1 = (Book) target.Edit(1).ViewData.Model;
            Book b2 = (Book)target.Edit(2).ViewData.Model;
            Book b3 = (Book)target.Edit(3).ViewData.Model;
            Assert.AreEqual("web", b1.Title);
            Assert.AreEqual(2, b2.BookID);
            Assert.AreEqual("asp", b3.Title);
        }
        [TestMethod]
        public void Cannot_Edit_Book()
        {
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new Book[]
            {
                new Book { BookID = 1, Title = "web" },
                new Book { BookID = 2, Title = "db" },
                new Book { BookID = 3, Title = "asp" }
            });
            AdminController target = new AdminController(mock.Object);
            Book b4 = (Book)target.Edit(4).ViewData.Model;
            Assert.IsNull(b4);
        }
        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            AdminController target = new AdminController(mock.Object);
            Book book = new Book
            {
                Title = "TestBook"
            };
            //ActionResult result = target.Edit(book);
            mock.Verify(b => b.SaveBook(book));
            //Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }
        [TestMethod]
        public void Cannot_Save_InValid_Changes()
        {
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            AdminController target = new AdminController(mock.Object);
            Book book = new Book
            {
                Title = "TestBook"
            };
            target.ModelState.AddModelError("error", "error");
            //ActionResult result = target.Edit(book);
            mock.Verify(b => b.SaveBook(It.IsAny<Book>()), Times.Never);
            //.IsInstanceOfType(result, typeof(ViewResult));
        }
        [TestMethod]
        public void Can_Delete_Valid_Book()
        {
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            Book book = new Book { BookID = 1, Title = "Test Book" };
            mock.Setup(m => m.Books).Returns(new Book[]
                {
                    new Book { BookID = 2, Title = "Test 2" },
                    new Book { BookID = 3, Title = "Test 3" },
                    book
                });
            AdminController target = new AdminController(mock.Object);
            target.ModelState.AddModelError("error", "error");
            ActionResult result = target.Delete(book.BookID);
            mock.Verify(b => b.DeleteBook(book.BookID));
        }
    }
}
