using System.ComponentModel.DataAnnotations;

namespace TestWebApp.Data.Entities;

public class User
{
    [Key]
    public int Id { get; set; }
    public string FullName { get; set; }
    public List<TaskList> TaskLists { get; set; } = new();
    public List<TaskList> OwnerTaskLists { get; set; } = new();
}
