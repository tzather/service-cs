using System;
using System.ComponentModel.DataAnnotations;
// using Swashbuckle.AspNetCore.Annotations;

namespace Tzather.Identity.Api.Models;

public class UserModel
{
  public Guid Id { get; set; }

  [Required]
  [StringLength(50, MinimumLength = 7)]
  public string UserName { get; set; } = "";

  [Required]
  [StringLength(20, MinimumLength = 7)]
  public string Password { get; set; } = "";

}
