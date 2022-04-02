using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using TodoList.Converters;

namespace TodoList.DTOs;

public class TodoTaskForm
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "請輸入標題")]
    [StringLength(50, ErrorMessage = "標題不得輸入超過50個字")]
    public string? Title { get; set; }
    public string? Description { get; set; }
    public TaskStatus? Status { get; set; }
    [JsonConverter(typeof(DateTimeConverter)), Required(ErrorMessage = "請選擇最終期限")]
    public DateTime? ExpirationDate { get; set; }
    [Required(ErrorMessage = "請選擇緊急程度")]
    public EmergencyLevel? EmergencyLevel { get; set; }
}
