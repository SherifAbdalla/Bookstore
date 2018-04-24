using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Bookstore.Domain.Adstract;
using Bookstore.Domain.Entities;
using Bookstore.WebUI.Controllers;
using Bookstore.WebUI.HTMLHelper;
using System.Linq;
using System.Web.Mvc;
using Bookstore.WebUI.Models;

namespace Bookstore.UnitTests
{
    [TestClass]
    public class ProductCatalog
    {
        [TestMethod]
        public void Can_Paginate()
        {
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(new Domain.Book[]
                {
                    new Domain.Book { BookID=1, Title="Book1" },
                    new Domain.Book { BookID=2, Title="Book2" },
                    new Domain.Book { BookID=3, Title="Book3" },
                    new Domain.Book { BookID=4, Title="Book4" },
                    new Domain.Book { BookID=5, Title="Book5" }
                });
            BookController controller = new BookController(mock.Object);
            controller.PageSize = 3;
            BookListViewModel result = (BookListViewModel) controller.Pagination(null, 2).Model;
            //Book[] bookArray = result.Books.ToArray();
            //Assert.IsTrue(bookArray.Length == 2);
            //Assert.AreEqual(bookArray[0].Title, "Book4");
            //Assert.AreEqual(bookArray[1].Title, "Book5");
        }
        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            HtmlHelper myhelper = null;
            PagingInfo paginginfo = new PagingInfo
            {
                CurrentPage = 2,
                Totalltems = 14,
                ItemsPerPage = 5
            };           
            Func<int, string> pageUrlDelegate = i => "page" + i;
            string expectedResult = "<a class=\"btn btn-default\"href =\"page0\">0</a>"+
                                    "<a class=\"btn btn-default btn-primary selected\"href =\"page1\">1</a>" +
                                    "<a class=\"btn btn-default\"href =\"page2\">2</a>";
            MvcHtmlString result = myhelper.PageLinks(paginginfo, pageUrlDelegate);
            Assert.AreEqual(expectedResult, result.ToString());
        }
        [TestMethod]
        public void Can_Send_Paginate_View_Mode()
        {
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns( new Domain.Book[]
                {
                new Domain.Book { BookID = 1, Title = "Operating System" },
                new Domain.Book { BookID = 2, Title = "web Applicatons ASP.NET" },
                new Domain.Book { BookID = 3, Title = "Android Mobile Applications" },
                new Domain.Book { BookID = 4, Title = "Database System" },
                new Domain.Book { BookID = 5, Title = "MIS"}
                }
                );
            BookController Controller = new BookController(mock.Object);
            Controller.PageSize = 3;
            BookListViewModel result = (BookListViewModel)Controller.Pagination(null, 2).Model;
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.Totalltems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }
        [TestMethod]
        public void Can_Filter_Book()
        {
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(new Domain.Book[]
                {
                new Domain.Book { BookID = 1, Title = "Operating System", Specialization = "CS" },
                new Domain.Book { BookID = 2, Title = "web Applicatons ASP.NET", Specialization = "IS" },
                new Domain.Book { BookID = 3, Title = "Android Mobile Applications", Specialization = "IS" },
                new Domain.Book { BookID = 4, Title = "Database System", Specialization = "IS" },
                new Domain.Book { BookID = 5, Title = "MIS", Specialization = "IS" }
                }
                );
            BookController Controller = new BookController(mock.Object);
            Controller.PageSize = 3;
            //Book[] result = ((BookListViewModel)Controller.Pagination("IS", 2).Model).Books.ToArray();
            //Assert.AreEqual(result.Length, 1);
            //Assert.IsTrue(result[0].Title == "MIS" && result[0].Specialization == "IS");
        }
        [TestMethod]
        public void Can_Create_Specialization()
        {
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(new Domain.Book[]
                {
                new Domain.Book { BookID = 1, Title = "Operating System", Specialization = "CS" },
                new Domain.Book { BookID = 2, Title = "web Applicatons ASP.NET", Specialization = "IS" },
                new Domain.Book { BookID = 3, Title = "Android Mobile Applications", Specialization = "IS" },
                new Domain.Book { BookID = 4, Title = "Database System", Specialization = "IS" },
                new Domain.Book { BookID = 5, Title = "MIS", Specialization = "IS" }
                }
                );
            NavController Controller = new NavController(mock.Object);
            string[] result = ((IEnumerable<string>)Controller.Menu().Model).ToArray();
            Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(result[0] == "CS" && result[1] == "IS");
        }
        [TestMethod]
        public void Indicates_Selected_Spec()
        {
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(new Domain.Book[]
                {
                new Domain.Book { BookID = 1, Title = "Operating System", Specialization = "CS" },
                new Domain.Book { BookID = 2, Title = "web Applicatons ASP.NET", Specialization = "IS" },
                new Domain.Book { BookID = 3, Title = "Android Mobile Applications", Specialization = "IS" },
                new Domain.Book { BookID = 4, Title = "Database System", Specialization = "IS" },
                new Domain.Book { BookID = 5, Title = "MIS", Specialization = "IS" }
                }
                );
            NavController Controller = new NavController(mock.Object);
            string result = Controller.Menu("IS").ViewBag.SelectedSpec;
            Assert.AreEqual("IS", result);
        }
        [TestMethod]
        public void Generate_Spec_Specific_Book_Count()
        {
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(new Domain.Book[]
                {
                new Domain.Book { BookID = 1, Title = "Operating System", Specialization = "CS" },
                new Domain.Book { BookID = 2, Title = "web Applicatons ASP.NET", Specialization = "IS" },
                new Domain.Book { BookID = 3, Title = "Android Mobile Applications", Specialization = "IS" },
                new Domain.Book { BookID = 4, Title = "Database System", Specialization = "CS" },
                new Domain.Book { BookID = 5, Title = "MIS", Specialization = "IS" }
                }
                );
            BookController Controller = new BookController(mock.Object);
            int result1 = ((BookListViewModel)Controller.Pagination("IS").Model).PagingInfo.Totalltems;
            int result2 = ((BookListViewModel)Controller.Pagination("CS").Model).PagingInfo.Totalltems;
            Assert.AreEqual(result1, 3);
            Assert.AreEqual(result2, 2);
        }
    }
}
