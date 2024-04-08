using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using MyFirstProject.Tests.Models;
using System.Text;

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
        public async Task SaveTodoItem_CreatesNewTodoItem()
        {
            // Arrange
            var todoItem = new TodoItem { Name = "Test Todo Item", IsComplete = false };
            var json = JsonConvert.SerializeObject(todoItem);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
        
            // Act
            var response = await _client!.PostAsync("/api/TodoItem", data);
        
            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        
            var returnedTodoItem = JsonConvert.DeserializeObject<TodoItem>(await response.Content.ReadAsStringAsync());
            Assert.IsNotNull(returnedTodoItem);
            Assert.AreEqual(todoItem.Name, returnedTodoItem?.Name);
            Assert.AreEqual(todoItem.IsComplete, returnedTodoItem?.IsComplete);
        }

        [TestMethod]
        public async Task SaveTodoItem_ReturnsBadRequestForSpecialCharacters()
        {
            // Arrange
            var todoItem = new TodoItem { Name = "Test Todo Item!", IsComplete = false };
            var json = JsonConvert.SerializeObject(todoItem);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
        
            // Act
            var response = await _client!.PostAsync("/api/TodoItem", data);
        
            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task DeleteTodoItem_RemovesTodoItem()
        {
            // Arrange
            var todoItem = new TodoItem { Name = "Test Todo Item", IsComplete = false };
            var json = JsonConvert.SerializeObject(todoItem);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
        
            var response = await _client!.PostAsync("/api/TodoItem", data);
            var returnedTodoItem = JsonConvert.DeserializeObject<TodoItem>(await response.Content.ReadAsStringAsync());
        
            // Act
            var deleteResponse = await _client!.DeleteAsync($"/api/TodoItem/{returnedTodoItem?.Id}");
        
            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        }
    }
}
