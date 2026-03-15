using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Server.Models;
using Server.Services;

namespace Server.Middleware
{
    public class AuthMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task Invoke(HttpContext context, UserService userService)
        {
            string? header = context.Request.Headers.Authorization.FirstOrDefault();

            if (header != null && header.StartsWith("Bearer "))
            {
                string token = header["Bearer ".Length..];
                User user = userService.GetByToken(token);

                if (user != null)
                    context.Items["User"] = user;
            }

            await _next(context);
        }
    }
}

namespace Server.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class RequireAuthAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.Items["User"] is not User user)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
