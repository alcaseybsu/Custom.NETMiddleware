using Microsoft.AspNetCore.Builder;
using Web.Middleware; // Ensure this using directive is correct based on your middleware's namespace

namespace Web.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseMyMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MyMiddleware>();
        }
    }
}
