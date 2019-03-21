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

        public ViewResult List(int bookPage = 1) =>
            View(new BooksListViewModel
            {
                Books = repository.Books
                     .OrderBy(p => p.BookID)
                     .Skip((bookPage - 1) * PageSize)
                     .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = bookPage,
                    ItemsPerPage = PageSize,
                    TotalItems = repository.Books.Count()
                }
            });
    }
}
