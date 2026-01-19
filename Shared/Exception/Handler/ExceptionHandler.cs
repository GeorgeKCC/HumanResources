namespace Shared.Exception.Handler
{
    public class ExceptionHandler(ILogger<ExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, System.Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError("Error Message: {exceptionMessage}, Time of occurrence {time}", exception.Message, DateTime.UtcNow);
            (string Detail, string Title, int StatusCode, string ErrorCode) = exception switch
            {
                NotFoundCustomException =>
                (
                    exception.Message,
                    exception.GetType().Name,
                    httpContext.Response.StatusCode = StatusCodes.Status404NotFound,
                    "404"
                ),
                UnauthorizedAccessException =>
                (
                    "You are not authorized.",
                    exception.GetType().Name,
                    httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized,
                    "401"
                ),
                ValidationException =>
                (
                    exception.Message,
                    exception.GetType().Name,
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest,
                    "001"
                ),
                DbUpdateConcurrencyException =>
                (
                    "The information to be updated was affected by another transaction.",
                    exception.GetType().Name,
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest,
                    "002"
                ),
                ExistColaboratorCustomException => (
                    exception.Message,
                    exception.GetType().Name,
                    httpContext.Response.StatusCode = StatusCodes.Status404NotFound,
                    "003"
                ),
                _ =>
                (
                   exception.Message,
                   exception.GetType().Name,
                   httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError,
                   "500"
                )
            };

            var problemDetails = new ProblemDetails
            {
                Title = Title,
                Detail = Detail,
                Status = StatusCode,
                Instance = httpContext.Request.Path
            };

            problemDetails.Extensions.Add("traceId", httpContext.TraceIdentifier);

            if (exception is ValidationException validationException)
            {
                problemDetails.Extensions.Add("ValidationErrors", validationException.Errors);
            }

            var response = new GenericResponseFailed<ProblemDetails>(ErrorCode, Detail, problemDetails);

            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken: cancellationToken);
            return true;
        }
    }
}
