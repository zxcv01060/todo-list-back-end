using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TodoList.DTOs;
using TaskStatus = TodoList.DTOs.TaskStatus;

namespace TodoList.Entites;

[Table("todo_task", Schema = "todo_list")]
public record TodoTask(
    [property: Key] int? Id,
    string Title,
    string Description,
    TaskStatus Status,
    [property: Column("expiration_date")] DateTime ExpirationDate,
    [property: Column("emergency_level")] EmergencyLevel EmergencyLevel,
    [property: Column("create_date")] DateTime? CreateDate,
    [property: Column("modify_date")] DateTime? ModifyDate
);