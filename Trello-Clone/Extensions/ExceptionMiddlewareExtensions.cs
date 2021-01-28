using Microsoft.AspNetCore.Builder;
using TrelloClone.WebUI.Middlewares;

namespace TrelloClone.WebUI.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void UseCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
