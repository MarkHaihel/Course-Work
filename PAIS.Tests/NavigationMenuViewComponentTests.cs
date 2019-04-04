using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using PAIS.Components;
using PAIS.Models;
using Xunit;

namespace PAIS.Tests
{
    public class NavigationMenuViewComponentTests
    {
        [Fact]
        public void Can_Select_Categories()
        {
            // Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns((new Book[] {
                new Book {BookID = 1, Name = "B1", PublicationType = "Apples"},
                new Book {BookID = 2, Name = "B2", PublicationType = "Apples"},
                new Book {BookID = 3, Name = "B3", PublicationType = "Plums"},
                new Book {BookID = 4, Name = "B4", PublicationType = "Oranges"},
            }).AsQueryable<Book>());
            NavigationMenuViewComponent target =
                new NavigationMenuViewComponent(mock.Object);

            // Act = get the set of categories
            string[] results = ((IEnumerable<string>)(target.Invoke()
                as ViewViewComponentResult).ViewData.Model).ToArray();

            // Assert
            Assert.True(Enumerable.SequenceEqual(new string[] { "Apples",
                "Oranges", "Plums" }, results));
        }
    }
}
