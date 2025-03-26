using System.Net;
using System.Text;
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
            //test
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
            Assert.AreEqual(10, todoItems!.Count);
        }

        // adicione o metodo de test para testar o endpoint GetTodoItemById
        [TestMethod]
        public async Task GetTodoItemById_ReturnsSuccessStatusCode()
        {
            // Arrange
            var requestUri = "/api/TodoItem/1";

            // Act
            var response = await _client!.GetAsync(requestUri);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task GetTodoItemById_ReturnsExpectedJsonObject()
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
        }

        // adicione o metodo de test para testar o endpoint PutTodoItem
        [TestMethod]
        public async Task PutTodoItem_ReturnsSuccessStatusCode()
        {
            // Arrange
            var requestUri = "/api/TodoItem/1";
            var todoItem = new TodoItem
            {
                Id = 1,
                Name = "Item 1",
                IsComplete = true,
                DueDate = DateTime.Now
            };
            var json = JsonConvert.SerializeObject(todoItem);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client!.PutAsync(requestUri, content);

            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        [TestMethod]
        public async Task PutTodoItem_ReturnsBadRequestStatusCode()
        {
            // Arrange
            var requestUri = "/api/TodoItem/1";
            var todoItem = new TodoItem
            {
                Id = 1,
                Name = "Item 1!",
                IsComplete = true,
                DueDate = DateTime.Now
            };
            var json = JsonConvert.SerializeObject(todoItem);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client!.PutAsync(requestUri, content);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

    }

}
