using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Text.Json;

namespace Tzather.BaseApi;

public class ExceptionFilter : IExceptionFilter
{
  private ILogger _logger;

  public ExceptionFilter(ILogger<ExceptionFilter> logger)
  {
    _logger = logger;
  }

  public void OnException(ExceptionContext context)
  {
    // Log the exception
    _logger.LogError(new EventId(0, context?.HttpContext?.User?.Identity?.Name), JsonSerializer.Serialize(context?.Exception));
    // Return the exception message without the stacktrace to the user
    HttpResponse response = context.HttpContext.Response;
    response.StatusCode = (int)HttpStatusCode.BadRequest;
    response.WriteAsync(context.Exception.Message);
    context.ExceptionHandled = true;
  }
}