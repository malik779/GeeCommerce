using System;
using System.Net;
using System.Text.Json;
using Gee.Core.Logs;
using Microsoft.AspNetCore.Http;
namespace Gee.Core.Middlewares
{
    public class ListenToOnlyApiGateway
    {
        private readonly RequestDelegate next;
        public ListenToOnlyApiGateway(RequestDelegate _next)
        {
            next = _next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            //default error /exception
            string message = "Unavailable";
            int statusCode = (int)HttpStatusCode.ServiceUnavailable;
            string title = "Unavailable";
            string signHeader = context.Request.Headers["Api-Gateway"].FirstOrDefault();

            if (signHeader is null)
            {
                title = "Unavailable";
                message = "Sorry, service is unavailable or you are not using the right gateway";
                statusCode = (int)HttpStatusCode.ServiceUnavailable;
                await ModifyHeader(context, title, message, statusCode);
                return;

            }
            
                await next(context);
        }

        private async Task ModifyHeader(HttpContext context, string title, string message, int statusCode)
        {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(new { Data = message, Status = statusCode, Message = title }), CancellationToken.None);
                return;
        }
    }
}
