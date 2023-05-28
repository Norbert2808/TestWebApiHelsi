using BehaviourTests.Drivers;
using BehaviourTests.Fakers;
using BehaviourTests.Mocks;
using BehaviourTests.ScenarioContextExtensions;
using FluentAssertions;
using Moq;
using TestWebApp.Models.TaskList;

namespace BehaviourTests.Steps.TaskListService;

[Binding]
[Scope(Feature = "TaskListService")]
public class AddConnectionSteps
{
    private readonly ScenarioContext _scenarioContext;
    private readonly TaskListServiceMocks _mocks;
    private readonly ErrorDriver _errorDriver;
    private bool _result;

    public AddConnectionSteps(
        ScenarioContext scenarioContext,
        TaskListServiceMocks mocks,
        ErrorDriver errorDriver)
    {
        _scenarioContext = scenarioContext;
        _mocks = mocks;
        _errorDriver = errorDriver;
    }

    [Given(@"there is AddConnectionCommand")]
    public void GivenThereIsAddConnectionCommand()
    {
        var request = TaskListServiceFaker.RandomAddConnectionCommand.Generate();
        
        _scenarioContext.SetCommand(request);
        _scenarioContext.SetUserId(request.UserId);
    }

    [Given(@"repository AddConnectionAsync mock")]
    public void GivenRepositoryAddConnectionAsyncMock()
    {
        var request = _scenarioContext.GetCommand<AddConnectionCommand>();

        _mocks.RepositoryMock
            .Setup(it => it.AddConnectionAsync(request, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mocks.IncrementExpectedRepositoryMockAddConnectionAsync();
    }

    [When(@"ITaskListService.AddConnectionAsync method executed")]
    public void WhenITaskListServiceAddConnectionAsyncMethodExecuted()
    {
        var service = _mocks.CreateTaskListService();
        var request = _scenarioContext.GetCommand<AddConnectionCommand>();
        
        if (_errorDriver.TryExecuteAsync(() => service.AddConnectionAsync(request, default), out var result))
        {
            _result = result;
        }
    }

    [Then(@"connection should be successfully added")]
    public void ThenConnectionShouldBeSuccessfullyAdded()
    {
        _result.Should().BeTrue();
    }

    [Then(@"connection should not be added")]
    public void ThenConnectionShouldNotBeAdded()
    {
        _result.Should().BeFalse();
    }
}