using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace TestItemsWebAPI.ExHandlers
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "An unhandled exception occurred");

            context.Result = new JsonResult(new { error = "An unexpected error occurred. Please try again later." })
            {
                StatusCode = (int)System.Net.HttpStatusCode.InternalServerError
            };

            context.ExceptionHandled = true;
        }
    }
}
