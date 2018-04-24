using Bookstore.Domain.Adstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookstore.Domain.Entities;

namespace Bookstore.Domain.Concrete
{
    public class EFBookRepository : IBookRepository
    {
        BookStore Context = new BookStore();
        public IEnumerable<Book> Books
        {
            get
            {
                return Context.Books;
            }
        }

        public Book DeleteBook(int BookID)
        {
            Book dbBook = Context.Books.Find(BookID);
            if (dbBook != null)
            {
                Context.Books.Remove(dbBook);
                Context.SaveChanges();
            }
            return dbBook;
        }

        public void SaveBook(Book book)
        {
             Book dbEntity = Context.Books.Find(book.BookID);
            if (dbEntity == null)
                Context.Books.Add(book);
            else
            {
                dbEntity.Title = book.Title;
                dbEntity.Specialization = book.Specialization;
                dbEntity.Price = book.Price;
                dbEntity.Description = book.Description;
                dbEntity.ImageData = book.ImageData;
                dbEntity.ImageMimeType = book.ImageMimeType;
            }
            Context.SaveChanges();
        }
    }
}
