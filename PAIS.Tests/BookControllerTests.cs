using System.Collections.Generic;
using System.Linq;
using Moq;
using PAIS.Controllers;
using PAIS.Models;
using Xunit;

namespace PAIS.Tests
{
    public class BookControllerTests
    {
        [Fact]
        public void CanPaginate()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns((new Book[] {
                new Book {BookID = 1, Name = "B1"},
                new Book {BookID = 2, Name = "B2"},
                new Book {BookID = 3, Name = "B3"},
                new Book {BookID = 4, Name = "B4"},
                new Book {BookID = 5, Name = "B5"}
            }).AsQueryable<Book>());

            BookController controller = new BookController(mock.Object);
            controller.PageSize = 3;

            //Act
            IEnumerable<Book> result =
                controller.List(2).ViewData.Model as IEnumerable<Book>;

            //Assert
            Book[] bookArray = result.ToArray();
            Assert.True(bookArray.Length == 2);
            Assert.Equal("B4", bookArray[0].Name);
            Assert.Equal("B5", bookArray[1].Name);
        }
    }
}
