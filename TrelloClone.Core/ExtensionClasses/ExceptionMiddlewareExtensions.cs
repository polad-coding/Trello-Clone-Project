using Microsoft.AspNetCore.Builder;
using TrelloClone.Core.CustomExceptionMiddleware;

namespace TrelloClone.Core.ExtensionClasses
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
