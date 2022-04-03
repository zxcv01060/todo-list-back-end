using Microsoft.AspNetCore.Mvc;

using TodoList.Configs;
using TodoList.DTOs;
using TodoList.Entites;
using TodoList.Extensions;

using TaskStatus = TodoList.DTOs.TaskStatus;

namespace TodoList.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoListController : ControllerBase
{
    private readonly TodoListDatabaseContext _databaseContext;

    public TodoListController(TodoListDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoTaskDto>>> SearchAll()
    {
        var result = await _databaseContext.TodoTasks.FindAsync(ConvertEntityToDto);

        return Ok(result);
    }

    private static TodoTaskDto ConvertEntityToDto(TodoTask task)
    {
        return new TodoTaskDto(
            (int) task.Id!,
            task.Title,
            task.Description,
            task.Status,
            task.ExpirationDate,
            task.EmergencyLevel,
            (DateTime) task.CreateDate!
        );
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<TodoTaskDto>> SearchById(int id)
    {
        var result = await _databaseContext.TodoTasks.FindAsync(id);

        if (result == null)
        {
            return NotFound();
        }

        return ConvertEntityToDto(result);
    }

    [HttpPost]
    public async Task<ActionResult<TodoTaskDto>> Create([FromBody] TodoTaskForm todoTaskForm)
    {
        if (ModelState.IsNotValid())
        {
            return BadRequest(ModelState);
        }

        var task = ConvertFormToEntity(todoTaskForm);
        _databaseContext.TodoTasks.Add(task);
        await _databaseContext.SaveChangesAsync();

        return CreatedAtAction(nameof(SearchById), new {id = task.Id}, ConvertEntityToDto(task));
    }

    private static TodoTask ConvertFormToEntity(TodoTaskForm form)
    {
        return new TodoTask(
            null,
            form.Title!,
            form.Description!,
            form.Status ?? TaskStatus.NotProcessed,
            form.ExpirationDate!.Value,
            form.EmergencyLevel!.Value,
            DateTime.Now,
            DateTime.Now
        );
    }
}
