using Microsoft.AspNetCore.Mvc;
using WebAutopark.Exceptions;

namespace WebAutopark.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _requestDelegate;

        public ExceptionHandlingMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _requestDelegate(context);
            }
            catch (NotFoundException ex)
            {
                HandleRedirect(context, "/Error/NotFound", ex.Message);
            }
            catch (Exception ex)
            {
                HandleRedirect(context, "/Error/Unexpected", "An unexpected error occurred. Please try again later.");
            }
        }

        private void HandleRedirect(HttpContext context, string redirectPath, string errorMessage)
        {
            context.Items["ErrorMessage"] = errorMessage;
            context.Response.Redirect(redirectPath);
            //return Task.CompletedTask;
        }
    }
}
