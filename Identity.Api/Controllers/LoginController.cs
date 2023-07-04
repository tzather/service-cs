using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tzather.BaseApi;
using Tzather.Identity.Api.Entities;
using Tzather.Identity.Api.Models;

namespace Tzather.Identity.Api.Controllers;

[Route("[controller]")]
public class LoginController : ControllerBase
{
  private readonly SignInManager<UserEntity> signInManager;
  private readonly UserManager<UserEntity> userManager;
  private readonly ITokenService tokenService;

  public LoginController(SignInManager<UserEntity> signInManager, UserManager<UserEntity> userManager, ITokenService tokenService)
  {
    this.signInManager = signInManager;
    this.userManager = userManager;
    this.tokenService = tokenService;
  }

  /// <summary>
  /// Allow the user to login
  /// </summary>
  /// <param name="model"></param>
  /// <returns></returns>
  /// <exception cref="ApplicationException"></exception>
  [AllowAnonymous]
  [HttpPost()]
  public async Task<object> Post([FromBody] LoginModel model)
  {
    var userEntity = await userManager.FindByNameAsync(model.UserName);
    if (userEntity != null)
    {
      var signInResult = await signInManager.CheckPasswordSignInAsync(userEntity, model.Password, false);
      if (signInResult.Succeeded)
      {
        var isTfaValid = await userManager.VerifyChangePhoneNumberTokenAsync(userEntity, model.Tfa, userEntity?.PhoneNumber ?? "");
        if (signInResult.RequiresTwoFactor && !isTfaValid)
        {
          return new LoginResponse { RequireTfa = true };
        }
        var claims = await userManager.GetClaimsAsync(userEntity);
        var roles = await userManager.GetRolesAsync(userEntity);
        return new LoginResponse
        {
          Token = tokenService.Build(userEntity.Id, claims, roles),
          // LandingPage = userEntity.LandingPage
        };
      }
    }
    throw new ApplicationException("Invalid Login");
  }
}
