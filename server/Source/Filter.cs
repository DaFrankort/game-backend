using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Server.Exceptions;

namespace Server.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            context.Result = context.Exception switch
            {
                LobbyNotFoundException or UserNotFoundException => new NotFoundObjectResult(
                    context.Exception.Message
                ),
                UserInLobbyException or UserNotInLobbyException => new BadRequestObjectResult(
                    context.Exception.Message
                ),
                _ => new ObjectResult("An unexpected error occurred.") { StatusCode = 500 },
            };
            context.ExceptionHandled = true;
        }
    }
}
