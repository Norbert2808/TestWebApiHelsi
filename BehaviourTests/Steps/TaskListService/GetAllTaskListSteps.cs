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
public class GetAllTaskListSteps
{
    private readonly ScenarioContext _scenarioContext;
    private readonly TaskListServiceMocks _mocks;
    private readonly ErrorDriver _errorDriver;

    public GetAllTaskListSteps(
        ScenarioContext scenarioContext,
        TaskListServiceMocks mocks,
        ErrorDriver errorDriver)
    {
        _scenarioContext = scenarioContext;
        _mocks = mocks;
        _errorDriver = errorDriver;
    }

    [Given(@"there is GetAllTaskListCommand with itemsPerPage (.*) and page (.*)")]
    public void GivenThereIsGetAllTaskListCommandWithItemsPerPageAndPage(int itemsPerPage, int page)
    {
        var command = TaskListServiceFaker.GenerateGetAllTaskListCommand(itemsPerPage, page);
        
        _scenarioContext.SetCommand(command);
        _scenarioContext.SetUserId(command.UserId);
    }

    [Given(@"user has permission for (.*) TaskLists")]
    public void GivenUserHasPermissionForTaskLists(int count)
    {
        var result = TaskListServiceFaker.RandomTaskListFullModel.Generate(count);
        var userId = _scenarioContext.GetUserId();
        
        result.ForEach(it => it.Owner.Id = userId!.Value);
        
        _mocks.RepositoryMock
            .Setup(it => it.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);
        _mocks.IncrementExpectedRepositoryMockGetAllAsync();
    }

    [When(@"ITaskListService.GetAllAsync method executed")]
    public void WhenITaskListServiceGetAllAsyncMethodExecuted()
    {
        var service = _mocks.CreateTaskListService();
        var request = _scenarioContext.GetCommand<GetAllTaskListCommand>();
        
        if (_errorDriver.TryExecuteAsync(() => service.GetAllAsync(request, default), out var result))
        {
            _scenarioContext.SetCommandResult(result);
        }
    }

    [Then(@"this method should return collection with (.*) elements")]
    public void ThenThisMethodShouldReturnCollectionWithElements(int count)
    {
        var result = _scenarioContext.GetCommandResult<TaskListsPaginationModel>();

        result.Should().NotBeNull();
        result.TaskLists.Should().HaveCount(count);
    }
}