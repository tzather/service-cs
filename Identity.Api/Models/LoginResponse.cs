namespace Tzather.Identity.Api.Models;

/// <summary>
/// Respose Object when attemptign to login
/// </summary>
public class LoginResponse
{

  /// <summary>
  /// Is Tfa required
  /// </summary>
  public bool RequireTfa { get; set; } = false;

  /// <summary>
  /// Authentication token
  /// </summary>
  public string Token { get; set; } = "";

  /// <summary>
  /// LandingPage
  /// </summary>
  public string LandingPage { get; set; } = "";
}

