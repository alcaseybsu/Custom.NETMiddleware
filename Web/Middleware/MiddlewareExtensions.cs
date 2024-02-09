using Microsoft.AspNetCore.Builder;
using Web.Middleware; 

namespace Web.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthMiddleware>();
        }
    }
}
