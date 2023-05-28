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
public class CommonTaskListServiceSteps
{
    private readonly ScenarioContext _scenarioContext;
    private readonly TaskListServiceMocks _mocks;
    private readonly ErrorDriver _errorDriver;
    private bool _userHasPermission;

    public CommonTaskListServiceSteps(
        ScenarioContext scenarioContext,
        TaskListServiceMocks mocks,
        ErrorDriver errorDriver)
    {
        _scenarioContext = scenarioContext;
        _mocks = mocks;
        _errorDriver = errorDriver;
    }
    
    [Given(@"repository GetByIdAsync mock")]
    public void GivenRepositoryGetByIdAsyncMock()
    {
        var result = _userHasPermission
            ? TaskListServiceFaker.GenerateTaskListFullModel(_scenarioContext.GetUserId(), _scenarioContext.GetTaskListId())
            : null;
        
        _mocks.RepositoryMock
            .Setup(it => it.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);
        _mocks.IncrementExpectedRepositoryMockGetByIdAsync();
    }

    [Given(@"user has permission")]
    public void GivenUserHasPermission()
    {
        _userHasPermission = true;
    }

    [Given(@"user doesn't have permission")]
    public void GivenUserDoesntHavePermission()
    {
        _userHasPermission = false;
    }
    
    [Then(@"this method should return TaskList with name '(.*)'")]
    public void ThenThisMethodShouldReturnTaskListWithName(string name)
    {
        var result = _scenarioContext.GetCommandResult<TaskListModel>();

        result.Should().NotBeNull();
        result.Name.Should().Be(name);
    }

    [Then(@"this method should return null")]
    public void ThenThisMethodShouldReturnNull()
    {
        var result = _scenarioContext.GetCommandResult<TaskListModel>();

        result.Should().BeNull();
    }
    
    [AfterScenario]
    public void VerifyMocks()
    {
        _mocks.Verify();
    }
    
    [AfterScenario]
    public void CheckForUnexpectedExceptionsAfterEachScenario()
    {
        _errorDriver.AssertNoUnexpectedExceptionsRaised();
    }
}