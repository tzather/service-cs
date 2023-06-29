using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tzather.Identity.Api.Entities;
using Tzather.Identity.Api.Services;

namespace Tzather.Identity.Api.Controllers;

[Route("[controller]")]
public class LoginController : ControllerBase
{
  private readonly SignInManager<UserEntity> signInManager;
  private readonly UserManager<UserEntity> userManager;
  private readonly ITokenService tokenService;

  public LoginController(
  // SignInManager<UserEntity> signInManager,
  // UserManager<UserEntity> userManager,
  ITokenService tokenService)
  {
    // this.signInManager = signInManager;
    // this.userManager = userManager;
    this.tokenService = tokenService;
  }

  [AllowAnonymous]
  [HttpPost()]
  public async Task<IActionResult> Post()
  {
    return Ok(new { token = "Hello" });
  }
}
