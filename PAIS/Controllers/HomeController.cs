using Microsoft.AspNetCore.Mvc;
using PAIS.Models;
using System.Linq;
using PAIS.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Diagnostics;

namespace PAIS.Controllers
{
    public class HomeController : Controller
    {
        private IBookRepository bookRepository;
        private ICommentRepository commentRepository;
        public int PageSize = 6;

        public HomeController(IBookRepository bRepo, ICommentRepository cRepo)
        {
            bookRepository = bRepo;
            commentRepository = cRepo;
        }

        public ViewResult List(string search, int bookPage = 1) =>
            View(new BooksListViewModel
            {
                Books = bookRepository.Books
                     .OrderBy(b => b.BookID)
                     .Where(b => search == null || b.Name.ToLower().Contains(search.ToLower()))
                     .Skip((bookPage - 1) * PageSize)
                     .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = bookPage,
                    ItemsPerPage = PageSize,
                    TotalItems = search == null ?
                        bookRepository.Books.Count() :
                        bookRepository.Books.Where(e =>
                        e.Name.ToLower().Contains(search.ToLower())).Count()
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
            if (comment == null)
            {
                return RedirectToAction("Error");
            }

            comment.Time = DateTime.Now;

            commentRepository.SaveComment(comment);

            return RedirectToAction("Details", "Home", new { bookId = comment.BookId });
        }

        [Authorize]
        [HttpPost]
        public IActionResult EditComment(Comment comment, string text)
        {
            if (comment == null)
            {
                return RedirectToAction("Error");
            }
            if (string.IsNullOrEmpty(text))
            {
                commentRepository.DeleteComment(comment.CommentId);
                goto loop;
            }

            commentRepository.SaveComment(comment);
            loop:

            return RedirectToAction("Details", "Home", new { bookId = comment.BookId });
        }

        [Authorize]
        [HttpPost]
        public IActionResult DeleteComment(int commentId)
        {
            if (commentId == 0)
            {
                return RedirectToAction("Error");
            }

            if (commentRepository.GetComment(commentId) == null)
            {
                return RedirectToAction("Error");
            }

            Comment comment = commentRepository.DeleteComment(commentId);

            return RedirectToAction("Details", "Home", new { bookId = comment.BookId });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
