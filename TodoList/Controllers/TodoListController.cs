using Microsoft.AspNetCore.Mvc;
using TodoList.Configs;
using TodoList.DTOs;
using TodoList.Entites;

namespace TodoList.Controllers;

[ApiController]
[Route("todo-list")]
public class TodoListController : ControllerBase
{
    private readonly TodoListDatabaseContext _databaseContext;

    public TodoListController(TodoListDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    [HttpGet]
    public IEnumerable<TodoTaskDto> SearchAll()
    {
        return _databaseContext.TodoTasks.Select(ConvertEntityToDto).ToList();
    }

    private static TodoTaskDto ConvertEntityToDto(TodoTask task)
    {
        return new TodoTaskDto(task!.Id, task.Title, task.Description,
            task.Status, task.ExpirationDate, task.EmergencyLevel, task.CreateDate);
    }
}