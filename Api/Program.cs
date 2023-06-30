using Tzather.Identity.Api.Extensions;

namespace Tzather.Identity.Api;

public class Program
{
  private static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);
    var appSetting = new AppSetting(builder.Configuration);
    builder
      .AddServices(appSetting.Name, appSetting.Version, appSetting.CorsOrigin, appSetting.Identity)
      .BuildApp(appSetting.Name, appSetting.Version, appSetting.CorsOrigin)
      .Run();
  }
}