using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

public class Todo
{
    public int Id { get; set; }
    public string Task { get; set; }
}

public class TodoDbContext : DbContext
{
    public DbSet<Todo> Todos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("TodoDatabase");
    }
}

public class TodoService
{
    private readonly TodoDbContext _dbContext;

    public TodoService(TodoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void AddTodo(string task)
    {
        _dbContext.Todos.Add(new Todo { Task = task });
        _dbContext.SaveChanges();
    }

    public void ShowTodos()
    {
        var todos = _dbContext.Todos.ToList();

        Console.WriteLine("Todos:");
        foreach (var todo in todos)
        {
            Console.WriteLine($"{todo.Id}. {todo.Task}");
        }
    }
}

class Program
{
    static void Main()
    {
        var serviceProvider = new ServiceCollection()
            .AddDbContext<TodoDbContext>()
            .AddScoped<TodoService>()
            .BuildServiceProvider();

        using (var scope = serviceProvider.CreateScope())
        {
            var todoService = scope.ServiceProvider.GetRequiredService<TodoService>();

            todoService.AddTodo("Alışveriş yap");
            todoService.AddTodo("Ders çalış");

            todoService.ShowTodos();
        }
    }
}
