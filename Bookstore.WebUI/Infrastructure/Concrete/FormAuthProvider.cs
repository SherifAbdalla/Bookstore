using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Bookstore.WebUI.Infrastructure.Concrete
{
    public class FormAuthProvider : Adstract.IAuthProvider
    {
        public bool Authenticate(string user, string password)
        {
            bool resuut = FormsAuthentication.Authenticate(user, password);
            if(resuut)
            {
                FormsAuthentication.SetAuthCookie(user, false);
            }
            return resuut;
        }
    }
}