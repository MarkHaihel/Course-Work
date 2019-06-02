using Microsoft.AspNetCore.Mvc;
using PAIS.Models;
using System.Linq;
using PAIS.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System;

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

        [Authorize]
        [HttpPost]
        public IActionResult AddComment(Comment comment)
        {
            comment.Time = DateTime.Now;

            commentRepository.SaveComment(comment);

            return RedirectToAction("Details", "Book", new { bookId = comment.BookId });
        }

        [Authorize]
        [HttpPost]
        public IActionResult DeleteComment(int commentId)
        {
            Comment comment = commentRepository.DeleteComment(commentId);

            return RedirectToAction("Details", "Book", new { bookId = comment.BookId });
        }
    }
}
