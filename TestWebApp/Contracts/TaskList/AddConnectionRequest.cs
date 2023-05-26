using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TestWebApp.Contracts.TaskList;

public class AddConnectionRequest
{
    [Required]
    [JsonPropertyName("taskListId")]
    public int Id { get; set; }
    
    [Required]
    [JsonPropertyName("connectionUserId")]
    public int ConnectionUserId { get; set; }
}
