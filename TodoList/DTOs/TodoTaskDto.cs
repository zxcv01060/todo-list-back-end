using System.Text.Json.Serialization;
using TodoList.Converters;

namespace TodoList.DTOs;

public record TodoTaskDto(
    int Id,
    string Title,
    string Description,
    TaskStatus Status,
    [property: JsonConverter(typeof(DateTimeConverter))]
    DateTime ExpirationDate,
    EmergencyLevel EmergencyLevel,
    DateTime CreateDate
);