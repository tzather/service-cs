using Zuhid.Tools.Models;

namespace Zuhid.Tools;

public interface IApiClient
{
  // Task<string> GetContent(string url);
  Task<bool> Login();

  Task<ApiResponse<TModel>> Get<TModel>(string url) where TModel : class;
}
