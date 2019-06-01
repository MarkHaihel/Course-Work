using Microsoft.AspNetCore.Mvc;
using PAIS.Models;
using System.Linq;
using PAIS.Models.ViewModels;

namespace PAIS.Controllers
{
    public class BookController : Controller
    {
        private IBookRepository bookRepository;
        private ICommentRepository commentRepository;
        public int PageSize = 6;

        public BookController(IBookRepository bRepo, ICommentRepository cRepo)
        {
            bookRepository = bRepo;
            commentRepository = cRepo;
        }

        public ViewResult List(string search, int bookPage = 1) =>
            View(new BooksListViewModel
            {
                Books = bookRepository.Books
                     .OrderBy(b => b.BookID)
                     .Where(b => search == null || b.Name == search)
                     .Skip((bookPage - 1) * PageSize)
                     .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = bookPage,
                    ItemsPerPage = PageSize,
                    TotalItems = search == null ?
                        bookRepository.Books.Count() :
                        bookRepository.Books.Where(e =>
                        e.PublicationType == search).Count()
                },
                Search = search
            });
        public ViewResult Details(int bookId) =>
            View(new BookCommentsViewModel
            {
                Book = bookRepository.GetBook(bookId),
                Comments = commentRepository.Comments.Where(c => c.BookId == bookId)
            });
    }
}
