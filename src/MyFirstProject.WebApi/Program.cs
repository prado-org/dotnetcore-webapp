using Microsoft.EntityFrameworkCore;
using MyFirstProject.TodoApi.Models;

var builder = WebApplication.CreateBuilder(args);

var isDevelopment = builder.Environment.IsDevelopment();

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
        options.UseInMemoryDatabase("MyInMemoryDb");
    }
    else
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    }
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

        // Check if any TodoItems exist, if not, add some
        if (!context.TodoItems.Any())
        {
            context.TodoItems.Add(new TodoItem { Name = "Task 1", IsComplete = false, DueDate = DateTime.Now.AddDays(7) });
            context.TodoItems.Add(new TodoItem { Name = "Task 2", IsComplete = true, DueDate = DateTime.Now.AddDays(14) });
            context.TodoItems.Add(new TodoItem { Name = "Task 3", IsComplete = true, DueDate = DateTime.Now.AddDays(21) });
            context.TodoItems.Add(new TodoItem { Name = "Task 4", IsComplete = true, DueDate = DateTime.Now.AddDays(28) });
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