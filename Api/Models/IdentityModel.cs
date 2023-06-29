
namespace Tzather.Identity.Api.Models;

public class IdentityModel
{
  public double ExpireMinutes { get; set; }
  public string Audience { get; set; } = "";
  public string Issuer { get; set; } = "";
  public string PrivateKey { get; set; } = "";
  public string PublicKey { get; set; } = "";
  public string SymmetricKey { get; set; } = "";
}
