using Microsoft.AspNetCore.Mvc;
using PAIS.Models;
using PAIS.Models.ViewModels;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace PAIS.Controllers
{
    [Authorize]
    public class ShopAdminController : Controller
    {
        private IBookRepository repository;

        public ShopAdminController(IBookRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index() => View(repository.Books);

        public ViewResult Edit(int bookId)
        {
            Book model = repository.Books
                   .FirstOrDefault(b => b.BookID == bookId);

            return View(new BookViewModel
            {
                BookID = model.BookID,
                Name = model.Name,
                Description = model.Description,
                Author = model.Author,
                Format = model.Format,
                PublicationDate = model.PublicationDate,
                PublicationType = model.PublicationType,
                Binder = model.Binder,
                Amount = model.Amount,
                Anotation = model.Anotation,
                ISBNCode = model.ISBNCode,
                Price = model.Price
            });
        }

        [HttpPost]
        public IActionResult Edit(BookViewModel model)
        {
            if (ModelState.IsValid)
            {
                Book newBook = new Book
                {
                    BookID = model.BookID,
                    Name = model.Name,
                    Description = model.Description,
                    Author = model.Author,
                    Format = model.Format,
                    PublicationDate = model.PublicationDate,
                    PublicationType = model.PublicationType,
                    Binder = model.Binder,
                    Amount = model.Amount,
                    Anotation = model.Anotation,
                    ISBNCode = model.ISBNCode,
                    Price = model.Price
                };
                //byte[] imageData = null;
                //using (var binaryReader = new BinaryReader(model.Image.OpenReadStream()))
                //{
                //    imageData = binaryReader.ReadBytes((int)model.Image.Length);
                //}
                //newBook.Image = imageData;
                newBook.Image = new byte[] { 3, 10, 8, 25 };

                repository.SaveBook(newBook);
                TempData["message"] = $"{newBook.Name} has been saved";
                return RedirectToAction("Index");
            }
            else
            {
                // there is something wrong with the data values
                return View(model);
            }
        }

        public ViewResult Create() => View("Edit", new BookViewModel());

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