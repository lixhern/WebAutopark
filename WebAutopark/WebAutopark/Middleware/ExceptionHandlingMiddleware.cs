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
                context.Response.Redirect($"/Error/NotFound?message={Uri.EscapeDataString(ex.Message)}");
            }
            catch (AlreadyExistException ex)
            {
                context.Response.Redirect($"/Error/AlreadyExist?message={Uri.EscapeDataString(ex.Message)}");
            }
            catch (Exception ex)
            {
                context.Response.Redirect($"/Error/Unexpected");
            }
        }
    }
}
