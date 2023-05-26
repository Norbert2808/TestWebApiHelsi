using TestWebApp.Models.TaskList;

namespace TestWebApp.Repositories;

public interface ITaskListRepository
{
    Task<TaskListModel> CreateAsync(AddTaskListCommand command, CancellationToken cancellationToken);
    
    Task<TaskListModel?> UpdateAsync(UpdateTaskListCommand command, CancellationToken cancellationToken);
    
    Task DeleteAsync(int id, CancellationToken cancellationToken);
    
    Task<TaskListFullModel?> GetByIdAsync(int id, CancellationToken cancellationToken);
    
    Task<IReadOnlyCollection<TaskListFullModel>> GetAllAsync(CancellationToken cancellationToken);
    
    Task AddConnectionAsync(AddConnectionCommand command, CancellationToken cancellationToken);
    
    Task DeleteConnectionAsync(DeleteConnectionCommand command, CancellationToken cancellationToken);
}
