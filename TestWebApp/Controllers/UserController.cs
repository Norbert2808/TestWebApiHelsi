using System.Globalization;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using TestWebApp.Constants;
using TestWebApp.Contracts.User;
using TestWebApp.Mappers;
using TestWebApp.Services;

namespace TestWebApp.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(IActionResult), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Create(AddUserRequest requestModel, CancellationToken cancellationToken)
    {
        var createdUser = await _userService.CreateAsync(requestModel.ToAddCommand(), cancellationToken);

        return Ok(createdUser.ToResponse());
    }

    [HttpPost]
    [ProducesResponseType(typeof(IActionResult), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(IActionResult), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Update(UpdateUserRequest requestModel, CancellationToken cancellationToken)
    {
        var updatedUser = await _userService.UpdateAsync(requestModel.ToUpdateCommand(), cancellationToken);
        if (updatedUser is null)
        {
            return NotFound(new
            {
                errorMessage = string.Format(CultureInfo.InvariantCulture, ResponseConstants.UserWithIdWasNotFound, requestModel.Id)
            });
        }

        return Ok(updatedUser.ToResponse());
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(IActionResult), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(IActionResult), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var user = await _userService.GetByIdAsync(id, cancellationToken);
        if (user is null)
        {
            return NotFound(new
            {
                errorMessage = string.Format(CultureInfo.InvariantCulture, ResponseConstants.UserWithIdWasNotFound, id)
            });
        }

        return Ok(user.ToResponse());
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(IActionResult), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(IActionResult), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var users = await _userService.GetAllAsync(cancellationToken);
        if (users.Count == 0)
        {
            return NotFound(new
            {
                errorMessage = ResponseConstants.NoUsersFound
            });
        }

        return Ok(users.Select(it => it.ToResponse()));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(IActionResult), (int)HttpStatusCode.NoContent)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _userService.DeleteAsync(id, cancellationToken);

        return NoContent();
    }
}
