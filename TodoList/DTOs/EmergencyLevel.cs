using System.Text.Json.Serialization;

namespace TodoList.DTOs;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EmergencyLevel
{
    TopPriority,
    Normal,
    FuturePlan
}