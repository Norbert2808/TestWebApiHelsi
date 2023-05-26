namespace TestWebApp.Models.TaskList;

public class TaskListsPaginationModel : PaginationResultModel
{
    public ICollection<TaskListFullModel> TaskLists { get; set; }
}
