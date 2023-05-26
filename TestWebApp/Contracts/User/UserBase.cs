using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TestWebApp.Contracts.User;

public class UserBase
{
    [Required]
    [JsonPropertyName("userId")]
    public int Id { get; set; }
    
    [Required]
    [JsonPropertyName("fullName")]
    public string FullName { get; set; }
}
