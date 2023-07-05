using System.Net;

namespace Tzather.Tools.Models;

public class ApiResponse<TModel> where TModel : class
{
  public HttpStatusCode StatusCode { get; set; }
  public string Content { get; set; }
  public TModel Model { get; set; }
}
