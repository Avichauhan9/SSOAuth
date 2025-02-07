
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace SSO_Backend.Utilities;

public class HttpGlobalExceptionFilter : IExceptionFilter
{
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<HttpGlobalExceptionFilter> _logger;
    private readonly ProblemDetailsFactory _problemDetailsFactory;

    public HttpGlobalExceptionFilter(IWebHostEnvironment env, ILogger<HttpGlobalExceptionFilter> logger, ProblemDetailsFactory problemDetailsFactory)
    {
        _env = env;
        _logger = logger;
        _problemDetailsFactory = problemDetailsFactory;
    }

    public void OnException(ExceptionContext context)
    {
        string correlationId = Guid.NewGuid().ToString();
        _logger.LogError(new EventId(context.Exception.HResult),
            context.Exception,
            $"Correlation Id:{correlationId} => {context.Exception.Message}");

        ProblemDetails problemDetails;
        int statusCode = (int)HttpStatusCode.InternalServerError;

        if (context.Exception is UserNotFoundException)
        {
            statusCode = (int)HttpStatusCode.Unauthorized;
        }
        else if (context.Exception is PermissionsNotFoundException || context.Exception is UnAuthorizedException || context.Exception is UserInActiveException)
        {
            statusCode = (int)HttpStatusCode.Forbidden;
        }
        problemDetails = _problemDetailsFactory.CreateProblemDetails(
               context.HttpContext,
               statusCode,
               title: context.Exception.Message,
               detail: _env.IsDevelopment() ? $"Correlation Id:{correlationId} => {context.Exception}" : correlationId
        );

        context.Result = new ObjectResult(problemDetails);
        context.HttpContext.Response.StatusCode = statusCode;
        context.ExceptionHandled = true;
    }
}
