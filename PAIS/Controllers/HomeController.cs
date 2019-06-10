using Microsoft.AspNetCore.Mvc;
using PAIS.Models;
using System.Linq;
using PAIS.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace PAIS.Controllers
{
    public class HomeController : Controller
    {
        private IBookRepository bookRepository;
        private IRateRepository rateRepository;
        private ICommentRepository commentRepository;
        private INewsRepository newsRepository;
        public int PageSize = 4;

        public HomeController(IBookRepository bRepo, ICommentRepository cRepo, IRateRepository rRepo
            ,INewsRepository nRepo)
        {
            bookRepository = bRepo;
            commentRepository = cRepo;
            rateRepository = rRepo;
            newsRepository = nRepo;
        }

        public ViewResult List(string search, int page = 1) =>
            View(new BooksListViewModel
            {
                Books = bookRepository.Books
                     .OrderBy(b => b.BookID)
                     .Where(b => search == null || b.Name.ToLower().Contains(search.ToLower()))
                     .Skip((page - 1) * PageSize)
                     .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = search == null ?
                        bookRepository.Books.Count() :
                        bookRepository.Books.Where(e =>
                        e.Name.ToLower().Contains(search.ToLower())).Count()
                },
                Search = search
            });

        public ViewResult Contact() =>
            View();

        public ViewResult Details(int bookId) =>
            View(new BookCommentsViewModel
            {
                Book = bookRepository.GetBook(bookId),
                Comments = commentRepository.Comments.Where(c => c.BookId == bookId)
            });

        public ViewResult NewsList(string search, int page = 1) =>
            View(new NewsListViewModel
            {
                NewsRepo = newsRepository.NewsRepo
                 .OrderBy(n => n.Date)
                 .Skip((page - 1) * PageSize)
                 .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = search == null ?
                     newsRepository.NewsRepo.Count() :
                     newsRepository.NewsRepo.Where(e =>
                        e.Name.Contains(search) || e.Author.Contains(search) || e.Description.Contains(search)).Count()
                },
                Search = search
            });
        public ViewResult NewsDetails(int newsID) =>
            View(newsRepository.GetNews(newsID));

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

        [Authorize]
        [HttpPost]
        public IActionResult RateBook(Rate rate)
        {
            if (string.IsNullOrEmpty(rate.UserId) || rate.BookId == 0 || rate.Value < 1 || rate.Value > 5)
            {
                RedirectToAction("Error");
            }
            Book bookTORate = bookRepository.GetBook(rate.BookId);
            if (bookTORate == null)
            {
                return RedirectToAction("Error");
            }
            Rate yourRate = new Rate
            {
                BookId = rate.BookId,
                UserId = rate.UserId,
                Value = rate.Value
            };

            List<Rate> allRates = rateRepository.Rates.ToList();
            if (allRates != null)
            {
                bool isFinded = false;
                foreach (var r in allRates)
                {
                    if (r.BookId == rate.BookId && r.UserId == rate.UserId)
                    {
                        isFinded = true;
                        yourRate.RateId = r.RateId;
                        bookTORate.Rate += (yourRate.Value - r.Value) / bookTORate.RatesAmount;
                        rateRepository.SaveRate(yourRate);
                        bookRepository.SaveBook(bookTORate);
                        break;
                    }
                }
                if (!isFinded)
                {
                    uint amount = bookTORate.RatesAmount;
                    bookTORate.RatesAmount++;
                    bookTORate.Rate = (bookTORate.Rate * amount + yourRate.Value) / bookTORate.RatesAmount;
                    bookRepository.SaveBook(bookTORate);
                    rateRepository.SaveRate(yourRate);
                }
            }
            else
            {
                uint amount = bookTORate.RatesAmount;
                bookTORate.RatesAmount++;
                bookTORate.Rate = (bookTORate.Rate * amount + yourRate.Value) / bookTORate.RatesAmount;
                bookRepository.SaveBook(bookTORate);
                rateRepository.SaveRate(yourRate);
            }

            return RedirectToAction("Details", "Home", new { bookId = bookTORate.BookID });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
