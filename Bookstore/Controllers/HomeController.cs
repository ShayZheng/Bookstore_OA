using Bookstore.Models;
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
        private static List<Models.Book> _books = new List<Book>
    {
        new Book { Id = "9b0896fa-3880-4c2e-bfd6-925c87f22878", Name = "CQRS for Dummies", IsReserved = false },
        new Book { Id = "0550818d-36ad-4a8d-9c3a-a715bf15de76", Name = "Visual Studio Tips", IsReserved = false },
        new Book { Id = "8e0f11f1-be5c-4dbc-8012-c19ce8cbe8e1", Name = "NHibernate Cookbook", IsReserved = false }
    };

        public ActionResult Index()
        {
            return View(_books);
        }

        [HttpPost]
        public ActionResult Search(string searchTerm)
        {
            var matchingBooks = _books.Where(b => b.Name.Contains(searchTerm));
            return View("Index", matchingBooks);
        }

        [HttpPost]
        public ActionResult Reserve(string bookId)
        {
            var book = _books.FirstOrDefault(b => b.Id == bookId);
            if (book == null)
            {
                return HttpNotFound();
            }

            if (book.IsReserved)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict);
            }

            book.IsReserved = true;
            var bookingNumber = Guid.NewGuid().ToString();
            TempData["BookingNumber"] = bookingNumber;

            return RedirectToAction("Index");
        }

        //    public ActionResult Index()
        //    {
        //        return View();
        //    }

        //    public ActionResult About()
        //    {
        //        ViewBag.Message = "Your application description page.";

        //        return View();
        //    }

        //    public ActionResult Contact()
        //    {
        //        ViewBag.Message = "Your contact page.";

        //        return View();
        //    }
    }
}