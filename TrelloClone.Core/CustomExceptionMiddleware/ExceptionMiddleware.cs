using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using TrelloClone.Core.Models;

namespace TrelloClone.Core.CustomExceptionMiddleware
{
    public class ExceptionMiddleware
    {
        private RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
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
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception.HResult;

            return context.Response.WriteAsync(new ErrorDetailsModel 
            { 
                Message = exception.Message,
                StatusCode = context.Response.StatusCode 
            }.ToString());
        }
    }
}
