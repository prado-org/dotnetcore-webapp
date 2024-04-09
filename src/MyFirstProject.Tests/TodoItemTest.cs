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

        //criar teste para o metodo GetTodoItem
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

        //criar um teste que o retorno http seja 404 quando o id não existir
        [TestMethod]
        public async Task GetTodoItem_ReturnsNotFoundStatusCode()
        {
            // Arrange
            var requestUri = "/api/TodoItem/100";

            // Act
            var response = await _client!.GetAsync(requestUri);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
