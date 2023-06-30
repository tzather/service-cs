using Tzather.Identity.Api.Models;

namespace Tzather.Identity.Api;

public class AppSetting
{
  public AppSetting(IConfiguration configuration)
  {
    if (configuration != null)
    {
      configuration.GetSection("AppSettings")?.Bind(this);
      this.IdentityDbContext = configuration.GetConnectionString("IdentityDbContext") ?? throw new ArgumentNullException("IdentityDbContext is required");
    }
  }

  public string Name { get; set; } = "";
  public string Version { get; set; } = "";
  public string CorsOrigin { get; set; } = "";
  public string IdentityDbContext { get; set; } = "";
  public IdentityModel Identity { get; set; } = new IdentityModel();
}
