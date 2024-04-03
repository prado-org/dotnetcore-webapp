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
        public async Task PostTodoItem_ReturnsCreatedTodoItem()
        {
            // Arrange
            var requestUri = "/api/TodoItem";
            var newTodoItem = new TodoItem { Name = "Test Todo Item" };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(newTodoItem), Encoding.UTF8, "application/json");

            // Act
            var response = await _client!.PostAsync(requestUri, jsonContent);
            var content = await response.Content.ReadAsStringAsync();
            var createdTodoItem = JsonConvert.DeserializeObject<TodoItem>(content);

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.IsNotNull(createdTodoItem);
            Assert.AreEqual(newTodoItem.Name, createdTodoItem!.Name);
        }

        
    }
}
