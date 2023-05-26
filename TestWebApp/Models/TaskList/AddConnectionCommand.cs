namespace TestWebApp.Models.TaskList;

public class AddConnectionCommand : TaskListBaseCommand
{
    public int ConnectionUserId { get; set; }
}