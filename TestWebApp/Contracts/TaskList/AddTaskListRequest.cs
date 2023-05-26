using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TestWebApp.Contracts.TaskList;

public class AddTaskListRequest
{
    [Required]
    [JsonPropertyName("taskListName")]
    [StringLength(255, MinimumLength = 1, ErrorMessage = "The TaskList name must be between 1 and 255 characters long.")]
    public string Name { get; set; }
}
