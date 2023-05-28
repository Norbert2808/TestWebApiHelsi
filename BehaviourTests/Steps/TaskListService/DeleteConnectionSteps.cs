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
public class DeleteConnectionSteps
{
    private readonly ScenarioContext _scenarioContext;
    private readonly TaskListServiceMocks _mocks;
    private readonly ErrorDriver _errorDriver;
    private bool _result;

    public DeleteConnectionSteps(
        ScenarioContext scenarioContext,
        TaskListServiceMocks mocks,
        ErrorDriver errorDriver)
    {
        _scenarioContext = scenarioContext;
        _mocks = mocks;
        _errorDriver = errorDriver;
    }

    [Given(@"there is DeleteConnectionCommand")]
    public void GivenThereIsDeleteConnectionCommand()
    {
        var request = TaskListServiceFaker.RandomDeleteConnectionCommand.Generate();
        
        _scenarioContext.SetCommand(request);
        _scenarioContext.SetUserId(request.UserId);
    }

    [Given(@"repository DeleteConnectionAsync mock")]
    public void GivenRepositoryDeleteConnectionAsyncMock()
    {
        var request = _scenarioContext.GetCommand<DeleteConnectionCommand>();

        _mocks.RepositoryMock
            .Setup(it => it.DeleteConnectionAsync(request, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mocks.IncrementExpectedRepositoryMockDeleteConnectionAsync();
    }

    [When(@"ITaskListService.DeleteConnectionAsync method executed")]
    public void WhenITaskListServiceDeleteConnectionAsyncMethodExecuted()
    {
        var service = _mocks.CreateTaskListService();
        var request = _scenarioContext.GetCommand<DeleteConnectionCommand>();
        
        if (_errorDriver.TryExecuteAsync(() => service.DeleteConnectionAsync(request, default), out var result))
        {
            _result = result;
        }
    }

    [Then(@"connection should be successfully deleted")]
    public void ThenConnectionShouldBeSuccessfullyDeleted()
    {
        _result.Should().BeTrue();
    }

    [Then(@"connection should not be deleted")]
    public void ThenConnectionShouldNotBeDeleted()
    {
        _result.Should().BeFalse();
    }
}