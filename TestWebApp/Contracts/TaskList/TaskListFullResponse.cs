using System.Text.Json.Serialization;
using TestWebApp.Contracts.User;

namespace TestWebApp.Contracts.TaskList;

public class TaskListFullResponse : TaskListBaseResponse
{
    [JsonPropertyName("sharedUsers")]
    public IEnumerable<UserResponse> SharedUsers { get; set; }
}
