using Microsoft.AspNetCore.Mvc;
using PAIS.Models;
using PAIS.Models.ViewModels;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace PAIS.Controllers
{
    [Authorize(Roles = "shop admin")]
    public class ShopAdminController : Controller
    {
        private IBookRepository bookRepository;
        private ICommentRepository commentRepository;
        private IRateRepository rateRepository;

        public ShopAdminController(IBookRepository bRepo, ICommentRepository cRepo, IRateRepository rRepo)
        {
            bookRepository = bRepo;
            commentRepository = cRepo;
            rateRepository = rRepo;
        }

        public ViewResult Index(string search = "") => View(bookRepository.Books.Where(b => b.Name.Contains(search)));

        public ViewResult Edit(int bookId)
        {
            Book model = bookRepository.GetBook(bookId);

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
                Price = model.Price,
                Rate = model.Rate,
                RatesAmount = model.RatesAmount
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
                    Price = model.Price,
                    Rate = model.Rate,
                    RatesAmount = model.RatesAmount
                };
                if (model.Image != null)
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(model.Image.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)model.Image.Length);
                    }
                    newBook.Image = imageData;
                }
                else
                {
                    return RedirectToAction("Error");
                }

                bookRepository.SaveBook(newBook);
                TempData["message"] = $"{newBook.Name} була збережена";
                return RedirectToAction("Index");
            }
            else
            {
                // there is something wrong with the data values
                return View(model);
            }
        }

        public ViewResult Create() => View("Edit", new BookViewModel() { Rate = 0, RatesAmount = 0 });

        [HttpPost]
        public IActionResult Delete(int bookId)
        {
            Book deletetBook = bookRepository.DeleteBook(bookId);

            commentRepository.DeleteBookComments(bookId);
            rateRepository.DeleteBookRates(bookId);

            if (deletetBook != null)
            {
                TempData["message"] = $"{deletetBook.Name} була видалена";
            }

            return RedirectToAction("Index");
        }
    }
}