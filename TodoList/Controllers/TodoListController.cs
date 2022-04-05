using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

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
    public ActionResult<IEnumerable<TodoTaskDto>> SearchAll()
    {
        var result = _databaseContext.TodoTasks.ToList();

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

    [HttpGet]
    [Route("export-excel")]
    public async Task<IActionResult> ExportExcel()
    {
        var memoryStream = new MemoryStream();
        using (var document = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
        {
            var workbookPart = document.AddWorkbookPart();
            workbookPart.Workbook = new Workbook
            {
                Sheets = new Sheets()
            };
            var workSheetPart = workbookPart.AddNewPart<WorksheetPart>();
            workSheetPart.Worksheet = new Worksheet();
            workbookPart.Workbook.Sheets.Append(new Sheet
            {
                Id = document.WorkbookPart.GetIdOfPart(workSheetPart), SheetId = 1, Name = "待辦事項"
            });
            workbookPart.Workbook.Save();
            var sheetData = workSheetPart.Worksheet.AppendChild(new SheetData());
            AddTaskDataTo(sheetData);
            workbookPart.Workbook.Save();
        }

        memoryStream.Seek(0, SeekOrigin.Begin);
        return new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    private void AddTaskDataTo(OpenXmlElement sheet)
    {
        AddTaskDataHeader(sheet);
        foreach (var todoTask in _databaseContext.TodoTasks)
        {
            var row = new Row();
            row.Append(
                CreateCell(todoTask.Id ?? -1),
                CreateCell(todoTask.Title),
                CreateCell(todoTask.Description),
                CreateCell(todoTask.Status),
                CreateCell(todoTask.ExpirationDate),
                CreateCell(todoTask.EmergencyLevel),
                CreateCell(todoTask.CreateDate ?? DateTime.Now)
            );
            sheet.AppendChild(row);
        }
    }

    private static Cell CreateCell(int value)
    {
        return new Cell
        {
            CellValue = new CellValue(value.ToString()),
            DataType = new EnumValue<CellValues>(CellValues.Number)
        };
    }

    private static Cell CreateCell(TaskStatus status)
    {
        var statusString = status switch
        {
            TaskStatus.Completed => "已完成",
            TaskStatus.WaitResponse => "待回覆",
            TaskStatus.Processing => "處理中",
            TaskStatus.NotProcessed => "未處理",
            _ => "Undefined"
        };
        return CreateCell(statusString);
    }

    private static Cell CreateCell(DateTime value)
    {
        return new Cell
        {
            CellValue = new CellValue(value),
            DataType = new EnumValue<CellValues>(CellValues.Date)
        };
    }

    private static Cell CreateCell(EmergencyLevel emergencyLevel)
    {
        var emergencyLevelString = emergencyLevel switch
        {
            EmergencyLevel.TopPriority => "最優先",
            EmergencyLevel.Normal => "普通",
            EmergencyLevel.FuturePlan => "未來計畫",
            _ => "Undefined"
        };
        return CreateCell(emergencyLevelString);
    }

    private void AddTaskDataHeader(OpenXmlElement sheet)
    {
        var header = new Row();
        header.Append(
            CreateCell("編號"),
            CreateCell("標題"),
            CreateCell("內容"),
            CreateCell("狀態"),
            CreateCell("期限"),
            CreateCell("緊急程度"),
            CreateCell("建立日期")
        );
        sheet.AppendChild(header);
    }

    private static Cell CreateCell(string value)
    {
        return new Cell
        {
            CellValue = new CellValue(value),
            DataType = new EnumValue<CellValues>(CellValues.String)
        };
    }
}
