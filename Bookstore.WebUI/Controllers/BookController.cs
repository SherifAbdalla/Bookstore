using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bookstore.Domain.Adstract;
using Bookstore.WebUI.Models;
using Bookstore.Domain;

namespace Bookstore.WebUI.Controllers
{
    public class BookController : Controller
    {
        private IBookRepository repository;
        public int PageSize = 4;
        public BookController(IBookRepository BookRep)
        {
            repository = BookRep;
        }
        public ViewResult List()
        {
            return View(repository.Books);
        }
        public ViewResult Pagination(string Specilization, int Page = 1)
        {
            BookListViewModel Model = new BookListViewModel {
                Books = repository.Books.Where(b => Specilization == null || b.Specialization == Specilization)
                .OrderBy(b => b.BookID).Skip((Page - 1) * PageSize)
                .Take(4), PagingInfo = new PagingInfo { CurrentPage = Page, ItemsPerPage = PageSize, Totalltems = Specilization == null ? repository.Books.Count() : repository.Books.Where(b => b.Specialization == Specilization).Count()},
                CurrentSpecilization = Specilization };
            return View("List", Model);
        }
        public FileContentResult GetImage(int BookID)
        {
            Book book = repository.Books.FirstOrDefault(b => b.BookID == BookID);
            if (book != null)
            {
                return File(book.ImageData, book.ImageMimeType);
            }
            else
                return null;
        }
    }
}