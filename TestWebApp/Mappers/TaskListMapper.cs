using TestWebApp.Contracts.TaskList;
using TestWebApp.Data.Entities;
using TestWebApp.Models.TaskList;

namespace TestWebApp.Mappers;

public static class TaskListMapper
{
    public static AddTaskListCommand ToAddCommand(this AddTaskListRequest request, int userId)
    {
        return new AddTaskListCommand
        {
            Name = request.Name,
            UserId = userId
        };
    }
    
    public static TaskListBaseResponse ToResponse(this TaskListModel model)
    {
        return new TaskListBaseResponse
        {
            Id = model.Id,
            Name = model.Name,
            Owner = model.Owner.ToResponse()
        };
    }
        
    public static TaskList ToPostgresqlEntity(this AddTaskListCommand command)
    {
        return new TaskList
        {
            Name = command.Name,
            OwnerId = command.UserId
        };
    }
        
    public static TaskListModel ToTaskListModel(this TaskList taskList)
    {
        return new TaskListModel
        {
            Id = taskList.Id,
            Name = taskList.Name,
            Owner = taskList.Owner.ToUserModel()
        };
    }
    
    public static UpdateTaskListCommand ToUpdateCommand(this UpdateTaskListRequest request, int userId)
    {
        return new UpdateTaskListCommand
        {
            Id = request.Id,
            Name = request.Name,
            UserId = userId
        };
    }
    
    public static TaskListFullModel ToTaskListFullModel(this TaskList taskList)
    {
        return new TaskListFullModel
        {
            Id = taskList.Id,
            Name = taskList.Name,
            Owner = taskList.Owner.ToUserModel(),
            SharedUsers = taskList.SharedUsers.Select(it => it.ToUserModel()).ToList()
        };
    }
    
    public static TaskListFullResponse ToFullResponse(this TaskListFullModel model)
    {
        return new TaskListFullResponse
        {
            Id = model.Id,
            Name = model.Name,
            Owner = model.Owner.ToResponse(),
            SharedUsers = model.SharedUsers.Select(it => it.ToResponse()).ToList()
        };
    }
        
    public static AddConnectionCommand ToAddConnectionCommand(this AddConnectionRequest model, int userId)
    {
        return new AddConnectionCommand
        {
            Id = model.Id,
            UserId = userId,
            ConnectionUserId = model.ConnectionUserId
        };
    }
    
    public static UserTaskList ToPostgresqlEntity(this AddConnectionCommand model)
    {
        return new UserTaskList
        {
            TaskListId = model.Id,
            UserId = model.ConnectionUserId,
        };
    }
    
    public static TaskListConnectionsResponse ToConnectionsResponse(this TaskListFullModel model)
    {
        return new TaskListConnectionsResponse
        {
            Id = model.Id,
            SharedUsers = model.SharedUsers.Select(it => it.ToResponse()).ToList()
        };
    }
    
    public static DeleteConnectionCommand ToDeleteConnectionCommand(this DeleteConnectionRequest model, int userId)
    {
        return new DeleteConnectionCommand
        {
            Id = model.Id,
            UserId = userId,
            ConnectionUserId = model.ConnectionUserId
        };
    }
    
    public static TaskListsPaginationResponse ToPaginationResponse(this TaskListsPaginationModel model)
    {
        return new TaskListsPaginationResponse
        {
            ItemsCount = model.ItemsCount,
            ItemsPerPage = model.ItemsPerPage,
            Page = model.Page,
            PagesCount = model.PagesCount,
            TaskLists = model.TaskLists.Select(it => it.ToResponse()),
        };
    }
}
