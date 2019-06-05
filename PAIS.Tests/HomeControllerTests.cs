using System.Collections.Generic;
using System.Linq;
using Moq;
using PAIS.Controllers;
using PAIS.Models;
using Xunit;
using PAIS.Models.ViewModels;
using System;
using Microsoft.AspNetCore.Mvc;

namespace PAIS.Tests
{
    public class HomeControllerTests
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

            Mock<ICommentRepository> mock1 = new Mock<ICommentRepository>();
            mock1.Setup(m => m.Comments).Returns((new Comment[] { }).AsQueryable<Comment>());

            HomeController controller = new HomeController(mock.Object, mock1.Object);
            controller.PageSize = 3;

            //Act
            BooksListViewModel result =
                controller.List(null, 2).ViewData.Model as BooksListViewModel;

            //Assert
            Book[] bookArray = result.Books.ToArray();
            Assert.True(bookArray.Length == 2);
            Assert.Equal("B4", bookArray[0].Name);
            Assert.Equal("B5", bookArray[1].Name);
        }

        [Fact]
        public void Can_Send_Pagination_View_Model()
        {
            // Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns((new Book[] {
                new Book {BookID = 1, Name = "P1"},
                new Book {BookID = 2, Name = "P2"},
                new Book {BookID = 3, Name = "P3"},
                new Book {BookID = 4, Name = "P4"},
                new Book {BookID = 5, Name = "P5"}
            }).AsQueryable<Book>());

            Mock<ICommentRepository> mock1 = new Mock<ICommentRepository>();
            mock1.Setup(m => m.Comments).Returns((new Comment[] { }).AsQueryable<Comment>());


            // Arrange
            HomeController controller =
            new HomeController(mock.Object, mock1.Object) { PageSize = 3 };

            // Act
            BooksListViewModel result =
                controller.List(null, 2).ViewData.Model as BooksListViewModel;

            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.Equal(2, pageInfo.CurrentPage);
            Assert.Equal(3, pageInfo.ItemsPerPage);
            Assert.Equal(5, pageInfo.TotalItems);
            Assert.Equal(2, pageInfo.TotalPages);
        }

        [Fact]
        public void Can_Filter_Books()
        {
            // Arrange
            // - create the mock repository
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns((new Book[] {
                new Book {BookID = 1, Name = "B1", PublicationType = "Cat1"},
                new Book {BookID = 2, Name = "B2", PublicationType = "Cat2"},
                new Book {BookID = 3, Name = "B3", PublicationType = "Cat1"},
                new Book {BookID = 4, Name = "B4", PublicationType = "Cat2"},
                new Book {BookID = 5, Name = "B5", PublicationType = "Cat3"}
            }).AsQueryable<Book>());

            Mock<ICommentRepository> mock1 = new Mock<ICommentRepository>();
            mock1.Setup(m => m.Comments).Returns((new Comment[] { }).AsQueryable<Comment>());

            // Arrange - create a controller and make the page size 3 items
            HomeController controller = new HomeController(mock.Object, mock1.Object);
            controller.PageSize = 3;

            // Action
            Book[] result =
            (controller.List("Cat2", 1).ViewData.Model as BooksListViewModel)
            .Books.ToArray();
            // Assert
            Assert.Equal(2, result.Length);
            Assert.True(result[0].Name == "B2" && result[0].PublicationType == "Cat2");
            Assert.True(result[1].Name == "B4" && result[1].PublicationType == "Cat2");
        }

        [Fact]
        public void Generate_Category_Specific_Book_Count()
        {
            // Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns((new Book[] {
                new Book {BookID = 1, Name = "B1", PublicationType = "Cat1"},
                new Book {BookID = 2, Name = "B2", PublicationType = "Cat2"},
                new Book {BookID = 3, Name = "B3", PublicationType = "Cat1"},
                new Book {BookID = 4, Name = "B4", PublicationType = "Cat2"},
                new Book {BookID = 5, Name = "B5", PublicationType = "Cat3"}
                }).AsQueryable<Book>());

            Mock<ICommentRepository> mock1 = new Mock<ICommentRepository>();
            mock1.Setup(m => m.Comments).Returns((new Comment[] { }).AsQueryable<Comment>());

            HomeController target = new HomeController(mock.Object, mock1.Object);
            target.PageSize = 3;
            Func<ViewResult, BooksListViewModel> GetModel = result =>
                result?.ViewData?.Model as BooksListViewModel;

            // Action
            int? res1 = GetModel(target.List("Cat1"))?.PagingInfo.TotalItems;
            int? res2 = GetModel(target.List("Cat2"))?.PagingInfo.TotalItems;
            int? res3 = GetModel(target.List("Cat3"))?.PagingInfo.TotalItems;
            int? resAll = GetModel(target.List(null))?.PagingInfo.TotalItems;

            // Assert
            Assert.Equal(2, res1);
            Assert.Equal(2, res2);
            Assert.Equal(1, res3);
            Assert.Equal(5, resAll);
        }
    }
}
