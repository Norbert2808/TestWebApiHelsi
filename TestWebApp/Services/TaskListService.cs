using TestWebApp.Models;
using TestWebApp.Models.TaskList;
using TestWebApp.Repositories;

namespace TestWebApp.Services;

public class TaskListService : ITaskListService
{
    private readonly ITaskListRepository _taskListRepository;

    public TaskListService(ITaskListRepository taskListRepository)
    {
        _taskListRepository = taskListRepository;
    }
    public async Task<TaskListModel> CreateAsync(AddTaskListCommand command, CancellationToken cancellationToken)
    {
        return await _taskListRepository.CreateAsync(command, cancellationToken);
    }

    public async Task<TaskListModel?> UpdateAsync(UpdateTaskListCommand command, CancellationToken cancellationToken)
    {
        var taskList = await _taskListRepository.GetByIdAsync(command.Id, cancellationToken);
        return HasPermission(command.UserId, taskList)
            ? await _taskListRepository.UpdateAsync(command, cancellationToken)
            : null;
    }

    public async Task<bool> DeleteAsync(DeleteTaskListCommand command, CancellationToken cancellationToken)
    { 
        var taskList = await _taskListRepository.GetByIdAsync(command.Id, cancellationToken);
        if (taskList is null || taskList.Owner.Id != command.UserId)
        {
            return false;
        }
        
        await _taskListRepository.DeleteAsync(command.Id, cancellationToken);
        return true;
    }

    public async Task<TaskListFullModel?> GetByIdAsync(GetByIdTaskListCommand command, CancellationToken cancellationToken)
    {
        var taskList = await _taskListRepository.GetByIdAsync(command.Id, cancellationToken);
        return HasPermission(command.UserId, taskList) ? taskList : null;
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
        return result;
    }

    public async Task<bool> AddConnectionAsync(AddConnectionCommand command, CancellationToken cancellationToken)
    {
        var taskList = await _taskListRepository.GetByIdAsync(command.Id, cancellationToken);
        if (!HasPermission(command.UserId, taskList))
        {
            return false;
        }

        await _taskListRepository.AddConnectionAsync(command, cancellationToken);
        return true;
    }

    public async Task<bool> DeleteConnectionAsync(DeleteConnectionCommand command, CancellationToken cancellationToken)
    {
        var taskList = await _taskListRepository.GetByIdAsync(command.Id, cancellationToken);
        if (!HasPermission(command.UserId, taskList))
        {
            return false;
        }

        await _taskListRepository.DeleteConnectionAsync(command, cancellationToken);
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
