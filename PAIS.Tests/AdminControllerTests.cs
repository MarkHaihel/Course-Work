using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using PAIS.Models.ViewModels;
using PAIS.Controllers;
using PAIS.Models;
using Xunit;

namespace PAIS.Tests
{
    public class AdminControllerTests
    {
        [Fact]
        public void Index_Contains_All_Books()
        {
            // Arrange - create the mock repository
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new Book[] 
            {
                 new Book {BookID = 1, Name = "B1"},
                 new Book {BookID = 2, Name = "B2"},
                 new Book {BookID = 3, Name = "B3"},
            }.AsQueryable<Book>());

            // Arrange - create a controller
            AdminController target = new AdminController(mock.Object);

            // Action
            Book[] result
                = GetViewModel<IEnumerable<Book>>(target.Index())?.ToArray();

            // Assert
            Assert.Equal(3, result.Length);
            Assert.Equal("B1", result[0].Name);
            Assert.Equal("B2", result[1].Name);
            Assert.Equal("B3", result[2].Name);
        }

        [Fact]
        public void Can_Edit_Book()
        {
            // Arrange - create the mock repository
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new Book[] {
                new Book {BookID = 1, Name = "B1"},
                new Book {BookID = 2, Name = "B2"},
                new Book {BookID = 3, Name = "B3"},
            }.AsQueryable<Book>());

            // Arrange - create the controller
            AdminController target = new AdminController(mock.Object);

            // Act
            Book p1 = GetViewModel<Book>(target.Edit(1));
            Book p2 = GetViewModel<Book>(target.Edit(2));
            Book p3 = GetViewModel<Book>(target.Edit(3));

            // Assert
            Assert.Equal(1, p1.BookID);
            Assert.Equal(2, p2.BookID);
            Assert.Equal(3, p3.BookID);
        }

        [Fact]
        public void Cannot_Edit_Nonexistent_Book()
        {
            // Arrange - create the mock repository
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new Book[] {
                new Book {BookID = 1, Name = "B1"},
                new Book {BookID = 2, Name = "B2"},
                new Book {BookID = 3, Name = "B3"},
            }.AsQueryable<Book>());

            // Arrange - create the controller
            AdminController target = new AdminController(mock.Object);

            // Act
            Book result = GetViewModel<Book>(target.Edit(4));

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Can_Save_Valid_Changes()
        {
            // Arrange - create mock repository
            Mock<IBookRepository> mock = new Mock<IBookRepository>();

            // Arrange - create mock temp data
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            // Arrange - create the controller
            AdminController target = new AdminController(mock.Object)
            {
                TempData = tempData.Object
            };

            // Arrange - create a Book
            BookViewModel book = new BookViewModel { Name = "Test" };

            // Act - try to save the Book
            IActionResult result = target.Edit(book);

            // Assert - check that the repository was called
            mock.Verify(m => m.SaveBook(new Book { Name = book.Name }));

            // Assert - check the result type is a redirection
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", (result as RedirectToActionResult).ActionName);
        }

        [Fact]
        public void Cannot_Save_Invalid_Changes()
        {
            // Arrange - create mock repository
            Mock<IBookRepository> mock = new Mock<IBookRepository>();

            // Arrange - create the controller
            AdminController target = new AdminController(mock.Object);

            // Arrange - create a Book
            BookViewModel book = new BookViewModel { Name = "Test" };

            // Arrange - add an error to the model state
            target.ModelState.AddModelError("error", "error");

            // Act - try to save the Book
            IActionResult result = target.Edit(book);

            // Assert - check that the repository was not called
            mock.Verify(m => m.SaveBook(It.IsAny<Book>()), Times.Never());

            // Assert - check the method result type
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Can_Delete_Valid_Books()
        {
            // Arrange - create a Book
            Book book = new Book { BookID = 2, Name = "Test" };

            // Arrange - create the mock repository
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new Book[] {
                new Book {BookID = 1, Name = "B1"},
                book,
                new Book {BookID = 3, Name = "B3"},
            }.AsQueryable<Book>());

            // Arrange - create the controller
            AdminController target = new AdminController(mock.Object);

            // Act - delete the Book
            target.Delete(book.BookID);

            // Assert - ensure that the repository delete method was
            // called with the correct Book
            mock.Verify(m => m.DeleteBook(book.BookID));
        }

        private T GetViewModel<T>(IActionResult result) where T : class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }
    }
}