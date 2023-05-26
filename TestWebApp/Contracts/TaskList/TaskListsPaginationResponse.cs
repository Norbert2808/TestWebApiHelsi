using System.Text.Json.Serialization;

namespace TestWebApp.Contracts.TaskList;

public class TaskListsPaginationResponse : PaginationResponse
{
    [JsonPropertyName("taskLists")]
    public IEnumerable<TaskListBaseResponse> TaskLists { get; set; }
}
