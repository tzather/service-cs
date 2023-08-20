using Microsoft.Data.SqlClient;

namespace Tzather.BaseApi;

public sealed class DatabaseLogger : ILogger
{
  private readonly string _connString;
  private readonly string _name;

  public DatabaseLogger(string name, string connString) => (_name, _connString) = (name, connString);

  public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default!;

  public bool IsEnabled(LogLevel logLevel) => true;

  public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
  {
    var queryString = $@"insert into dbo.Log(Updated,LogLevel,Category,EventId,EventName,State,Exception) values(@Updated,@LogLevel,@Category,@EventId,@EventName,@State,@Exception)";
    using var connection = new SqlConnection(_connString);
    using var command = new SqlCommand(queryString, connection);
    command.Parameters.AddWithValue("@Updated", DateTime.UtcNow);
    command.Parameters.AddWithValue("@LogLevel", logLevel.ToString());
    command.Parameters.AddWithValue("@Category", _name ?? "none");
    command.Parameters.AddWithValue("@EventId", eventId.Id);
    command.Parameters.AddWithValue("@EventName", (object)eventId.Name ?? DBNull.Value);
    command.Parameters.AddWithValue("@State", state?.ToString());
    command.Parameters.AddWithValue("@Exception", exception?.ToString() ?? "");

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

}

public sealed class DatabaseLoggerProvider : ILoggerProvider
{
  private string _category;
  private string _connString;

  public DatabaseLoggerProvider(string category, string connString) => (_category, _connString) = (category, connString);

  public ILogger CreateLogger(string categoryName) => new DatabaseLogger(_category, _connString);
  public void Dispose() { }
}