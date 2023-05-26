namespace TestWebApp.Models;

public class PaginationResultModel
{
    public int ItemsPerPage { get; set; }

    public int Page { get; set; }

    public int PagesCount { get; set; }

    public int ItemsCount { get; set; }
}
