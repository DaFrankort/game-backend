using Server.Models;

namespace Server.Utility
{
    public static class HttpContextUtil
    {
        public static User GetUser(this HttpContext context)
        {
            if (context.Items["User"] is not User user)
                throw new UnauthorizedAccessException("User not authenticated.");
            return user;
        }
    }
}
