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
            Assert.AreEqual(4, todoItems!.Count);
        }

        [TestMethod]
        public async Task CreateTodoItem_ReturnsSuccessStatusCode()
        {
            // Arrange
            var requestUri = "/api/TodoItem";
            var newTodoItem = new TodoItem { Name = "New Task", IsComplete = false };
            var content = new StringContent(JsonConvert.SerializeObject(newTodoItem), Encoding.UTF8, "application/json");

            // Act
            var response = await _client!.PostAsync(requestUri, content);

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        [TestMethod]
        public async Task CreateTodoItem_ReturnsValidationErrorForInvalidName()
        {
            // Arrange
            var requestUri = "/api/TodoItem";
            var newTodoItem = new TodoItem { Name = "Invalid@Name", IsComplete = false };
            var content = new StringContent(JsonConvert.SerializeObject(newTodoItem), Encoding.UTF8, "application/json");

            // Act
            var response = await _client!.PostAsync(requestUri, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.IsTrue(responseContent.Contains("The Name field is required, cannot contain special characters, and must be less than 255 characters."));
        }

        [TestMethod]
        public async Task CreateTodoItem_ReturnsConfirmationMessage()
        {
            // Arrange
            var requestUri = "/api/TodoItem";
            var newTodoItem = new TodoItem { Name = "New Task", IsComplete = false };
            var content = new StringContent(JsonConvert.SerializeObject(newTodoItem), Encoding.UTF8, "application/json");

            // Act
            var response = await _client!.PostAsync(requestUri, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.IsTrue(responseContent.Contains("New Task"));
        }
    }
}
