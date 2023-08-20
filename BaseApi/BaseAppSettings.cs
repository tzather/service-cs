namespace Tzather.BaseApi;

public class BaseAppSettings
{
  public string Name { get; set; } = "";
  public string Version { get; set; } = "";
  public string CorsOrigin { get; set; } = "";
  public IdentityModel Identity { get; set; } = new IdentityModel();
  public string LogDbContext { get; set; } = "";

  public BaseAppSettings(IConfiguration configuration)
  {
    if (configuration != null)
    {
      configuration.GetSection("AppSettings")?.Bind(this);
      LogDbContext = configuration.GetConnectionString("LogDbContext") ?? throw new ArgumentNullException("LogDbContext is required");
    }
  }
}
