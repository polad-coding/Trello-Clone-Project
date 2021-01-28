using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading.Tasks;
using TrelloClone.Core.Models;

namespace TrelloClone.WebUI.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            if (context.Response.HasStarted)
            {
                return null;
            }

            context.Response.Clear();
            context.Response.ContentType = "application/json";
            _logger.LogError(exception, exception.Message);

            try
            {
                return context.Response.WriteAsync(new ErrorDetailsModel
                {
                    Message = exception.Message
                }.ToString(), Encoding.UTF8);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to write an error to the response.");
            }

            return null;
        }
    }
}
