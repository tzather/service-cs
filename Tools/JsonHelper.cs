using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tzather.Tools;

public static class JsonHelper
{
  private static JsonSerializerOptions options = new JsonSerializerOptions
  {
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
  };

  public static string Serialize<TObject>(TObject obj) => JsonSerializer.Serialize(obj, options);
  public static TObject Deserialize<TObject>(string str) => JsonSerializer.Deserialize<TObject>(str, options);
}
