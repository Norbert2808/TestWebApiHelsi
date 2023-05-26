using TestWebApp.Models.User;

namespace TestWebApp.Models.TaskList;

public class TaskListFullModel : TaskListModel
{
    public IEnumerable<UserModel> SharedUsers { get; set; }
}