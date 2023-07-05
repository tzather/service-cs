using Tzather.Tools.Models;

namespace Tzather.Tools;

public interface IApiClient
{
  // Task<string> GetContent(string url);
  Task<bool> Login();

  Task<ApiResponse<TModel>> Get<TModel>(string url) where TModel : class;
}
