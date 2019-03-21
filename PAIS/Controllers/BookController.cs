using Microsoft.AspNetCore.Mvc;
using PAIS.Models;
using System.Linq;

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
            View(repository.Books
                .OrderBy(p => p.Name)
                .Skip((bookPage - 1) * PageSize)
                .Take(PageSize));

        public ViewResult List() => View(repository.Books);
    }
}
