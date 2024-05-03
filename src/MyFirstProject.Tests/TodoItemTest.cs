using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using MyFirstProject.Tests.Models;

namespace MyFirstProject.Tests
{
    [TestClass]
    public class TodoItemsControllerTests
    {
        private HttpClient? _client;

        private WebApplicationFactory<Program>? _factory;

        [TestInitialize]
        public void Initialize()
        {
            _client = new HttpClient();

            // Create a WebApplicationFactory and HttpClient
            _factory = new WebApplicationFactory<Program>();
            _client = _factory.CreateClient();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _client?.Dispose();
        }

        [TestMethod]
        public async Task GetTodoItems_ReturnsSuccessStatusCode()
        {
            // Arrange
            var requestUri = "/api/TodoItem";

            // Act
            var response = await _client!.GetAsync(requestUri);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task GetTodoItems_ReturnsExpectedJsonArray()
        {
            // Arrange
            var requestUri = "/api/TodoItem";

            // Act
            var response = await _client!.GetAsync(requestUri);
            var content = await response.Content.ReadAsStringAsync();
            var todoItems = JsonConvert.DeserializeObject<List<TodoItem>>(content);

            // Assert
            Assert.IsNotNull(todoItems);
            Assert.AreEqual(4, todoItems!.Count);
        }

        [TestMethod]
        public async Task GetTodoItem_ReturnsSuccessStatusCode()
        {
            // Arrange
            var requestUri = "/api/TodoItem/1";

            // Act
            var response = await _client!.GetAsync(requestUri);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task GetTodoItem_ReturnsExpectedJsonObject()
        {
            // Arrange
            var requestUri = "/api/TodoItem/1";

            // Act
            var response = await _client!.GetAsync(requestUri);
            var content = await response.Content.ReadAsStringAsync();
            var todoItem = JsonConvert.DeserializeObject<TodoItem>(content);

            // Assert
            Assert.IsNotNull(todoItem);
            Assert.AreEqual(1, todoItem!.Id);
            Assert.AreEqual("First item", todoItem.Name);
        }

        [TestMethod]
        public async Task DeleteTodoItem_ReturnsSuccessStatusCode()
        {
            // Arrange
            var requestUri = "/api/TodoItem/1";

            // Act
            var response = await _client!.DeleteAsync(requestUri);

            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        [TestMethod]
        public async Task DeleteTodoItem_RemovesItemFromDatabase()
        {
            // Arrange
            var requestUri = "/api/TodoItem/1";

            // Act
            await _client!.DeleteAsync(requestUri);

            // Assert
            var response = await _client!.GetAsync("/api/TodoItem");
            var content = await response.Content.ReadAsStringAsync();
            var todoItems = JsonConvert.DeserializeObject<List<TodoItem>>(content);

            Assert.IsNotNull(todoItems);
            Assert.AreEqual(3, todoItems!.Count);
        }

        [TestMethod]
        public async Task DeleteTodoItem_ReturnsNotFoundForNonExistentItem()
        {
            // Arrange
            var requestUri = "/api/TodoItem/5";

            // Act
            var response = await _client!.DeleteAsync(requestUri);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task GetTodoItem_ReturnsNotFoundForNonExistentItem()
        {
            // Arrange
            var requestUri = "/api/TodoItem/5";

            // Act
            var response = await _client!.GetAsync(requestUri);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task GetTodoItem_ReturnsBadRequestForInvalidId()
        {
            // Arrange
            var requestUri = "/api/TodoItem/invalid";

            // Act
            var response = await _client!.GetAsync(requestUri);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task GetTodoItem_ReturnsBadRequestForNegativeId()
        {
            // Arrange
            var requestUri = "/api/TodoItem/-1";

            // Act
            var response = await _client!.GetAsync(requestUri);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task GetTodoItem_ReturnsBadRequestForZeroId()
        {
            // Arrange
            var requestUri = "/api/TodoItem/0";

            // Act
            var response = await _client!.GetAsync(requestUri);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

    }
}
