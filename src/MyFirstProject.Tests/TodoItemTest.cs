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
        public async Task PostTodoItem_ValidName_SavesTodoItem()
        {
            // Arrange
            var requestUri = "/api/TodoItem";
            var todoItem = new TodoItem { Name = "ValidName" };
            var content = new StringContent(JsonConvert.SerializeObject(todoItem), Encoding.UTF8, "application/json");
        
            // Act
            var response = await _client!.PostAsync(requestUri, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            var savedTodoItem = JsonConvert.DeserializeObject<TodoItem>(responseContent);
        
            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual(todoItem.Name, savedTodoItem.Name);
        }
        
        [TestMethod]
        public async Task PostTodoItem_NameWithSpecialCharacter_ReturnsBadRequest()
        {
            // Arrange
            var requestUri = "/api/TodoItem";
            var todoItem = new TodoItem { Name = "InvalidName!" };
            var content = new StringContent(JsonConvert.SerializeObject(todoItem), Encoding.UTF8, "application/json");
        
            // Act
            var response = await _client!.PostAsync(requestUri, content);
        
            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
