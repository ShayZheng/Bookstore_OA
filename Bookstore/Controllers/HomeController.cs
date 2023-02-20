using Bookstore.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Bookstore.Controllers
{
    public class HomeController : Controller
    {
        //List books
        private static List<Models.Book> _books = new List<Book>
        {
            // Define a list of books in the bookstore
            new Book { Id = "9b0896fa-3880-4c2e-bfd6-925c87f22878", Name = "CQRS for Dummies", IsReserved = false, ImageUrl = "images/pexels-ata-ebem-10863290.jpg" },
            new Book { Id = "0550818d-36ad-4a8d-9c3a-a715bf15de76", Name = "Visual Studio Tips", IsReserved = false, ImageUrl = "images/pexels-ekrulila-2128249.jpg" },
            new Book { Id = "8e0f11f1-be5c-4dbc-8012-c19ce8cbe8e1", Name = "NHibernate Cookbook", IsReserved = false, ImageUrl = "images/pexels-martin-de-arriba-7171398.jpg" }
        };

        //Get: book store
        public ActionResult Index()
        {
            //Pass the list of books to the view
            return View(_books);
        }

        // Search books 
        [HttpPost]

        public ActionResult Search(string searchTerm)
        {
            // Find all books that contain the search term in the name
            var matchingBooks = _books.Where(b => b.Name.Contains(searchTerm));

            if (matchingBooks.Any())
            {
                // Pass the matching books to the view
                return View("Index", matchingBooks);
            }
            else
            {
                // If no books match the search term, return a view with an error message
                ViewBag.ErrorMessage = "No books were found matching your search term.";
                return View("Error");
            }
        }

        // Reserve books
        // Reserve books
        [HttpPost]
        [Authorize]
        public ActionResult Reserve(string bookId)
        {
            // Find the book with the specific ID
            var book = _books.FirstOrDefault(b => b.Id == bookId);
            if (book == null)
            {
                // If the book is not found, return an error view
                ViewBag.ErrorMessage = "Sorry, the book is not found.";
                return View("Error");
            }

            if (book.IsReserved)
            {
                return RedirectToAction("Index", new { errorMessage = "This book has already been reserved by another customer." });
            }

            book.IsReserved = true;
            var bookingNumber = Guid.NewGuid().ToString();
            TempData["BookingNumber"] = bookingNumber;

            ViewBag.BookingNumber = TempData["BookingNumber"];

            return View("BookingNumber");
        }

        // Return a book
        [HttpPost]
        [Authorize]
        public ActionResult Return(string bookId)
        {
            // Find the book with the specific ID
            var book = _books.FirstOrDefault(b => b.Id == bookId);
            if (book == null)
            {
                // If the book is not found, return an error view
                ViewBag.ErrorMessage = "Sorry, the book is not found.";
                return View("Error");
            }

            if (!book.IsReserved)
            {
                // If the book is not reserved, return an error view
                ViewBag.ErrorMessage = "This book has not been reserved.";
                return View("Error");
            }

            book.IsReserved = false;


            return View("Return");
        }
    }
}