using Microsoft.Extensions.Logging;
using Moq;
using TestWebApp.Models.TaskList;
using TestWebApp.Repositories;
using TestWebApp.Services;

namespace BehaviourTests.Mocks;

public class TaskListServiceMocks
{
    private int _expectedRepositoryMockCreateAsync;
    private int _expectedRepositoryMockUpdateAsync;
    private int _expectedRepositoryMockGetByIdAsync;
    private int _expectedRepositoryMockDeleteAsync;
    private int _expectedRepositoryMockGetAllAsync;
    private int _expectedRepositoryMockAddConnectionAsync;
    private int _expectedRepositoryMockDeleteConnectionAsync;

    public Mock<ITaskListRepository> RepositoryMock { get; } = new (MockBehavior.Strict);

    public ITaskListService CreateTaskListService()
    {
        return new TaskListService(RepositoryMock.Object, new Logger<TaskListService>(new LoggerFactory()));
    }
    
    public void Verify()
    {
        RepositoryMock.Verify(it => 
                it.CreateAsync(It.IsAny<AddTaskListCommand>(), It.IsAny<CancellationToken>()),
            Times.Exactly(_expectedRepositoryMockCreateAsync));
        RepositoryMock.Verify(it => 
                it.UpdateAsync(It.IsAny<UpdateTaskListCommand>(), It.IsAny<CancellationToken>()),
            Times.Exactly(_expectedRepositoryMockUpdateAsync));
        RepositoryMock.Verify(it => 
                it.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()),
            Times.Exactly(_expectedRepositoryMockGetByIdAsync));
        RepositoryMock.Verify(it => 
                it.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()),
            Times.Exactly(_expectedRepositoryMockDeleteAsync));
        RepositoryMock.Verify(it => 
                it.GetAllAsync(It.IsAny<CancellationToken>()),
            Times.Exactly(_expectedRepositoryMockGetAllAsync));
        
        RepositoryMock.Verify(it => 
                it.AddConnectionAsync(It.IsAny<AddConnectionCommand>(), It.IsAny<CancellationToken>()),
            Times.Exactly(_expectedRepositoryMockAddConnectionAsync));
        RepositoryMock.Verify(it => 
                it.DeleteConnectionAsync(It.IsAny<DeleteConnectionCommand>(), It.IsAny<CancellationToken>()),
            Times.Exactly(_expectedRepositoryMockDeleteConnectionAsync));
    }
    
    public void IncrementExpectedRepositoryMockCreateAsync()
    {
        _expectedRepositoryMockCreateAsync++;
    }
    
    public void IncrementExpectedRepositoryMockUpdateAsync()
    {
        _expectedRepositoryMockUpdateAsync++;
    }
    
    public void IncrementExpectedRepositoryMockGetByIdAsync()
    {
        _expectedRepositoryMockGetByIdAsync++;
    }
    
    public void IncrementExpectedRepositoryMockDeleteAsync()
    {
        _expectedRepositoryMockDeleteAsync++;
    }
    
    public void IncrementExpectedRepositoryMockGetAllAsync()
    {
        _expectedRepositoryMockGetAllAsync++;
    }
    
    public void IncrementExpectedRepositoryMockAddConnectionAsync()
    {
        _expectedRepositoryMockAddConnectionAsync++;
    }
    
    public void IncrementExpectedRepositoryMockDeleteConnectionAsync()
    {
        _expectedRepositoryMockDeleteConnectionAsync++;
    }
}