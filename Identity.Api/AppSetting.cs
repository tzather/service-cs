using Tzather.BaseApi;

namespace Tzather.Identity.Api;

class AppSetting : BaseAppSettings
{
  public AppSetting(IConfiguration configuration) : base(configuration)
  {
    if (configuration != null)
    {
      IdentityDbContext = configuration.GetConnectionString("IdentityDbContext") ?? throw new ArgumentNullException("IdentityDbContext is required");
    }
  }
  public string IdentityDbContext { get; set; } = "";
}
