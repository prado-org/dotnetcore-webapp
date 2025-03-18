using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MyFirstProject.TodoApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var isDevelopment = builder.Environment.IsDevelopment();

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ToDo API",
        Description = "An ASP.NET Core Web API for managing ToDo items",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });
});

string connString = builder.Configuration.GetConnectionString("DefaultConnection");

//builder.Services.AddDbContext<TodoItemContext>(opt =>
//    opt.UseSqlServer(connString));

// use in-memory database
//builder.Services.AddDbContext<TodoItemContext>(opt =>
//    opt.UseInMemoryDatabase("TodoList"));

builder.Services.AddDbContext<TodoItemContext>(options =>
{
    if (isDevelopment)
    {
        //options.UseInMemoryDatabase("MyInMemoryDb");
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    }
    else
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    }
});

// Configure JSON options
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    //app.UseSwagger();
    //app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

// removido por causa do Codespaces
//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// create databse if not exists
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<TodoItemContext>();
        context.Database.EnsureCreated();

        var _categories = new List<Category>
        {
            new Category { Name = "Work" },
            new Category { Name = "Home" },
            new Category { Name = "Hobby" },
            new Category { Name = "Health" },
            new Category { Name = "Finance" }
        };

        // check if any Categories exist, if not, add some
        if (!context.Categories.Any())
        {
            context.Categories.AddRange(_categories);
            context.SaveChanges();
        }
        
        // Check if any TodoItems exist, if not, add some
        if (!context.TodoItems.Any())
        {
            context.TodoItems.Add(new TodoItem { Name = "Task 1", IsComplete = false, DueDate = DateTime.Now.AddDays(7), Category = _categories.ElementAt(0) });
            context.TodoItems.Add(new TodoItem { Name = "Task 2", IsComplete = true, DueDate = DateTime.Now.AddDays(14), Category = _categories.ElementAt(1) });
            context.TodoItems.Add(new TodoItem { Name = "Task 3", IsComplete = true, DueDate = DateTime.Now.AddDays(21), Category = _categories.ElementAt(2) });
            context.TodoItems.Add(new TodoItem { Name = "Task 4", IsComplete = true, DueDate = DateTime.Now.AddDays(28), Category = _categories.ElementAt(3) });
            context.TodoItems.Add(new TodoItem { Name = "Task 5", IsComplete = false, DueDate = DateTime.Now.AddDays(35), Category = _categories.ElementAt(4) });
            context.TodoItems.Add(new TodoItem { Name = "Task 6", IsComplete = true, DueDate = DateTime.Now.AddDays(42), Category = _categories.ElementAt(0) });
            context.TodoItems.Add(new TodoItem { Name = "Task 7", IsComplete = false, DueDate = DateTime.Now.AddDays(49), Category = _categories.ElementAt(1) });
            context.TodoItems.Add(new TodoItem { Name = "Task 8", IsComplete = true, DueDate = DateTime.Now.AddDays(56), Category = _categories.ElementAt(2) });
            context.TodoItems.Add(new TodoItem { Name = "Task 9", IsComplete = false, DueDate = DateTime.Now.AddDays(63), Category = _categories.ElementAt(3) });
            context.TodoItems.Add(new TodoItem { Name = "Task 10", IsComplete = true, DueDate = DateTime.Now.AddDays(70), Category = _categories.ElementAt(4) });
            context.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");
    }
}

app.Run();

public partial class Program {}