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
    if (objectResult != null)
    {
      var controllerMethod = $"{context.ActionDescriptor.RouteValues["controller"]}/{context.ActionDescriptor.RouteValues["action"]}";
      var queryString = context.HttpContext.Request.QueryString.Value;
      logger.LogInformation($"{controllerMethod}{queryString} -> {JsonSerializer.Serialize(objectResult.Value, jsonSerializerOptions)}");
    }
    else if (context.Exception != null)
    {
      logger.LogError(new EventId(11, context.Controller.ToString()), JsonSerializer.Serialize(new
      {
        context.Exception.Message,
        context.Exception.StackTrace,
      }), "HEEEEEEE", "sdfsdsds");
      context.Result = new BadRequestObjectResult("Internal Server Error");
      context.ExceptionHandled = true;
    }

    // If ModelState is invalid then return a badrequest
    else if (!context.ModelState.IsValid)
    {
      logger.LogWarning(GetActionEventId(context.HttpContext.Request.Method, context.HttpContext.Request.Path), JsonSerializer.Serialize(context.ModelState));
      context.Result = new BadRequestObjectResult(context.ModelState);
    }
  }

  private static JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
  {
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
  };

  private static EventId GetActionEventId(string action, string path)
  {
    switch (action.ToUpper())
    {
      case "GET": return new EventId(100001, path);
      case "POST": return new EventId(100002, path);
      case "PUT": return new EventId(100003, path);
      case "DELETE": return new EventId(100004, path);
      default: return new EventId(100000, $"{action}:{path}");
    }
  }
}