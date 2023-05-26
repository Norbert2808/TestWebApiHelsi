using System.Text.Json.Serialization;
using TestWebApp.Contracts.User;

namespace TestWebApp.Contracts.TaskList;

public class TaskListConnectionsResponse
{
    [JsonPropertyName("taskListId")]
    public int Id { get; set; }
    
    [JsonPropertyName("sharedUsers")]
    public IEnumerable<UserResponse> SharedUsers { get; set; }
}