using BehaviourTests.Drivers;
using BehaviourTests.Fakers;
using BehaviourTests.Mocks;
using BehaviourTests.ScenarioContextExtensions;
using FluentAssertions;
using TestWebApp.Models.TaskList;

namespace BehaviourTests.Steps.TaskListService;

[Binding]
[Scope(Feature = "TaskListService")]
public class GetByIdTaskListSteps
{
    private readonly ScenarioContext _scenarioContext;
    private readonly TaskListServiceMocks _mocks;
    private readonly ErrorDriver _errorDriver;

    public GetByIdTaskListSteps(
        ScenarioContext scenarioContext,
        TaskListServiceMocks mocks,
        ErrorDriver errorDriver)
    {
        _scenarioContext = scenarioContext;
        _mocks = mocks;
        _errorDriver = errorDriver;
    }

    [Given(@"there is GetByIdTaskListCommand with id (.*)")]
    public void GivenThereIsGetByIdTaskListCommandWithId(int id)
    {
        var request = TaskListServiceFaker.RandomGetByIdTaskListCommand.Generate();
        request.Id = id;
        
        _scenarioContext.SetCommand(request);
        _scenarioContext.SetUserId(request.UserId);
        _scenarioContext.SetTaskListId(request.Id);
    }

    [When(@"ITaskListService.GetByIdAsync method executed")]
    public void WhenITaskListServiceGetByIdAsyncMethodExecuted()
    {
        var service = _mocks.CreateTaskListService();
        var request = _scenarioContext.GetCommand<GetByIdTaskListCommand>();
        
        if (_errorDriver.TryExecuteAsync(() => service.GetByIdAsync(request, default), out var result))
        {
            _scenarioContext.SetCommandResult(result);
        }
    }

    [Then(@"this method should return TaskListFullModel with id (.*)")]
    public void ThenThisMethodShouldReturnTaskListFullModelWithId(int id)
    {
        var result = _scenarioContext.GetCommandResult<TaskListFullModel>();

        result.Should().NotBeNull();
        result.Id.Should().Be(id);
    }
}