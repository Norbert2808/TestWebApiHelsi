namespace TestWebApp.Models.TaskList;

public class AddTaskListCommand
{
    public int UserId { get; set; }

    public string Name { get; set; }
}