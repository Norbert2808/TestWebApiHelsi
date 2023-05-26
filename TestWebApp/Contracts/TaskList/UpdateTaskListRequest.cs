using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TestWebApp.Contracts.TaskList;

public class UpdateTaskListRequest : AddTaskListRequest
{
    [Required]
    [JsonPropertyName("taskListId")]
    public int Id { get; set; }
}
