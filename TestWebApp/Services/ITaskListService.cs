using TestWebApp.Models.TaskList;

namespace TestWebApp.Services;

public interface ITaskListService
{
    Task<TaskListModel> CreateAsync(AddTaskListCommand command, CancellationToken cancellationToken);
    
    Task<TaskListModel?> UpdateAsync(UpdateTaskListCommand command, CancellationToken cancellationToken);
    
    Task<bool> DeleteAsync(DeleteTaskListCommand command, CancellationToken cancellationToken);
    
    Task<TaskListFullModel?> GetByIdAsync(GetByIdTaskListCommand command, CancellationToken cancellationToken);
    
    Task<TaskListsPaginationModel> GetAllAsync(GetAllTaskListCommand command, CancellationToken cancellationToken);
        
    Task<bool> AddConnectionAsync(AddConnectionCommand command, CancellationToken cancellationToken);
    
    Task<bool> DeleteConnectionAsync(DeleteConnectionCommand command, CancellationToken cancellationToken);
}
