using Microsoft.AspNetCore.Mvc;
using PAIS.Models;
using System.Linq;

namespace PAIS.Controllers
{
    public class AdminController : Controller
    {
        private IBookRepository repository;

        public AdminController(IBookRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index() => View(repository.Books);

        public ViewResult Edit(int bookId) =>
            View(repository.Books
                .FirstOrDefault(b => b.BookID == bookId));

        [HttpPost]
        public IActionResult Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                repository.SaveBook(book);
                TempData["message"] = $"{book.Name} has been saved";
                return RedirectToAction("Index");
            }
            else
            {
                // there is something wrong with the data values
                return View(book);
            }
        }

        public ViewResult Create() => View("Edit", new Book());

        [HttpPost]
        public IActionResult Delete(int bookId)
        {
            Book deletetBook = repository.DeleteBook(bookId);

            if (deletetBook != null)
            {
                TempData["message"] = $"{deletetBook.Name} was deleted";
            }

            return RedirectToAction("Index");
        }
    }
}