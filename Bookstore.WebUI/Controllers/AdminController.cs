using Bookstore.Domain;
using Bookstore.Domain.Adstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bookstore.WebUI.Controllers
{
    //[Authorize]
    public class AdminController : Controller
    {
        private readonly IBookRepository _repository;
        public AdminController(IBookRepository repo)
        {
            _repository = repo;
        }
        // GET: Admin
        public ViewResult Index()
        {
            return View(_repository.Books);
        }
        [HttpPost]
        public ViewResult Index(string searchValue)
        {
            IEnumerable<Book> books;
            if (searchValue != null)
            {
                books = from b in _repository.Books
                        where //b.Description.IndexOf(searchValue) > 0  ||
                              b.Title.IndexOf(searchValue) >= 0 || 
                              b.Specialization.IndexOf(searchValue) >= 0
                              select b;
            }
            else
            {
                books = from b in _repository.Books
                        select b;
            }
            return View(books);
        }
        public ViewResult Edit(int BookID)
        {
            Book book = _repository.Books.FirstOrDefault(b => b.BookID == BookID);
            return View(book);
        }
        [HttpPost]
        public ActionResult Edit(Book book, HttpPostedFileWrapper image)
        {
            if(ModelState.IsValid)
            {
                if(image != null)
                {
                    book.ImageMimeType = image.ContentType;
                    book.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(book.ImageData, 0, image.ContentLength);
                }
                _repository.SaveBook(book);
                TempData["message"] = book.Title + " has been saved";
                return RedirectToAction("Index");
            }
            else
            {
                return View(book);
            }
        }
        public ViewResult Create()
        {
            return View("Edit", new Book());
        }
        public ActionResult Delete(int BookID)
        {
            Book deletedBook = _repository.DeleteBook(BookID);
            if(deletedBook != null)
            {
                TempData["message"] = deletedBook.Title + " was delete";
            }
            return RedirectToAction("Index");
        }
        public ViewResult Search(string searchValue)
        {
            IEnumerable<Book> books;
                if (searchValue != null)
            {
                books = from b in _repository.Books
                      where b.Description.Contains(searchValue) || b.Title.Contains(searchValue) || b.Specialization.Contains(searchValue)
                      select b;
            }
                else
            {
                books = from b in _repository.Books
                        select b;
            }
            return View("Index", books);
        }
    }
}