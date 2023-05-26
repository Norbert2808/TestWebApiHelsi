using System.Text.Json.Serialization;

namespace TestWebApp.Contracts;

public class PaginationResponse
{
    [JsonPropertyName("itemsPerPage")]
    public int ItemsPerPage { get; set; }

    [JsonPropertyName("page")]
    public int Page { get; set; }

    [JsonPropertyName("pagesCount")]
    public int PagesCount { get; set; }

    [JsonPropertyName("itemsCount")]
    public int ItemsCount { get; set; }
}
