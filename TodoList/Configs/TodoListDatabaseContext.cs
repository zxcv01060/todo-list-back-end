using Microsoft.EntityFrameworkCore;

using TodoList.Entites;

namespace TodoList.Configs;

public class TodoListDatabaseContext : DbContext
{
    public virtual DbSet<TodoTask> TodoTasks { get; set; }

    public TodoListDatabaseContext(DbContextOptions<TodoListDatabaseContext> options) : base(options)
    {
    }
}
