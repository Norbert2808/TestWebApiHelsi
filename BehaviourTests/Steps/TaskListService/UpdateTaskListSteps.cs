using BehaviourTests.Drivers;
using BehaviourTests.Fakers;
using BehaviourTests.Mocks;
using BehaviourTests.ScenarioContextExtensions;
using Moq;
using TestWebApp.Models.TaskList;

namespace BehaviourTests.Steps.TaskListService;

[Binding]
[Scope(Feature = "TaskListService")]
public class UpdateTaskListSteps
{
    private readonly ScenarioContext _scenarioContext;
    private readonly TaskListServiceMocks _mocks;
    private readonly ErrorDriver _errorDriver;

    public UpdateTaskListSteps(
        ScenarioContext scenarioContext,
        TaskListServiceMocks mocks,
        ErrorDriver errorDriver)
    {
        _scenarioContext = scenarioContext;
        _mocks = mocks;
        _errorDriver = errorDriver;
    }

    [Given(@"there is UpdateTaskListCommand with name '(.*)'")]
    public void GivenThereIsUpdateTaskListCommandWithName(string name)
    {
        var request = TaskListServiceFaker.RandomUpdateTaskListCommand.Generate();
        request.Name = name;
        
        _scenarioContext.SetCommand(request);
        _scenarioContext.SetUserId(request.UserId);
    }

    [Given(@"repository UpdateAsync mock")]
    public void GivenRepositoryUpdateAsyncMock()
    {
        var request = _scenarioContext.GetCommand<UpdateTaskListCommand>();

        _mocks.RepositoryMock
            .Setup(it => it.UpdateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(TaskListServiceFaker.GenerateTaskListModel(request.Name));
        _mocks.IncrementExpectedRepositoryMockUpdateAsync();
    }

    [When(@"ITaskListService.UpdateAsync method executed")]
    public void WhenITaskListServiceUpdateAsyncMethodExecuted()
    {
        var service = _mocks.CreateTaskListService();
        var request = _scenarioContext.GetCommand<UpdateTaskListCommand>();
        
        if (_errorDriver.TryExecuteAsync(() => service.UpdateAsync(request, default), out var result))
        {
            _scenarioContext.SetCommandResult(result);
        }
    }
}