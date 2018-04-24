using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Bookstore.WebUI.Infrastructure.Adstract;
using Bookstore.WebUI.Models;
using Bookstore.WebUI.Controllers;
using System.Web.Mvc;

namespace Bookstore.UnitTests
{
    [TestClass]
    public class AdminSecurityTests
    {
        [TestMethod]
        public void Can_Login_With_Valid_Credentials()
        {
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("admin", "admin")).Returns(true);
            LoginViewModel model = new LoginViewModel { Username = "admin", Password = "admin" };
            AccountController target = new AccountController(mock.Object);
            ActionResult result = target.Login(model, "myUrl");
            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            Assert.AreEqual("myUrl", ((RedirectResult)result).Url);
        }
        [TestMethod]
        public void Can_Login_With_InValid_Credentials()
        {
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("userx", "passx")).Returns(true);
            LoginViewModel model = new LoginViewModel { Username = "admin", Password = "admin" };
            AccountController target = new AccountController(mock.Object);
            ActionResult result = target.Login(model, "myUrl");
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsFalse(((ViewResult)result).ViewData.ModelState.IsValid);
        }
    }
}
