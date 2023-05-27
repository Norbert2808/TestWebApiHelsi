using System.Globalization;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using TestWebApp.Constants;
using TestWebApp.Contracts;
using TestWebApp.Contracts.TaskList;
using TestWebApp.Filters;
using TestWebApp.Mappers;
using TestWebApp.Models.TaskList;
using TestWebApp.Services;

namespace TestWebApp.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[UserResourceFilter]
[ProducesResponseType((int)HttpStatusCode.Forbidden)]
public class TaskListController : ControllerBase
{
    private readonly ITaskListService _taskListService;

    public TaskListController(ITaskListService taskListService)
    {
        _taskListService = taskListService;
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(IActionResult), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(IActionResult), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Create(
        AddTaskListRequest requestModel, 
        [FromHeader(Name = HeaderConstants.UserId)] int userId,
        CancellationToken cancellationToken)
    {
        var createdTaskList = await _taskListService.CreateAsync(requestModel.ToAddCommand(userId), cancellationToken);

        return Ok(createdTaskList.ToResponse());
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(IActionResult), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(IActionResult), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Update(
        UpdateTaskListRequest requestModel, 
        [FromHeader(Name = HeaderConstants.UserId)] int userId,
        CancellationToken cancellationToken)
    {
        var updatedTaskList = await _taskListService.UpdateAsync(requestModel.ToUpdateCommand(userId), cancellationToken);
        if (updatedTaskList is null)
        {
            return BadRequest(new
            {
                errorMessage = string.Format(CultureInfo.InvariantCulture, ResponseConstants.DontHavePermission, requestModel.Id)
            });
        }

        return Ok(updatedTaskList.ToResponse());
    }
    
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(IActionResult), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(IActionResult), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Delete(
        int id,
        [FromHeader(Name = HeaderConstants.UserId)] int userId,
        CancellationToken cancellationToken)
    {
        var wasDeleted = await _taskListService.DeleteAsync(new DeleteTaskListCommand { Id = id, UserId = userId }, cancellationToken);
        if (!wasDeleted)
        {
            return BadRequest(new
            {
                errorMessage = string.Format(CultureInfo.InvariantCulture, ResponseConstants.YouAreNotOwnerOfTaskList, id)
            });
        }

        return Ok(new
        {
            responseMessage = string.Format(CultureInfo.InvariantCulture, ResponseConstants.TaskListSuccessfullyDeleted, id)
        });
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(IActionResult), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(IActionResult), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetById(
        int id, 
        [FromHeader(Name = HeaderConstants.UserId)] int userId,
        CancellationToken cancellationToken)
    {
        var taskList = await _taskListService.GetByIdAsync(new GetByIdTaskListCommand { Id = id, UserId = userId }, cancellationToken);
        if (taskList is null)
        {
            return BadRequest(new
            {
                errorMessage = string.Format(CultureInfo.InvariantCulture, ResponseConstants.DontHavePermission, id)
            });
        }

        return Ok(taskList.ToFullResponse());
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(IActionResult), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAll(
        [FromHeader(Name = HeaderConstants.UserId)] int userId,
        [FromQuery] PaginationQueryRequest pagination,
        CancellationToken cancellationToken)
    {
        var taskLists = await _taskListService.GetAllAsync(new GetAllTaskListCommand(pagination, userId), cancellationToken);

        return Ok(taskLists.ToPaginationResponse());
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(IActionResult), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(IActionResult), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> AddConnection(
        AddConnectionRequest requestModel, 
        [FromHeader(Name = HeaderConstants.UserId)] int userId,
        CancellationToken cancellationToken)
    {
        var wasAdded = await _taskListService.AddConnectionAsync(requestModel.ToAddConnectionCommand(userId), cancellationToken);
        if (!wasAdded)
        {
            return BadRequest(new
            {
                errorMessage = string.Format(CultureInfo.InvariantCulture, ResponseConstants.DontHavePermission, requestModel.Id)
            });
        }

        return Ok(new
        {
            responseMessage = string.Format(CultureInfo.InvariantCulture, ResponseConstants.ConnectionHasBeenAdded, requestModel.ConnectionUserId, requestModel.Id)
        });
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(IActionResult), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(IActionResult), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetConnections(
        int id, 
        [FromHeader(Name = HeaderConstants.UserId)] int userId,
        CancellationToken cancellationToken)
    {
        var taskList = await _taskListService.GetByIdAsync(new GetByIdTaskListCommand { Id = id, UserId = userId }, cancellationToken);
        if (taskList is null)
        {
            return BadRequest(new
            {
                errorMessage = string.Format(CultureInfo.InvariantCulture, ResponseConstants.DontHavePermission, id)
            });
        }

        return Ok(taskList.ToConnectionsResponse());
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(IActionResult), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(IActionResult), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> DeleteConnection(
        DeleteConnectionRequest requestModel, 
        [FromHeader(Name = HeaderConstants.UserId)] int userId,
        CancellationToken cancellationToken)
    {
        var wasDeleted = await _taskListService.DeleteConnectionAsync(requestModel.ToDeleteConnectionCommand(userId), cancellationToken);
        if (!wasDeleted)
        {
            return BadRequest(new
            {
                errorMessage = string.Format(CultureInfo.InvariantCulture, ResponseConstants.DontHavePermission, requestModel.Id)
            });
        }

        return Ok(new
        {
            responseMessage = string.Format(CultureInfo.InvariantCulture, ResponseConstants.ConnectionHasBeenDeleted, requestModel.ConnectionUserId, requestModel.Id)
        });
    }
}
