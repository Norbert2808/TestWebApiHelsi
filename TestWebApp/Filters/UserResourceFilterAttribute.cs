using Microsoft.AspNetCore.Mvc;

namespace TestWebApp.Filters;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class UserResourceFilterAttribute : ServiceFilterAttribute
{
    public UserResourceFilterAttribute()
        : base(typeof(UserResourceFilter))
    {
        IsReusable = false;
    }
}
