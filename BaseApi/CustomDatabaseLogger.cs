using System.Collections.Concurrent;
using System.Runtime.Versioning;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Data.SqlClient;
using System.Text.Json;
using System.Text.Encodings.Web;


namespace Tzather.BaseApi;

public sealed class CustomDatabaseLoggerConfiguration
{

  public int EventId { get; set; }

  public Dictionary<LogLevel, ConsoleColor> LogLevelToColorMap { get; set; } = new()
  {
    [LogLevel.Information] = ConsoleColor.Green
  };
}


public sealed class CustomDatabaseLogger : ILogger
{
  private readonly string category;
  private readonly string connString = "Server=localhost;Initial Catalog=Log;User Id=sa;Password=P@ssw0rd;TrustServerCertificate=true;";

  private readonly string _name;
  private readonly Func<CustomDatabaseLoggerConfiguration> _getCurrentConfig;

  public CustomDatabaseLogger(string name, Func<CustomDatabaseLoggerConfiguration> getCurrentConfig) => (_name, _getCurrentConfig) = (name, getCurrentConfig);

  public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default!;

  public bool IsEnabled(LogLevel logLevel) => _getCurrentConfig().LogLevelToColorMap.ContainsKey(logLevel);

  public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
  {
    var queryString = $@"insert into dbo.Log(Updated,LogLevel,Category,EventId,EventName,State,Exception) values(@Updated,@LogLevel,@Category,@EventId,@EventName,@State,@Exception)";
    using var connection = new SqlConnection(connString);
    using var command = new SqlCommand(queryString, connection);
    command.Parameters.AddWithValue("@Updated", DateTime.UtcNow);
    command.Parameters.AddWithValue("@LogLevel", logLevel.ToString());
    command.Parameters.AddWithValue("@Category", category ?? "none");
    command.Parameters.AddWithValue("@EventId", eventId.Id);
    command.Parameters.AddWithValue("@EventName", (object)eventId.Name ?? DBNull.Value);
    command.Parameters.AddWithValue("@State", state?.ToString());
    command.Parameters.AddWithValue("@Exception", exception?.ToString() ?? "");
    // command.Parameters.AddWithValue("@Exception", exception != null ? Formatter(state, exception) : DBNull.Value);

    try
    {
      command.Connection.Open();
      command.ExecuteNonQuery();
    }
    // catch { } // Ignore exception thrown during loggging
    finally
    {
      command.Connection.Close();
    }
  }

  private string Formatter<TState>(TState state, Exception exception)
  {
    if (exception != null)
    {
      var stacktrace = new List<object>();
      var stepList = exception.StackTrace.Split(" at ");
      for (int i = 0; i < stepList.Length; i++)
      {
        string item = stepList[i].Trim();
        if (!string.IsNullOrWhiteSpace(item))
        {
          var index = item.IndexOf(" in ");
          if (index > 0)
          {
            stacktrace.Add(new { At = item.Substring(0, index).Trim(), In = item.Substring(index + 3).Trim() });
          }
          else
          {
            stacktrace.Add(new { At = item });
          }
        }
      }
      return JsonSerializer.Serialize(
        new { exception.Message, stacktrace, exception.Data },
        new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, IgnoreNullValues = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping }
      );
    }
    else
    {
      return state.ToString();
    }
  }
}


[UnsupportedOSPlatform("browser")]
[ProviderAlias("ColorConsole")]
public sealed class CustomDatabaseLoggerProvider : ILoggerProvider
{
  private readonly IDisposable? _onChangeToken;
  private CustomDatabaseLoggerConfiguration _currentConfig;
  private readonly ConcurrentDictionary<string, CustomDatabaseLogger> _loggers = new(StringComparer.OrdinalIgnoreCase);

  public CustomDatabaseLoggerProvider(IOptionsMonitor<CustomDatabaseLoggerConfiguration> config)
  {
    _currentConfig = config.CurrentValue;
    _onChangeToken = config.OnChange(updatedConfig => _currentConfig = updatedConfig);
  }

  public ILogger CreateLogger(string categoryName) => _loggers.GetOrAdd(categoryName, name => new CustomDatabaseLogger(name, GetCurrentConfig));

  private CustomDatabaseLoggerConfiguration GetCurrentConfig() => _currentConfig;

  public void Dispose()
  {
    _loggers.Clear();
    _onChangeToken?.Dispose();
  }
}

public static class CustomDatabaseLoggerExtensions
{
  public static ILoggingBuilder AddCustomDatabaseLogger(this ILoggingBuilder builder)
  {
    builder.AddConfiguration();
    builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, CustomDatabaseLoggerProvider>());
    LoggerProviderOptions.RegisterProviderOptions<CustomDatabaseLoggerConfiguration, CustomDatabaseLoggerProvider>(builder.Services);
    return builder;
  }

  public static ILoggingBuilder AddCustomDatabaseLogger(this ILoggingBuilder builder, Action<CustomDatabaseLoggerConfiguration> configure)
  {
    builder.AddCustomDatabaseLogger();
    builder.Services.Configure(configure);
    return builder;
  }
}


