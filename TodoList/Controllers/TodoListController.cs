using Microsoft.AspNetCore.Mvc;
using TodoList.DTOs;
using TaskStatus = TodoList.DTOs.TaskStatus;

namespace TodoList.Controllers;

[ApiController]
[Route("todo-list")]
public class TodoListController : ControllerBase
{
    [HttpGet]
    public IEnumerable<TodoTaskDto> SearchAll()
    {
        return new[]
        {
            new TodoTaskDto(1,
                "foo",
                "bar",
                TaskStatus.Processing,
                DateTime.Now,
                EmergencyLevel.FuturePlan,
                DateTime.Now
            ),
            new TodoTaskDto(2,
                "foo bar",
                "test",
                TaskStatus.WaitResponse,
                DateTime.Now,
                EmergencyLevel.TopPriority,
                DateTime.Now
            ),
            new TodoTaskDto(
                3,
                "Hello World!",
                "Hello C# .NET!",
                TaskStatus.Completed,
                DateTime.Today,
                EmergencyLevel.Normal,
                DateTime.Now
            )
        };
    }

    private void Foo()
    {
    }
}