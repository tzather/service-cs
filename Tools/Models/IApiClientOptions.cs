namespace Tzather.Tools.Models;

public interface IApiClientOptions
{
  string BaseUrl { get; set; }
  string Authorization { get; }
}
