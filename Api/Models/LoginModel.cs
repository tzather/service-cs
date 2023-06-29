using System.ComponentModel.DataAnnotations;

namespace Tzather.Identity.Api.Models;

public class LoginModel
{
  [Required]
  [StringLength(50, MinimumLength = 5)]
  public string UserName { get; set; } = "";

  [Required]
  [StringLength(20, MinimumLength = 7)]
  public string Password { get; set; } = "";

  public bool RememberMe { get; set; }
}
