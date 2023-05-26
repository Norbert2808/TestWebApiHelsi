using TestWebApp.Models.User;

namespace TestWebApp.Models.TaskList;

public class TaskListModel
{
    public int Id { get; set; }

    public string Name { get; set; }
    
    public UserModel Owner { get; set; }
}