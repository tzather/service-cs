using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Tzather.BaseApi;

public class ActionFilter : IActionFilter
{
  private readonly ILogger logger;

  public ActionFilter(ILogger<ActionFilter> logger) => this.logger = logger;
  public void OnActionExecuting(ActionExecutingContext context) { }

  public void OnActionExecuted(ActionExecutedContext context)
  {
    var objectResult = context.Result as ObjectResult;
    var controllerAction = $"{context.ActionDescriptor.RouteValues["controller"]}.{context.ActionDescriptor.RouteValues["action"]}";
    var action = $"{context.ActionDescriptor.RouteValues["action"]}";
    var queryString = context.HttpContext.Request.QueryString.Value;

    if (context.Exception != null)
    {
      logger.LogError(new EventId(0, controllerAction), JsonSerializer.Serialize(new
      {
        controllerAction,
        action,
        queryString,
        context.HttpContext.Request.Path,
        // context.HttpContext.Request.Body,
        Exception = context.Exception.Message,
        context.Exception.StackTrace,
      }, jsonSerializerOptions));

      context.Result = new BadRequestObjectResult("Internal Server Error");
      context.ExceptionHandled = true;
    }

    // If ModelState is invalid then return a badrequest
    else if (!context.ModelState.IsValid)
    {
      logger.LogWarning(new EventId(0, controllerAction), JsonSerializer.Serialize(new
      {
        ModelState = context.ModelState
      }, jsonSerializerOptions));

      context.Result = new BadRequestObjectResult(context.ModelState);
    }
    else
    {
      logger.LogInformation(new EventId(0, controllerAction), JsonSerializer.Serialize(new
      {
        Name = context.HttpContext.User?.Identity?.Name,
        objectResult = objectResult?.Value,
      }, jsonSerializerOptions));
    }
  }

  private static JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
  {
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
  };
}