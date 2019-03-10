using Microsoft.AspNetCore.Mvc;
using PAIS.Models;

namespace PAIS.Controllers
{
    public class BookController : Controller
    {
        private IBookRepository repository;

        public BookController(IBookRepository repo)
        {
            repository = repo;
        }

        public ViewResult List() => View(repository.Books);
    }
}
