namespace Tzather.Identity.Api.Models;

public class LoginResponse
{
  public string Token { get; set; } = "";
  public bool RequireTfa { get; set; }
  public string LandingPage { get; set; } = "";
}

