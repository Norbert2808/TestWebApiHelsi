using TestWebApp.Contracts;

namespace TestWebApp.Models.TaskList;

public class GetAllTaskListCommand : PaginationBaseModel
{
    public GetAllTaskListCommand(PaginationQueryRequest pagination, int userId)
    {
        UserId = userId;
        ItemsPerPage = pagination.ItemsPerPage ?? int.MaxValue;
        Page = pagination.Page ?? 1;
    }
    
    public int UserId { get; set; }
}
