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
  public void OnActionExecuting(ActionExecutingContext context)
  {
    Console.WriteLine("OnActionExecuting");
  }

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
      }));
      context.Result = new BadRequestObjectResult("Hello");
      context.ExceptionHandled = true;
    }
    // If ModelState is invalid then return a badrequest
    else if (!context.ModelState.IsValid)
    {
      logger.LogWarning(new EventId(11, context.Controller.ToString()), JsonSerializer.Serialize(context.ModelState));
      context.Result = new BadRequestObjectResult(context.ModelState);
    }
  }

  private static JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
  {
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
  };
}