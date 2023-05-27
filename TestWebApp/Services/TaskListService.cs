using TestWebApp.Models;
using TestWebApp.Models.TaskList;
using TestWebApp.Repositories;

namespace TestWebApp.Services;

public class TaskListService : ITaskListService
{
    private readonly ITaskListRepository _taskListRepository;
    private readonly ILogger<TaskListService> _logger;

    public TaskListService(ITaskListRepository taskListRepository, ILogger<TaskListService> logger)
    {
        _taskListRepository = taskListRepository;
        _logger = logger;
    }
    
    public async Task<TaskListModel> CreateAsync(AddTaskListCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("User {userId} created a TaskList with the name '{name}'.", command.UserId, command.Name);
        return await _taskListRepository.CreateAsync(command, cancellationToken);
    }

    public async Task<TaskListModel?> UpdateAsync(UpdateTaskListCommand command, CancellationToken cancellationToken)
    {
        var taskList = await _taskListRepository.GetByIdAsync(command.Id, cancellationToken);
        if (!HasPermission(command.UserId, taskList))
        {
            _logger.LogInformation("User {userId} tried to update a TaskList {id}, but he don't have permission.", command.UserId, command.Id);
            return null;
        }
        
        _logger.LogInformation("User {userId} updated a TaskList {id}.", command.UserId, command.Id);
        return await _taskListRepository.UpdateAsync(command, cancellationToken);
    }

    public async Task<bool> DeleteAsync(DeleteTaskListCommand command, CancellationToken cancellationToken)
    { 
        var taskList = await _taskListRepository.GetByIdAsync(command.Id, cancellationToken);
        if (taskList is null || taskList.Owner.Id != command.UserId)
        {
            _logger.LogInformation("User {userId} tried to delete a TaskList {id}, but he don't have permission.", command.UserId, command.Id);
            return false;
        }
        
        _logger.LogInformation("User {userId} deleted a TaskList {id}.", command.UserId, command.Id);
        await _taskListRepository.DeleteAsync(command.Id, cancellationToken);
        return true;
    }

    public async Task<TaskListFullModel?> GetByIdAsync(GetByIdTaskListCommand command, CancellationToken cancellationToken)
    {
        var taskList = await _taskListRepository.GetByIdAsync(command.Id, cancellationToken);
        if (!HasPermission(command.UserId, taskList))
        {
            _logger.LogInformation("User {userId} tried to get a TaskList {id}, but he don't have permission.", command.UserId, command.Id);
            return null;
        }
        
        _logger.LogInformation("User {userId} received a TaskList {id}.", command.UserId, command.Id);
        return taskList;
    }

    public async Task<TaskListsPaginationModel> GetAllAsync(GetAllTaskListCommand command, CancellationToken cancellationToken)
    {
        var taskLists = await _taskListRepository.GetAllAsync(cancellationToken);
        var orderedTaskList = taskLists
            .Where(it => HasPermission(command.UserId, it))
            .OrderByDescending(it => it.Id)
            .ToList();

        var result = new TaskListsPaginationModel();
        result.TaskLists = GetPaginationResult(command, result, orderedTaskList);
        _logger.LogInformation("User {userId} received all TaskLists for which he has permission.", command.UserId);
        return result;
    }

    public async Task<bool> AddConnectionAsync(AddConnectionCommand command, CancellationToken cancellationToken)
    {
        var taskList = await _taskListRepository.GetByIdAsync(command.Id, cancellationToken);
        if (!HasPermission(command.UserId, taskList))
        {
            _logger.LogInformation(
                "User {userId} tried to add a connection between User {connectionUserId} and TaskList {id}, but he don't have permission.",
                command.UserId, command.ConnectionUserId, command.Id);
            return false;
        }

        await _taskListRepository.AddConnectionAsync(command, cancellationToken);
        _logger.LogInformation("User {userId} added a connection between User {connectionUserId} and TaskList {id}.", 
            command.UserId, command.ConnectionUserId, command.Id);
        return true;
    }

    public async Task<bool> DeleteConnectionAsync(DeleteConnectionCommand command, CancellationToken cancellationToken)
    {
        var taskList = await _taskListRepository.GetByIdAsync(command.Id, cancellationToken);
        if (!HasPermission(command.UserId, taskList))
        {
            _logger.LogInformation(
                "User {userId} tried to delete a connection between User {connectionUserId} and TaskList {id}, but he don't have permission.",
                command.UserId, command.ConnectionUserId, command.Id);
            return false;
        }

        await _taskListRepository.DeleteConnectionAsync(command, cancellationToken);
        _logger.LogInformation("User {userId} deleted a connection between User {connectionUserId} and TaskList {id}.",
            command.UserId, command.ConnectionUserId, command.Id);
        return true;
    }

    private static bool HasPermission(int userId, TaskListFullModel? taskList)
    {
        return taskList is not null && (taskList.Owner.Id == userId || taskList.SharedUsers.Any(it => it.Id == userId));
    }

    private static ICollection<T> GetPaginationResult<T>(PaginationBaseModel request, PaginationResultModel result, ICollection<T> collection)
        where T : class
    {
        result.ItemsPerPage = request.ItemsPerPage;
        result.ItemsCount = collection.Count;
        var pagesCount = result.ItemsCount / result.ItemsPerPage;
        result.PagesCount = pagesCount == 0
            ? 1 
            : (result.ItemsCount % result.ItemsPerPage == 0) 
                ? pagesCount 
                : pagesCount + 1;
        result.Page = (result.PagesCount >= request.Page) ? request.Page : result.PagesCount;

        return collection.Skip(result.ItemsPerPage * (result.Page - 1)).Take(result.ItemsPerPage).ToList();
    }
}
