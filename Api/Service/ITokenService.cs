using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Tzather.Identity.Api.Services;

public interface ITokenService
{
  string Build(Guid id, IList<Claim> claims, IList<string> roles);
  void Configure(JwtBearerOptions options);
}
