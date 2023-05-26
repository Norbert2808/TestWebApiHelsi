using System.ComponentModel.DataAnnotations;

namespace TestWebApp.Data.Entities;

public class UserTaskList
{
    [Key]
    public int Id { get; set; }
    public int TaskListId { get; set; }
    public TaskList TaskList { get; set; } = null!;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}