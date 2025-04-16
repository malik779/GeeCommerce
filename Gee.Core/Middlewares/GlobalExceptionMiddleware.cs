using System.Net;
using System.Text.Json;
using Gee.Core.Logs;
using Microsoft.AspNetCore.Http;
namespace Gee.Core.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate next;
        public GlobalExceptionMiddleware(RequestDelegate _next)
        {
            next = _next;
        }
        public virtual async Task InvokeAsync(HttpContext context)
        {
            //default error /exception
            string message = "sorry, internal server occurred. Kindly try again";
            int statusCode = (int)HttpStatusCode.InternalServerError;
            string title = "Error";

            try
            {
                await next(context);

                //check if exception is too many request
                if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
                {
                    title = "Warning";
                    message = "Too many request made";
                    statusCode = (int)HttpStatusCode.TooManyRequests;
                    await ModifyHeader(context,title,message,statusCode);
                }
                // if response is Unauthorized 401 
                if(context.Response.StatusCode==StatusCodes.Status401Unauthorized
                    )
                {
                    title = "Alert";
                    message = "You are not authorized to access";
                    statusCode= (int)HttpStatusCode.Unauthorized;
                    await ModifyHeader(context, title, message, statusCode);
                }

                //if response is forbidden 403
                if (context.Response.StatusCode == StatusCodes.Status403Forbidden
                   )
                {
                    title = "Out of access";
                    message = "You are not allwoed/required to access";
                    statusCode = (int)HttpStatusCode.Forbidden;
                    await ModifyHeader(context, title, message, statusCode);
                }
            }
            catch (Exception ex)
            {
                //Log Orignal Exceptions into database, file ,consle,debugger or whatever method u want
                LogExceptions.LogException(ex);
                //check if exception is timeout  408
                if (ex is TaskCanceledException || ex is TimeoutException
                  )
                {
                    title = "Out of time";
                    message = "Request time out...try again";
                    statusCode = (int)HttpStatusCode.RequestTimeout;
                   
                }
                //if none of the exception do the default error
                await ModifyHeader(context, title, message, statusCode);
            }
        }

        private async Task ModifyHeader(HttpContext context, string title, string message, int statusCode)
        {
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new {Data=message,Status=statusCode,Message=title}), CancellationToken.None);
            return;
        }
    }
}
