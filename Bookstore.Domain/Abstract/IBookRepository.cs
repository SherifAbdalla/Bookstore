using Bookstore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Domain.Adstract
{
    public interface IBookRepository
    {
        IEnumerable<Book> Books { get; }
        void SaveBook(Book book);
        Book DeleteBook(int BookID);
    }
}
