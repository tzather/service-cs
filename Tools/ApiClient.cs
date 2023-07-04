using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Zuhid.Tools.Models;

namespace Zuhid.Tools;

public abstract class ApiClient : IApiClient
{
  protected readonly HttpClient httpClient;
  protected readonly string baseUrl;
  protected string authorization;
  protected readonly ILogger logger;

  protected JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
  {
    PropertyNameCaseInsensitive = true
  };
  protected List<KeyValuePair<string, string>> customHeaders = new List<KeyValuePair<string, string>>();

  public ApiClient(IApiClientOptions apiClientOptions, HttpClient httpClient, ILogger<ApiClient> logger)
  {
    baseUrl = apiClientOptions.BaseUrl;
    authorization = apiClientOptions.Authorization;
    this.httpClient = httpClient;
    this.logger = logger;
  }

  // public async Task<string> GetContent(string url) {
  //   var request = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}{url}");
  //   request.Headers.Add("Authorization", authorization);
  //   int statusCode = -1;
  //   string content = null;
  //   try {
  //     var response = await httpClient.SendAsync(request);
  //     content = response.Content != null ? (await response.Content.ReadAsStringAsync()) : null;
  //   } catch (Exception ex) {
  //     Log(LogLevel.Error, new {
  //       Url = $"{baseUrl}{url}",
  //       StatusCode = statusCode,
  //       Message = ex.Message,
  //       StackTrace = ex.StackTrace,
  //       Content = content,
  //     });
  //   }
  //   return content;
  // }


  public abstract Task<bool> Login();

  public async Task<ApiResponse<TModel>> Get<TModel>(string url) where TModel : class
  {
    var apiResponse = new ApiResponse<TModel>();
    if (await Login())
    {
      var request = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}/{url}");
      customHeaders.ForEach(item =>
      {
        if (request.Headers.Contains(item.Key))
        {
          request.Headers.Remove(item.Key);
        }
        request.Headers.Add(item.Key, item.Value);
      });
      try
      {
        var response = await httpClient.SendAsync(request);
        if (response != null)
        {
          apiResponse.StatusCode = response.StatusCode;
          apiResponse.Content = response.Content != null ? await response.Content.ReadAsStringAsync() : null;
          if (response.IsSuccessStatusCode && !string.IsNullOrWhiteSpace(apiResponse.Content))
          {
            apiResponse.Model = JsonSerializer.Deserialize<TModel>(apiResponse.Content, jsonSerializerOptions);
          }
          else
          {
            Log(LogLevel.Warning, new
            {
              Url = $"{baseUrl}/{url}",
              apiResponse.StatusCode,
              apiResponse.Content
            });
          }
        }
      }
      catch (Exception ex)
      {
        Log(LogLevel.Error, new
        {
          Url = $"{baseUrl}/{url}",
          apiResponse.StatusCode,
          ex.Message,
          ex.StackTrace,
          apiResponse.Content,
        });
      }
    }
    return apiResponse;
  }

  protected void Log(LogLevel logLevel, object message) => logger.Log(logLevel, JsonSerializer.Serialize(message, new JsonSerializerOptions
  {
    WriteIndented = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
  }));
}
