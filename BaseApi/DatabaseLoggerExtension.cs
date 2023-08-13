// using Microsoft.Extensions.Logging;
// using System.Collections.Concurrent;
// using System.Runtime.Versioning;
// using Microsoft.Extensions.Options;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.DependencyInjection.Extensions;
// using Microsoft.Extensions.Logging.Configuration;

// namespace Tzather.BaseApi;

// public static class DatabaseLoggerExtension
// {
//   public static ILoggingBuilder AddCustomDatabaseLogger(this ILoggingBuilder builder)
//   {
//     builder.AddConfiguration();

//     builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider>(p => new DatabaseLoggerProvider("Server=localhost;Initial Catalog=Log;User Id=sa;Password=P@ssw0rd;TrustServerCertificate=true;")));
//     // LoggerProviderOptions.RegisterProviderOptions<DatabaseLoggerConfiguration, DatabaseLoggerProvider>(builder.Services);

//     return builder;
//   }
// }