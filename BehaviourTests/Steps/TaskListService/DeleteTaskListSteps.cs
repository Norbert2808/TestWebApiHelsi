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
public class DeleteTaskListSteps
{
    private readonly ScenarioContext _scenarioContext;
    private readonly TaskListServiceMocks _mocks;
    private readonly ErrorDriver _errorDriver;
    private bool _result;

    public DeleteTaskListSteps(
        ScenarioContext scenarioContext,
        TaskListServiceMocks mocks,
        ErrorDriver errorDriver)
    {
        _scenarioContext = scenarioContext;
        _mocks = mocks;
        _errorDriver = errorDriver;
    }

    [Given(@"there is DeleteTaskListCommand")]
    public void GivenThereIsDeleteTaskListCommand()
    {
        var request = TaskListServiceFaker.RandomDeleteTaskListCommand.Generate();
        
        _scenarioContext.SetCommand(request);
        _scenarioContext.SetUserId(request.UserId);
    }

    [Given(@"repository DeleteAsync mock")]
    public void GivenRepositoryDeleteAsyncMock()
    {
        var request = _scenarioContext.GetCommand<DeleteTaskListCommand>();

        _mocks.RepositoryMock
            .Setup(it => it.DeleteAsync(request.Id, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mocks.IncrementExpectedRepositoryMockDeleteAsync();
    }

    [When(@"ITaskListService.DeleteAsync method executed")]
    public void WhenITaskListServiceDeleteAsyncMethodExecuted()
    {
        var service = _mocks.CreateTaskListService();
        var request = _scenarioContext.GetCommand<DeleteTaskListCommand>();
        
        if (_errorDriver.TryExecuteAsync(() => service.DeleteAsync(request, default), out var result))
        {
            _result = result;
        }
    }

    [Then(@"TaskList should be successfully deleted")]
    public void ThenTaskListShouldBeSuccessfullyDeleted()
    {
        _result.Should().BeTrue();
    }

    [Then(@"TaskList should not be deleted")]
    public void ThenTaskListShouldNotBeDeleted()
    {
        _result.Should().BeFalse();
    }
}