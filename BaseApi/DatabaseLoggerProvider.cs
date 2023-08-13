namespace Tzather.BaseApi;

public class DatabaseLoggerProvider : ILoggerProvider
{
  private readonly string connString;

  public DatabaseLoggerProvider(string connString)
  {
    Console.WriteLine("aaaaaaaaaaaaaaaaaaaaaaaaaaaa DatabaseLoggerProvider = " + connString);

    this.connString = connString;
  }

  public ILogger CreateLogger(string categoryName) => new DatabaseLogger(categoryName, connString);

  public void Dispose() { }
}
