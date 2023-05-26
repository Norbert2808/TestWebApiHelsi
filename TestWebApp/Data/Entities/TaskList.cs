using System.ComponentModel.DataAnnotations;

namespace TestWebApp.Data.Entities;

public class TaskList
{
    [Key]
    public int Id { get; set; }
    [StringLength(255, MinimumLength = 1)]
    public string Name { get; set; }
    public int OwnerId { get; set; }
    public User Owner { get; set; } = null!;
    public List<User> SharedUsers { get; set; } = new();
}
