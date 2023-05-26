using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace TestWebApp.Contracts;

public class PaginationQueryRequest
{
    [FromQuery(Name = "itemsPerPage")]
    [Range(1, int.MaxValue, ErrorMessage = "itemsPerPage value must be greater than 0")]
    public int? ItemsPerPage { get; set; }

    [FromQuery(Name = "page")]
    [Range(1, int.MaxValue, ErrorMessage = "page value must be greater than 0")]
    public int? Page { get; set; }
}
