using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tzather.BaseApi;
using Tzather.Identity.Api.Entities;

namespace Tzather.Identity.Api;

public class Program
{
  private static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);
    var appSetting = new AppSetting(builder.Configuration);
    AddDatabase<IIdentityDbContext, IdentityDbContext>(builder.Services, appSetting.IdentityDbContext);
    builder.Services.AddIdentity<UserEntity, RoleEntity>().AddEntityFrameworkStores<IdentityDbContext>().AddDefaultTokenProviders();

    builder
      .AddServices(appSetting.Name, appSetting.Version, appSetting.CorsOrigin, appSetting.Identity)
      .BuildApp(appSetting.Name, appSetting.Version, appSetting.CorsOrigin)
      .Run();
  }


  public static void AddDatabase<ITContext, TContext>(IServiceCollection services, string connectionString)
    where ITContext : class
    where TContext : DbContext, ITContext
  {
    services.AddDbContext<TContext>(options => options
      .UseSqlServer(connectionString)
      .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking) // don't track entities
      .EnableSensitiveDataLogging() // log sql param values
    );
    services.AddScoped<ITContext, TContext>();
  }
}