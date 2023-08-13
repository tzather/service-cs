namespace Tzather.BaseApi;

public static class DatabaseLoggerFactory
{
  public static ILoggerFactory AddDatabase(this ILoggerFactory factory, string connString)
  {
    Console.WriteLine("aaaaaaaaaaaaaaaaaaaaaaaaaaaa = ");

    factory.AddProvider(new DatabaseLoggerProvider(connString));
    return factory;
  }
}
