using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TestWebApp.Constants;
using TestWebApp.Services;

namespace TestWebApp.Filters;

public class UserResourceFilter : IAsyncResourceFilter
{
    private readonly IUserService _userService;
    
    public UserResourceFilter(IUserService userService)
    {
        _userService = userService;
    }

    public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }
        
        if (context.HttpContext.Request.Headers.TryGetValue(HeaderConstants.UserId, out var userIdStr)
            && int.TryParse(userIdStr, out var userId)
            && await _userService.GetByIdAsync(userId, CancellationToken.None) is not null)
        {
            await next(); 
            return;
        }
        
        context.Result = new JsonResult(new { errorMessage = ResponseConstants.UserWasNotFound })
        {
            StatusCode = (int)HttpStatusCode.Forbidden
        };
    }
}
