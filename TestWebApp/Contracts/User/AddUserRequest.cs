using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TestWebApp.Contracts.User;

public class AddUserRequest
{
    [Required]
    [JsonPropertyName("fullName")]
    public string FullName { get; set; }
}
