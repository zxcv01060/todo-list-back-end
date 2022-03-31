using System.Text.Json.Serialization;

namespace TodoList.DTOs;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TaskStatus
{
    Completed,
    WaitResponse,
    Processing,
    NotProcessed
}