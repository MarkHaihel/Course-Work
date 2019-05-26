using Microsoft.AspNetCore.Mvc;
using PAIS.Models;
using System.Linq;
using PAIS.Models.ViewModels;

namespace PAIS.Controllers
{
    public class BookController : Controller
    {
        private IBookRepository repository;
        public int PageSize = 4;

        public BookController(IBookRepository repo)
        {
            repository = repo;
        }

        public ViewResult List(string type, int bookPage = 1) =>
            View(new BooksListViewModel
            {
                Books = repository.Books
                     .OrderBy(b => b.BookID)
                     .Where(b => type == null || b.PublicationType == type)
                     .Skip((bookPage - 1) * PageSize)
                     .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = bookPage,
                    ItemsPerPage = PageSize,
                    TotalItems = type == null ?
                        repository.Books.Count() :
                        repository.Books.Where(e =>
                        e.PublicationType == type).Count()
                },
                CurrentType = type
            });
        public ViewResult Details(int bookId) =>
            View(repository.GetBook(bookId));
    }
}
