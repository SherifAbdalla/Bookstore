using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.WebUI.Infrastructure.Adstract
{
    public interface IAuthProvider
    {
        bool Authenticate(string user, string password);
    }
}
