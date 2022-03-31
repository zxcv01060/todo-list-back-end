using Microsoft.EntityFrameworkCore;
using TodoList.Entites;

namespace TodoList.Configs;

public class TodoListDatabaseContext : DbContext
{
    public TodoListDatabaseContext(DbContextOptions<TodoListDatabaseContext> options) : base(options)
    {
    }

    public DbSet<TodoTask> TodoTasks { get; set; }
}