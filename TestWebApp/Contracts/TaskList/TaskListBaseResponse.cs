using System.Text.Json.Serialization;
using TestWebApp.Contracts.User;

namespace TestWebApp.Contracts.TaskList;

public class TaskListBaseResponse
{
    [JsonPropertyName("taskListId")]
    public int Id { get; set; }
    
    [JsonPropertyName("taskListName")]
    public string Name { get; set; }
    
    [JsonPropertyName("owner")]
    public UserResponse Owner { get; set; }
}