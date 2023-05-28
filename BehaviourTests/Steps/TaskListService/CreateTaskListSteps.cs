using BehaviourTests.Drivers;
using BehaviourTests.Fakers;
using BehaviourTests.Mocks;
using BehaviourTests.ScenarioContextExtensions;
using Moq;
using TestWebApp.Models.TaskList;

namespace BehaviourTests.Steps.TaskListService;

[Binding]
[Scope(Feature = "TaskListService")]
public class CreateTaskListSteps
{
    private readonly ScenarioContext _scenarioContext;
    private readonly TaskListServiceMocks _mocks;
    private readonly ErrorDriver _errorDriver;

    public CreateTaskListSteps(
        ScenarioContext scenarioContext,
        TaskListServiceMocks mocks,
        ErrorDriver errorDriver)
    {
        _scenarioContext = scenarioContext;
        _mocks = mocks;
        _errorDriver = errorDriver;
    }

    [Given(@"there is AddTaskListCommand with name '(.*)'")]
    public void GivenThereIsAddTaskListCommandWithName(string name)
    {
        var request = TaskListServiceFaker.RandomAddTaskListCommand.Generate();
        request.Name = name;
        
        _scenarioContext.SetCommand(request);
    }
    
    [Given(@"repository CreateAsync mock")]
    public void GivenRepositoryCreateAsyncMock()
    {
        var request = _scenarioContext.GetCommand<AddTaskListCommand>();

        _mocks.RepositoryMock
            .Setup(it => it.CreateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(TaskListServiceFaker.GenerateTaskListModel(request.Name));
        _mocks.IncrementExpectedRepositoryMockCreateAsync();
    }

    [When(@"ITaskListService.CreateAsync method executed")]
    public void WhenITaskListServiceCreateAsyncMethodExecuted()
    {
        var service = _mocks.CreateTaskListService();
        var request = _scenarioContext.GetCommand<AddTaskListCommand>();
        
        if (_errorDriver.TryExecuteAsync(() => service.CreateAsync(request, default), out var result))
        {
            _scenarioContext.SetCommandResult(result);
        }
    }
}