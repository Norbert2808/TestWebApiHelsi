namespace TestWebApp.Models.TaskList;

public class UpdateTaskListCommand : AddTaskListCommand
{
    public int Id { get; set; }
}