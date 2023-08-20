using Microsoft.AspNetCore.Identity;
using Tzather.BaseApi;
using Tzather.Identity.Api.Entities;

namespace Tzather.Identity.Api;

public class Program
{
  private static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);
    var appSetting = new AppSetting(builder.Configuration);
    builder.AddDatabase<IIdentityDbContext, IdentityDbContext>(builder.Services, appSetting.IdentityDbContext);
    builder.Services.AddIdentity<UserEntity, RoleEntity>().AddEntityFrameworkStores<IdentityDbContext>().AddDefaultTokenProviders();

    builder
      .AddServices(appSetting)
      .BuildApp(appSetting)
      .Run();
  }
}