using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using WebLibrary.Controllers;
using WebLibrary.Data;
using Moq;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using WebLibrary.Models;

namespace XUnitTesting
{
    public class UnitTest1 : IClassFixture<WebApplicationFactory<Program>>
    {

        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _httpClient;


        public UnitTest1()
        {
            var factory = new WebApplicationFactory<Program>();
            _factory = factory;
            _httpClient = _factory.CreateClient();

        }

        [Theory]
        [InlineData("/Home/AddBook")]
        [InlineData("/Home/Index")]
        public async void isAllPagesLoaded(string url)
        {
            //Arrange
            var client = _factory.CreateClient();
            //Act
            var response = await client.GetAsync(url);
            int code = (int)response.StatusCode;
            //Assert
            Assert.Equal(200, code);
        }

        [Theory]
        [InlineData("Admin Panel")]
        public async void testIndexPagesContent(string title)
        {
            //Arrange
            var client = _factory.CreateClient();

            //Act
            var response = await _httpClient.GetAsync("/Home/Index/");
            var pageContent = await response.Content.ReadAsStringAsync();
            string contentString  = pageContent.ToString();
            //Assert
            Assert.Contains(title, contentString);
        }


        [Fact]
        public void Index_ReturnsViewResult()
        {
            // Arrange
            //   var mockedDbContext = Create.MockedDbContextFor<AppDbContext>();
            var loggerMock = Substitute.For<ILogger<HomeController>>();
            var controller = new HomeController(loggerMock, GetDbContext());
            var book = new Book
            {
                Id = 5, 
                Author = "Stoycho",
                Title = "Title",
                Type = "book"
            };


            // Act
            var result = controller.ReceiveDataToEdit(book);
            var jsonResult = Assert.IsType<JsonResult>(result);
            dynamic data = jsonResult.Value;

            // Assert
            Assert.NotNull(jsonResult.Value);
            Assert.True(data.Success);
        }

        public AppDbContext GetDbContext()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var option = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connection,
                s => {
                    //s.UseNetTopologySuite();
                    s.MigrationsHistoryTable("__MigrationHistory");
                }).Options;

            var dbContext = new AppDbContext(option);

            if (dbContext != null)
            {
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
            }

            return dbContext;
        }

    }
}